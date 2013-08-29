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
			// Arrange
			string firstNameStart = "st";

			using (var context = new DirectoryContextMock())
			{
                var users = context.Users.Where(u => u.FirstName.StartsWith(firstNameStart)).Skip(10).Take(10);
                var count = users.Count();

				Assert.IsNotNull(context.ConnectionString);
			}
		}

        [TestMethod]
        public void GetCountWithQueryTest()
        {
			// Arrange
			string firstNameStart = "st";

            using (var context = new DirectoryContextMock())
            {
                var userCount1 = context.Users.Where(u => u.FirstName.StartsWith(firstNameStart)).Select(u => new { u.FirstName }).Count();
                var userCount2 = context.Users.Count(u => u.FirstName.StartsWith(firstNameStart));

				Assert.AreEqual(userCount1, userCount2);
            }
        }

		[TestMethod]
		public void GetQueryableTypeTest()
		{
			// Arrange
			string userName = "sbaker";
			string groupName = "gbl-biztalk_developers";

			using (var context = new DirectoryContextMock())
			{
				var user = context.Users.First(u => u.UserName == userName);
				var userGroups = user.Groups.ToArray();
				var group = context.Groups.First(u => u.Name == groupName);
				var groupUsers = group.Users.ToArray();

				Assert.IsTrue(userGroups.Length > 0);
				Assert.IsTrue(groupUsers.Length > 0);
			}
		}

		[TestMethod]
		public void WhereUserFirstNameIsStephenTest()
		{
			// Arrange
			string firstName = "Stephen";
			string lastNameTest = "Test";
			string lastName = "Baker";

			using (var context = new DirectoryContextMock())
			{
				var users = context.Users.Where(u => u.FirstName == firstName && (u.LastName == lastNameTest || u.LastName == lastName))
					.OrderBy(u => u.LastName)
					.Select(u => new { Name = string.Concat(u.FirstName, " ", u.LastName) })
					.ToList();

				Assert.IsTrue(users.Count >= 1);
				Assert.IsTrue(users.All(u => u.Name.StartsWith(firstName)));
			}
		}

		[TestMethod]
		public void NotEnumeratingTheResultsDoesntExecuteTest()
		{
			// Arrange
			string partialFirstName1 = "tephe";
			string partialFirstName2 = "teve";

			using (var context = new DirectoryContextMock())
			{
				var usersNamedStephenQuery = context.Users.Where(u => u.FirstName.Contains(partialFirstName1) || u.FirstName.Contains(partialFirstName2));
			}

			// TODO: test this somehow
		}

		[TestMethod]
		public void FirstUserByEmailTest()
		{
			// Arrange
			string email = "stephen.baker@homeserver.local";
			string firstName = "Stephen";

			using (var context = new DirectoryContextMock())
			{
				var single = context.Users.First(u => u.Email == email);

				Assert.IsNotNull(single);
				Assert.AreEqual(single.FirstName, firstName);
			}
		}

		[TestMethod]
		public void FirstUserByIdTest()
		{
			// Arrange
			string userName = "sbaker";

			using (var context = new DirectoryContextMock())
			{
				var single = (from u in context.Users
							  where u.UserName == userName
							  select u).Single();

				Assert.IsNotNull(single);
				Assert.AreEqual(single.UserName.ToLower(), userName);
			}
		}

		[TestMethod]
		public void SingleUserByFirstNameAndLastNameFailsTest()
		{
			// Arrange
			string firstName = "Stephen";

			using (var context = new DirectoryContextMock())
			{
				try
				{
					var single = (from u in context.Users
								  where u.FirstName == firstName
								  select u).Single();
				}
				catch
				{
					// Passed Test.
					return;
				}

				// TODO: only makes sense if we already know the query will return more than one result
				Assert.Fail("Returned more then one result and didn't throw an exception.");
			}
		}

		[TestMethod]
		public void WhereUserFirstNameTest()
		{
			// Arrange
			string userName = "Stephen";

			using (var context = new DirectoryContextMock())
			{
				var all = context.Users.Where(u => userName == u.FirstName);

				Assert.IsNotNull(all);
				Assert.IsTrue(all.Count() > 0);
			}
		}

		[TestMethod]
		public void WhereGetUsersByAnonymousObjectTest()
		{
			// Arrange
			string commonName = "Stephen Baker";

			using (var context = new DirectoryContextMock())
			{
				var test = new { Cn = string.Empty };
				var all = context.Users.Where(u => test.Cn == commonName).ToArray();

				Assert.IsNotNull(all);
				Assert.IsTrue(all.Length > 0);
			}
		}

		[TestMethod]
		public void WhereLastUserFirstNameTest()
		{
			// Arrange
			string firstName = "Stephen";

			using (var context = new DirectoryContextMock())
			{
				var user = context.Users.OrderBy(u => u.LastName).Last(u => u.FirstName == firstName);

				Assert.IsNotNull(user);
			}
		}

		[TestMethod]
		public void LastOrDefaultUserFirstNameTest()
		{
			// Arrange
			string firstName = "asdf";

			using (var context = new DirectoryContextMock())
			{
				var user = context.Users.LastOrDefault(u => u.FirstName == firstName);

				Assert.IsNull(user);
			}
		}

		[TestMethod]
		public void LastUserFirstNameIsNotEmptyTest()
		{
			// Arrange
			string firstName = "Stephen";
			string email = null;

			using (var context = new DirectoryContextMock())
			{
				var user = context.Users.LastOrDefault(u => u.FirstName == firstName && u.Email == email);

				Assert.IsNotNull(user);
			}
		}

		[TestMethod]
		public void WhereUserFirstNameSkip1Take10Test()
		{
			string firstNameStart = "St";

			using (var context = new DirectoryContextMock())
			{
				var all = context.Users.Where(u => u.FirstName.StartsWith(firstNameStart))
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
			string firstNameFragment1 = "tephe";
			string firstNameFragment2 = "teve";

			using (var context = new DirectoryContextMock())
			{
				// Takes a while...figure out why..
				var usersFirstNameMethodQuery = context.Users.Where(u => u.FirstName.Contains(firstNameFragment1) || u.FirstName.Contains(firstNameFragment2));
				Assert.IsTrue(usersFirstNameMethodQuery.ToArray().Length > 0);
			}
		}

		[TestMethod]
		public void WhereFirstNameStartsWithAndEndsWithTest()
		{
			string firstNameStart = "Ste";
			string firstNameEnd = "en";

			using (var context = new DirectoryContextMock())
			{
				// Takes a while...figure out why..
				var usersFirstNameMethodQuery = context.Users.Where(u => u.FirstName.StartsWith(firstNameStart) || u.FirstName.EndsWith(firstNameEnd)).ToList();
				Assert.IsTrue(usersFirstNameMethodQuery.Count > 0);
			}
		}

		[TestMethod]
		public void EntrySetCollectionQueryProviderInheritedTest()
		{
			// Test that the query provider for a sub-query matches that of the parent.

			// Arrange
			string ouName = "ExternalUsers";
			Type queryableType;
			Type whereType;

			// Act
			using (var context = new DirectoryContextMock())
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
			string ouName = "ExternalUsers";
			int expected = 0; // Change this to your expected number!
			int result;

			// Act
			using (var context = new DirectoryContextMock())
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
