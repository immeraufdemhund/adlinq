using System.ComponentModel;
using System.DirectoryServices.Linq.Attributes;

namespace System.DirectoryServices.Linq.EntryObjects
{
	public abstract class EntryObject : DisposableObject, IEntryWithRelationships, INotifyPropertyChanged
	{
		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Fields

		private const int UF_DONT_EXPIRE_PASSWD = 0x10000;

		private IRelationshipManager _relationshipManager;

		#endregion

		#region Constructors



		#endregion

		#region Properties

		IRelationshipManager IEntryWithRelationships.RelationshipManager
		{
			get
			{
				if (_relationshipManager == null)
				{
					_relationshipManager = GetRelationshipManager();
				}

				return _relationshipManager;
			}
		}

		internal Type ElementType
		{
			get
			{
				return GetType();
			}
		}

		internal string ADPath { get; set; }

		internal DirectoryEntry Entry { get; set; }

		internal ChangeState ChangeState { get; set; }

		internal DirectoryContext Context { get; set; }

		#endregion

		#region Methods

		internal string GetCnValue()
		{
			string givenName = null;
			string surName = null;

			foreach (var property in ElementType.GetProperties())
			{
				var attr = property.GetAttribute<DirectoryPropertyAttribute>();

				if (attr == null)
				{
					continue;
				}

				switch ((attr.Name ?? string.Empty).ToLower())
				{
					case "cn":
					{
						return Convert.ToString(property.GetValue(this, null));
					}
					case "givenname":
					{
						givenName = Convert.ToString(property.GetValue(this, null));
						break;
					}
					case "sn":
					{
						surName = Convert.ToString(property.GetValue(this, null));
						break;
					}
					default:
					{
						continue;
					}
				}

				if (!string.IsNullOrEmpty(givenName) && !string.IsNullOrEmpty(surName))
				{
					break;
				}
			}

			return string.Concat(givenName, " ", surName);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && Entry != null)
			{
				Entry.Dispose();
			}
		}

		protected virtual IRelationshipManager GetRelationshipManager()
		{
			return new RelationshipManager(this);
		}

		protected void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null && !string.IsNullOrEmpty(propertyName))
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public DateTime ParseDateTime(string attribute)
		{
			if (Entry.Properties.Contains(attribute))
			{
				object value = Entry.Properties[attribute].Value;

				if (value != null && value is long)
				{
					return DateTime.FromFileTime((long)value);
				}
			}

			return DateTime.MinValue;
		}

		#endregion
	}
}