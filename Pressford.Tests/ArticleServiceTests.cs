using System;
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
    public class ArticleServiceTests
    {
        #region Test Objects
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
        private readonly List<Article> _testArticles = new List<Article>
            {
                new Article
                {
                    Body = "Article 1",
                    Title = "Test article 1",
                    Id = 1,
                    DateCreated = DateTime.Now
                }
            };
        #endregion
        private ArticleService _articleService;

        [SetUp]
        public void SetUp()
        {
            var mockUserSet = SetUpFakeUsers(_testUsers);
            var mockArticleSet = SetUpFakeArticles(_testArticles);

            var mockContext = new Mock<PressfordModel>();
            mockContext.Setup(x => x.Users).Returns(mockUserSet.Object);
            mockContext.Setup(x => x.Articles).Returns(mockArticleSet.Object);

            _articleService = new ArticleService(mockContext.Object);
        }

        [Test]
        public void GetArticleById_NonExistingArticleId_ReturnNull()
        {
            var result = _articleService.GetArticleById(0);

            Assert.IsNull(result);
        }

        [Test]
        public void GetArticleById_ExistingArticleId_ReturnsArticle()
        {
            var result = _articleService.GetArticleById(1);

            Assert.IsInstanceOf<Article>(result);
        }

        [Test]
        public void GetAllArticles_NoParams_ReturnsIEnumerableOfArticles()
        {
            var result = _articleService.GetAllArticles();

            Assert.IsInstanceOf<IEnumerable<Article>>(result);
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

        private static Mock<DbSet<Article>> SetUpFakeArticles(IEnumerable<Article> articles)
        {
            var data = articles.AsQueryable();

            var articleMock = new Mock<DbSet<Article>>();
            articleMock.As<IQueryable<Article>>().Setup(m => m.Provider).Returns(data.Provider);
            articleMock.As<IQueryable<Article>>().Setup(m => m.Expression).Returns(data.Expression);
            articleMock.As<IQueryable<Article>>().Setup(m => m.ElementType).Returns(data.ElementType);
            articleMock.As<IQueryable<Article>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return articleMock;
        }
    }
}