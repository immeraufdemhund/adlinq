﻿using System.Collections.Generic;
using System.DirectoryServices.Linq.Attributes;
using System.DirectoryServices.Linq.EntryObjects;

namespace System.DirectoryServices.Linq.Tests.Mocks
{
	public class DirectoryContextMock : DirectoryContext
	{
		public DirectoryContextMock() : base()
		{
		}

		public DirectoryContextMock(string connectionString, string username, string password) : base(connectionString, username, password)
		{
		}

		public IEntrySet<User> Users { get; set; }
		public IEntrySet<Group> Groups { get; set; }
		public IEntrySet<OU> OrganizationUnits { get; set; }
	}

	[DirectoryType("User", "OU=ExternalUsers")]
	public class User : UserEntryObject
	{
		private Guid _id;
		private string _employeeNumber;
		private string _email;
		private string _userName;
		private string _firstName;
		private string _lastName;

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

		[DirectoryProperty("EmployeeNumber", true)]
		public string EmployeeNumber
		{
			get
			{
				return _employeeNumber;
			}
			set
			{
				_employeeNumber = value;
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

		[EntryCollectionProperty("member", MatchingRule = MatchingRuleType.InChain)]
		public IEnumerable<Group> Groups
		{
			get;
			set;
		}

		[DirectoryProperty( "memberOf", true )]
		public IEnumerable<string> DirectGroupNames { get; set; }
	}

	[DirectoryType("group", "OU=ExternalUsers")]
	public class Group : GroupEntryObject
	{
		private string _name;

		[DirectoryProperty("samaccountname")]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (_name != value)
				{
					_name = value;
					NotifyPropertyChanged("Name");
				}
			}
		}

		[EntryCollectionProperty("memberOf", MatchingRule = MatchingRuleType.InChain)]
		public EntryCollection<User> Users
		{
			get
			{
				return GetEntryCollection<User>("Users");
			}
		}
	}

	[DirectoryType("organizationalunit")]
	public class OU : EntryObject
	{
		[DirectoryProperty("name", true)]
		public string Name { get; set; }

		[EntryCollectionProperty(MatchingRule = MatchingRuleType.Children, Scope = SearchScope.OneLevel)]
		public EntrySetCollection<OU> Ous
		{
			get
			{
				return GetEntrySetCollection<OU>("Ous");
			}
		}

		[EntryCollectionProperty(MatchingRule = MatchingRuleType.Children)]
		public EntrySetCollection<User> Users
		{
			get
			{
				return GetEntrySetCollection<User>("Users");
			}
		}

		[EntryCollectionProperty(MatchingRule = MatchingRuleType.Children, Scope = SearchScope.OneLevel)]
		public EntrySetCollection<User> DirectUsers
		{
			get
			{
				return GetEntrySetCollection<User>("DirectUsers");
			}
		}

		[EntryCollectionProperty(MatchingRule = MatchingRuleType.Children)]
		public EntrySetCollection<Group> Groups
		{
			get
			{
				return GetEntrySetCollection<Group>("Groups");
			}
		}
	}
}