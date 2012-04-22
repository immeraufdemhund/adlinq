using System.ComponentModel;

namespace System.DirectoryServices.Linq.EntryObjects
{
	public abstract class EntryObject : IEntryWithRelationships, INotifyPropertyChanged
	{
		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Fields

		private const int UF_DONT_EXPIRE_PASSWD = 0x10000;

		private IRelationshipManager _relationshipManager;

		#endregion

		#region Constructors

		public EntryObject()
		{

		}

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

		public Type ElementType
		{
			get
			{
				return GetType();
			}
		}

		internal string ADPath { get; set; }

		internal DirectoryEntry Entry { get; set; }

		internal DirectoryContext Context { get; set; }

		#endregion

		#region Methods

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