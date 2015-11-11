using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using Pressford.Data;
using Pressford.Data.Dtos;

namespace Pressford.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;
        private readonly List<User> _testUsers = new List<User>
            {
                new User
                {
                    Password = "testpass",
                    Email = "test@test.com",
                    Articles = new List<Article>(),
                    Articles1 = new List<Article>(),
                    Comments = new List<Comment>(),
                    Forename = "Test",
                    Id = 1,
                    IsAdmin = true,
                    Likes = new List<Like>(),
                    Surname = "User"
                },
                new User
                {
                    Password = "testpass2",
                    Email = "testuser2@test.com",
                    Articles = new List<Article>(),
                    Articles1 = new List<Article>(),
                    Comments = new List<Comment>(),
                    Forename = "Test",
                    Id = 1,
                    IsAdmin = false,
                    Likes = new List<Like>(),
                    Surname = "User2"
                }
            };

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var mockSet = SetUpFakeUsers(_testUsers);

            var mockContext = new Mock<PressfordModel>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            _userService = new UserService(mockContext.Object);
        }

        [Test]
        public void LoginUser_UserDoesNotExist_ReturnNull()
        {
            var result = _userService.LoginUser("nope@test.com", "nope");

            Assert.AreEqual(null, result);
        }

        [Test]
        public void LoginUser_UserExists_ReturnUser()
        {
            var result = _userService.LoginUser(_testUsers[0].Email, _testUsers[0].Password);

            Assert.AreEqual(_testUsers[0], result);
        }

        [Test]
        public void IsUserAdmin_UserIsAdmin_ReturnTrue()
        {
            var result = _userService.IsUserAdmin(_testUsers[0].Email);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsUserAdmin_UserIsAdmin_ReturnFalse()
        {
            var result = _userService.IsUserAdmin(_testUsers[1].Email);

            Assert.IsFalse(result);
        }

        private static Mock<DbSet<User>> SetUpFakeUsers(IEnumerable<User> users)
        {
            var data = users.AsQueryable();

            var userMock = new Mock<DbSet<User>>();
            userMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            userMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            userMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            userMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return userMock;
        }
    }
}