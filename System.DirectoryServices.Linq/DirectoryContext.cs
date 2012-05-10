using System.DirectoryServices.Linq.ChangeTracking;
using System.DirectoryServices.Linq.EntryObjects;
using System.Linq;

namespace System.DirectoryServices.Linq
{
	public class DirectoryContext : DisposableObject
	{
		#region Fields

		private IChangeTracker _changeTracker;
		private IResultMapper _resultMapper;
		private IQueryTranslator _queryTranslator;
		private IQueryExecutor _queryExecutor;
		private DirectoryQueryProvider _provider;

		#endregion

		#region Constructors

		public DirectoryContext() : this(GetLdapConnectionString())
		{
		}

		public DirectoryContext(string connectionString) : this(new DirectoryEntry(connectionString))
		{
			ConnectionString = connectionString;
		}

		public DirectoryContext(string connectionString, string userName, string password) : this(new DirectoryEntry(connectionString, userName, password))
		{
			ConnectionString = connectionString;
		}

		public DirectoryContext(DirectoryEntry domainEntry)
		{
			DomainEntry = domainEntry;
			domainEntry.AuthenticationType = AuthenticationTypes.Secure;
		}

		#endregion

		#region Properties

		internal DirectoryEntry DomainEntry { get; private set; }

		public IChangeTracker ChangeTracker
		{
			get
			{
				if (_changeTracker == null)
				{
					_changeTracker = GetChangeTracker();
				}

				return _changeTracker;
			}
		}

		public IResultMapper ResultMapper
		{
			get
			{
				if (_resultMapper == null)
				{
					_resultMapper = GetResultMapper();
				}

				return _resultMapper;
			}
		}

		public IQueryExecutor QueryExecutor
		{
			get
			{
				if (_queryExecutor == null)
				{
					_queryExecutor = GetQueryExecutor();
				}

				return _queryExecutor;
			}
		}

		public IQueryProvider QueryProvider
		{
			get
			{
				if (_provider == null)
				{
					_provider = new DirectoryQueryProvider(this);
				}

				return _provider;
			}
		}

		public IQueryTranslator Translator
		{
			get
			{
				if (_queryTranslator == null)
				{
					_queryTranslator = GetQueryTranslator();
				}

				return _queryTranslator;
			}
		}

		public string ConnectionString { get; set; }

		#endregion

		#region Methods

		public static string GetLdapConnectionString()
		{
			using (var adRoot = new DirectoryEntry("LDAP://RootDSE"))
			{
				var dnc = Convert.ToString(adRoot.Properties["defaultNamingContext"][0]);
				var server = Convert.ToString(adRoot.Properties["dnsHostName"][0]);
				return string.Format("LDAP://{0}/{1}", server, dnc);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && DomainEntry != null)
			{
				DomainEntry.Dispose();
			}
		}

		protected virtual IChangeTracker GetChangeTracker()
		{
			return new ChangeTracker(this);
		}

		protected virtual IResultMapper GetResultMapper()
		{
			return new ResultMapper(this);
		}

		protected virtual IQueryTranslator GetQueryTranslator()
		{
			return new QueryTranslator(this);
		}

		protected virtual IQueryExecutor GetQueryExecutor()
		{
			return new QueryExecutor(this);
		}

		public IEntrySet<T> CreateEntrySet<T>() where T : class
		{
			return new EntrySet<T>(this);
		}

		public void SubmitChanges()
		{
			ChangeTracker.SubmitChanges();
		}

		#endregion
	}
}
