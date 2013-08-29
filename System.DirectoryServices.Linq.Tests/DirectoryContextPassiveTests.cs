using System.DirectoryServices.Linq.Tests.Mocks;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.DirectoryServices.Linq.Tests
{
	/// <summary>
	/// Summary description for DirectoryContextTests
	/// </summary>
	[TestClass]
	public class DirectoryContextPassiveTests
	{
		[TestMethod]
		public void DefaultConstructorTest()
		{
			using (var context = new DirectoryContextMock())
			{
                var users = context.Users.Where(u => u.FirstName.StartsWith("st")).Skip(10).Take(10);
                var count = users.Count();

				Assert.IsNotNull(context.ConnectionString);
			}
		}

        [TestMethod]
        public void GetCountWithQueryTest()
        {
            using (var context = new DirectoryContextMock())
            {
                var userCount1 = context.Users.Where(u => u.FirstName.StartsWith("st")).Select(u => new { u.FirstName }).Count();
                var userCount2 = context.Users.Count(u => u.FirstName.StartsWith("st"));

				Assert.AreEqual(userCount1, userCount2);
            }
        }

		[TestMethod]
		public void GetQueryableTypeTest()
		{
			using (var context = new DirectoryContextMock())
			{
				var user = context.Users.First(u => u.UserName == "sbaker");
				var userGroups = user.Groups.ToArray();
				var group = context.Groups.First(u => u.Name == "gbl-biztalk_developers");
				var groupUsers = group.Users.ToArray();

				Assert.IsTrue(userGroups.Length > 0);
				Assert.IsTrue(groupUsers.Length > 0);
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

				Assert.IsTrue(users.Count >= 1);
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
				var single = context.Users.First(u => u.Email == "stephen.baker@homeserver.local");

				Assert.IsNotNull(single);
				Assert.AreEqual(single.FirstName, "Stephen");
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
				Assert.AreEqual(single.UserName.ToLower(), "sbaker");
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
								  where u.FirstName == "Stephen"
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
				var all = context.Users.Where(u => "Stephen" == u.FirstName);

				Assert.IsNotNull(all);
				Assert.IsTrue(all.Count() > 0);
			}
		}

		[TestMethod]
		public void WhereGetUsersByAnonymousObjectTest()
		{
			using (var context = new DirectoryContextMock())
			{
				var test = new { Cn = string.Empty };
				var all = context.Users.Where(u => test.Cn == "Stephen Baker").ToArray();

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
				var all = context.Users.Where(u => u.FirstName.StartsWith("St"))
					.Skip(90)
					.Take(10)
					.OrderBy(u => u.LastName)
					.ToArray();

				Assert.IsNotNull(all);
				Assert.IsTrue(all.Length == 10);
			}
		}

		[TestMethod]
		public void WhereFirstNameContainsTest()
		{
			using (var context = new DirectoryContextMock())
			{
				// Takes a while...figure out why..
				var usersFirstNameMethodQuery = context.Users.Where(u => u.FirstName.Contains("tephe") || u.FirstName.Contains("teve"));
				Assert.IsTrue(usersFirstNameMethodQuery.ToArray().Length > 0);
			}
		}

		[TestMethod]
		public void WhereFirstNameStartsWithAndEndsWithTest()
		{
			using (var context = new DirectoryContextMock())
			{
				// Takes a while...figure out why..
				var usersFirstNameMethodQuery = context.Users.Where(u => u.FirstName.StartsWith("Ste") || u.FirstName.EndsWith("en")).ToList();
				Assert.IsTrue(usersFirstNameMethodQuery.Count > 0);
			}
		}

		[TestMethod]
		public void EntrySetCollectionQueryProviderInheritedTest()
		{
			// Test that the query provider for a sub-query matches that of the parent.

			// Arrange
			var context = new DirectoryContextMock();
			string ouName = "ExternalUsers";
			Type queryableType;
			Type whereType;

			// Act
			using (context)
			{
				var queryable = context.OrganizationUnits.Single(u => u.Name == ouName).Users;
				var where = queryable.Where(u => true);
				queryableType = (queryable as IQueryable).Provider.GetType();
				whereType = where.Provider.GetType();
			}

			// Assert
			Assert.AreEqual(queryableType, whereType);
		}


		[TestMethod]
		public void OneLevelTest()
		{
			// Test that the OneLevel option works with subqueries.

			// Arrange
			var context = new DirectoryContextMock();
			string ouName = "ExternalUsers";
			int expected = 0; // Change this to your expected number!
			int result;

			// Act
			using (context)
			{
				var queryable = context.OrganizationUnits.Single(u => u.Name == ouName).Users;
				var x = queryable.Where(u => true);
				result = x.Count();
			}

			// Assert
			Assert.AreEqual(expected, result);
		}
	}
}
