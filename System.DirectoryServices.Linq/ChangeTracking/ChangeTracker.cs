using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.Linq.Attributes;
using System.DirectoryServices.Linq.EntryObjects;
using System.Reflection;

namespace System.DirectoryServices.Linq.ChangeTracking
{
	public class ChangeTracker : IChangeTracker
	{
		#region Constructors

		public ChangeTracker(DirectoryContext context)
		{
			Context = context;
			Changes = new Dictionary<EntryObject, List<string>>();
		}

		#endregion

		#region Properties

		public DirectoryContext Context { get; set; }

		public IDictionary<EntryObject, List<string>> Changes { get; set; }

		#endregion

		#region Methods

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var entryObject = (EntryObject)sender;

			if (!Changes.ContainsKey(entryObject))
			{
				Changes.Add(entryObject, new List<string>());
			}

			var properties = Changes[entryObject];

			if (!properties.Contains(e.PropertyName))
			{
				properties.Add(e.PropertyName);
			}
		}

		public void TrackChanges<T>(T entryObject) where T : EntryObject
		{
			if (!Equals(entryObject, default(T)))
			{
				entryObject.PropertyChanged += OnPropertyChanged;
			}
		}

		public void SubmitChanges()
		{
			foreach (var item in Changes)
			{
				var entry = item.Key;
				var values = item.Value;
				var entryType = entry.GetType();

				foreach (var propertyName in values)
				{
					var property = entryType.GetProperty(propertyName);
					var attributeName = GetAttributeName(property);
					entry.Entry.Properties[attributeName].Value = property.GetValue(entry, null);
				}

				entry.Entry.CommitChanges();
			}
		}

		private static string GetAttributeName(MemberInfo info)
		{
			var attribute = info.GetAttribute<DirectoryPropertyAttribute>();

			if (attribute != null && !string.IsNullOrEmpty(attribute.Name) && !attribute.IsReference && !attribute.IsReferenceCollection)
			{
				return attribute.Name;
			}

			return info.Name;
		}

		#endregion
	}
}