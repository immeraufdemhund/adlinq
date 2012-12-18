using System.DirectoryServices.Linq.Tests.Mocks;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
		public void GetCountTest()
		{
			using (var context = new DirectoryContextMock())
			{
                var users = context.Users.Where(u => u.FirstName.StartsWith("st"));
                var arrayUsers = users.ToArray();
                Assert.AreEqual(arrayUsers.Length, users.Count());
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

				Assert.IsTrue(users.Count == 1);
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
				Assert.IsNotNull(single);
				Assert.AreEqual(single.FirstName, "Stephen");
			}
		}

        [TestMethod]
        public void GetUserByIdTest()
        {
            using (var context = new DirectoryContextMock())
            {
                var userId = new Guid("678f0f9d-e757-4c7d-af12-b3d33a5742bc");
                var single = context.Users.First(u => u.Id == userId);
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
								  where u.FirstName == "Stephen" //&& u.LastName == "Baker"
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
                var all = context.Users.Where(u => u.FirstName.StartsWith("St"))
					.Skip(10)
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
            //using (var context = new DirectoryContextMock())
            //{
            //    var single = new User
            //    {
            //        UserName = "sbaker",
            //        FirstName = "Steve",
            //        LastName = "Baker",
            //        Email = "sbaker@test.com"
            //    };
            //    context.AddObject(single);
            //    single.SetPassword("1234!@#$");
            //    context.SubmitChanges();

            //    var single1 = context.Users.Single(u => u.UserName == "sbaker");
            //    context.DeleteObject(single1);
            //    context.SubmitChanges();
            //    var single2 = context.Users.SingleOrDefault(u => u.UserName == "sbaker");
            //    Assert.IsNull(single2);
            //}
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
