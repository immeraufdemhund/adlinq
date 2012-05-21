using System.DirectoryServices.Linq.Attributes;
using System.DirectoryServices.Linq.EntryObjects;

namespace System.DirectoryServices.Linq.Tests.Mocks
{
	public class DirectoryContextMock : DirectoryContext
	{
		public DirectoryContextMock() : base(string.Empty)
		{

		}

		private IEntrySet<User> _users;

		public IEntrySet<User> Groups
		{
			get
			{
				if (_users == null)
				{
					_users = CreateEntrySet<User>();
				}

				return _users;
			}
		}

		public IEntrySet<User> Users
		{
			get
			{
				if (_users == null)
				{
					_users = CreateEntrySet<User>();
				}

				return _users;
			}
		}
	}

	[DirectoryType("User", "OU=ExternalUsers")]
	public class User : UserEntryObject
	{
		private Guid _id;
		private string _email;
		private string _userName;
		private string _firstName;
		private string _lastName;
		//private int _userAccountControl;

		[DirectoryProperty("objectguid", true)]
		public Guid Id
		{
			get
			{
				return _id;
			}
			set
			{
				if (_id != value)
				{
					_id = value;
					NotifyPropertyChanged("Id");
				}
			}
		}

		[DirectoryProperty("samaccountname")]
		public string UserName
		{
			get
			{
				return _userName;
			}
			set
			{
				if (_userName != value)
				{
					_userName = value;
					NotifyPropertyChanged("UserName");
				}
			}
		}

		[DirectoryProperty("givenName")]
		public string FirstName
		{
			get
			{
				return _firstName;
			}
			set
			{
				if (_firstName != value)
				{
					_firstName = value;
					NotifyPropertyChanged("FirstName");
				}
			}
		}

		[DirectoryProperty("sn")]
		public string LastName
		{
			get
			{
				return _lastName;
			}
			set
			{
				if (_lastName != value)
				{
					_lastName = value;
					NotifyPropertyChanged("LastName");
				}
			}
		}

		[DirectoryProperty("mail")]
		public string Email
		{
			get
			{
				return _email;
			}
			set
			{
				if (_email != value)
				{
					_email = value;
					NotifyPropertyChanged("Email");
				}
			}
		}

		//[DirectoryProperty("userAccountControl")]
		//public int UserAccountControl
		//{
		//    get
		//    {
		//        return _userAccountControl;
		//    }
		//    set
		//    {
		//        if (_userAccountControl != value)
		//        {
		//            _userAccountControl = value;
		//            NotifyPropertyChanged("UserAccountControl");
		//        }
		//    }
		//}

		[EntryCollectionProperty("member", IsReferenceCollection = true)]
		public EntryCollection<Group> Groups
		{
		    get
		    {
		        return ((IEntryWithRelationships)this).RelationshipManager.GetEntryCollection<Group>("Groups");
		    }
		}
	}

	[DirectoryType("group")]
	public class Group : EntryObject
	{
		[DirectoryProperty("samaccountname")]
		public string Name { get; set; }
	}
}