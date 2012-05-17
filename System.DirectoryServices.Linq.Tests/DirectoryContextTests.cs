using System.DirectoryServices.Linq.Tests.Mocks;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.DirectoryServices.Linq.EntryObjects;

namespace System.DirectoryServices.Linq.Tests
{
	/// <summary>
	/// Summary description for DirectoryContextTests
	/// </summary>
	[TestClass]
	public class DirectoryContextTests
	{
		public DirectoryContextTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		[TestMethod]
		public void DefaultConstructorTest()
		{
			using (var context = new DirectoryContextMock())
			{
				Assert.IsNotNull(context.ConnectionString);
			}
		}

		[TestMethod]
		public void GetQueryableTypeTest()
		{
			using (var context = new DirectoryContextMock())
			{
				//Expression<Func<User, bool>> users = u => true;
				//Expression.Call();
				//var queryable = context.QueryProvider.CreateQuery(users);
				//Assert.AreEqual(queryable.ElementType, typeof(User));
			}
		}

		[TestMethod]
		public void WhereUserFirstNameIsStephenTest()
		{
			using (var context = new DirectoryContextMock())
			{
				var users = context.Users.Where(u => u.FirstName == "Stephen" && (u.LastName == "Test" || u.LastName == "Baker"))
					.OrderBy(u => u.LastName)
					.Select(u => new { Name = string.Concat(u.FirstName, " ", u.LastName) })
					.ToList();

				Assert.IsTrue(users.Count == 4);
				Assert.IsTrue(users.All(u => u.Name.StartsWith("Stephen")));
			}
		}

		[TestMethod]
		public void NotEnumeratingTheResultsDoesntExecuteTest()
		{
			using (var context = new DirectoryContextMock())
			{
				var usersNamedStephenQuery = context.Users.Where(u => u.FirstName.Contains("tephe") || u.FirstName.Contains("teve"));
			}
		}

		[TestMethod]
		public void FirstUserByEmailTest()
		{
			using (var context = new DirectoryContextMock())
			{
				var single = context.Users.First(u => u.UserName == "sbaker");
				var groups = single.Groups.First(g => g.Name.StartsWith("TEST"));
				Assert.IsNotNull(single);
				Assert.IsNotNull(groups);
				Assert.AreEqual(single.FirstName, "Stephen");
				Assert.IsNotNull(groups.Name.StartsWith("TEST"));
			}
		}

		[TestMethod]
		public void FirstUserByIdTest()
		{
			using (var context = new DirectoryContextMock())
			{
				var single = (from u in context.Users
							  where u.UserName == "sbaker"
							  select u).Single();

				Assert.IsNotNull(single);
				Assert.AreEqual(single.UserName, "sbaker");
			}
		}

		[TestMethod]
		public void SingleUserByFirstNameAndLastNameFailsTest()
		{
			using (var context = new DirectoryContextMock())
			{
				try
				{
					var single = (from u in context.Users
								  where u.FirstName == "Stephen" && u.LastName == "Baker"
								  select u).Single();
				}
				catch
				{
					// Passed Test.
					return;
				}

				Assert.Fail("Returned more then one result and didn't throw an exception.");
			}
		}

		[TestMethod]
		public void WhereUserFirstNameTest()
		{
			using (var context = new DirectoryContextMock())
			{
				var all = context.Users.Where(u => u.FirstName == "Stephen").ToArray();

				Assert.IsNotNull(all);
				Assert.IsTrue(all.Length > 0);
			}
		}

		[TestMethod]
		public void WhereLastUserFirstNameTest()
		{
			using (var context = new DirectoryContextMock())
			{
				var user = context.Users.OrderBy(u => u.LastName).Last(u => u.FirstName == "Stephen");

				Assert.IsNotNull(user);
			}
		}

		[TestMethod]
		public void LastOrDefaultUserFirstNameTest()
		{
			using (var context = new DirectoryContextMock())
			{
				var user = context.Users.LastOrDefault(u => u.FirstName == "asdf");

				Assert.IsNull(user);
			}
		}

		[TestMethod]
		public void LastUserFirstNameIsNotEmptyTest()
		{
			using (var context = new DirectoryContextMock())
			{
				var user = context.Users.LastOrDefault(u => u.FirstName == "Stephen" && u.Email == null);

				Assert.IsNotNull(user);
			}
		}

		[TestMethod]
		public void WhereUserFirstNameSkip1Take10Test()
		{
			using (var context = new DirectoryContextMock())
			{
				var test = context.Users.Where(u => u.FirstName == "Stephen").OrderBy(u => u.LastName).ToArray();

				var all = context.Users.Where(u => u.FirstName == "Stephen")
					.Skip(100)
					.Take(10)
					.OrderBy(u => u.LastName)
					.ToArray();

				Assert.IsNotNull(all);
				Assert.IsTrue(all.Length == 10);
			}
		}

		[TestMethod]
		public void AddAndDeleteNewUserSubmitChangesTest()
		{
			using (var context = new DirectoryContextMock())
			{
				//var single1 = context.Users.Single(u => u.UserName == "jsmith");
				var single = new User();
				single.UserName = "ssmith";
				single.FirstName = "Steve";
				single.LastName = "Baker";
				single.Email = "sbaker@test.com";
				context.AddObject(single);
				single.SetPassword("Wh@7Wh@7");
				context.SubmitChanges();

				var single1 = context.Users.Single(u => u.UserName == "ssmith");
				context.DeleteObject(single1);
				context.SubmitChanges();
			}
		}

		[TestMethod]
		public void WhereFirstNameContainsTest()
		{
			using (var context = new DirectoryContextMock())
			{
				// Takes a while...fugure out why..
				var usersFirstNameMethodQuery = context.Users.Where(u => u.FirstName.Contains("tephe") || u.FirstName.Contains("teve"));
				Assert.IsTrue(usersFirstNameMethodQuery.ToArray().Length > 0);
			}
		}

		[TestMethod]
		public void WhereFirstNameStartsWithAndEndsWithTest()
		{
			using (var context = new DirectoryContextMock())
			{
				// Takes a while...fugure out why..
				var usersFirstNameMethodQuery = context.Users.Where(u => u.FirstName.StartsWith("Ste") || u.FirstName.EndsWith("en"));
				Assert.IsTrue(usersFirstNameMethodQuery.ToArray().Length > 0);
			}
		}
	}
}
