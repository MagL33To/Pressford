using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using Pressford.Code.Interfaces;
using Pressford.Controllers;
using Pressford.Data.Dtos;
using Pressford.Data.Interfaces;
using Pressford.Models;

namespace Pressford.Tests
{
    [TestFixture]
    public class ArticleControllerTests
    {
        private Mock<IArticleService> _fakeArticleService;
        private Mock<IUserService> _fakeUserService;
        private Mock<IConfigurationService> _fakeConfigService;

        private Article _testArticle;

        private ArticleController _articleController;

        [SetUp]
        public void SetUp()
        {
            _fakeUserService = new Mock<IUserService>();
            _fakeArticleService = new Mock<IArticleService>();
            _fakeConfigService = new Mock<IConfigurationService>();

            _testArticle = new Article
            {
                User = new User(),
                Id = 1,
                Likes = new List<Like>(),
                Comments = new List<Comment>(),
                Body = "Test!",
                CreatedBy = 1,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Title = "Test Article",
                UpdatedBy = 1,
                User1 = new User()
            };

            _articleController = new ArticleController(_fakeArticleService.Object, _fakeUserService.Object, _fakeConfigService.Object);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("testuser@test.com");
            _articleController.ControllerContext = controllerContext.Object;
        }

        [Test]
        public void Index_NoParams_ReturnArticleIndexViewModel()
        {
            var result = _articleController.Index();

            Assert.IsInstanceOf<ArticleIndexViewModel>(result.Model);
        }

        [Test]
        public void View_NonExistantArticleId_Returns404()
        {
            var result = _articleController.View(1) as HttpNotFoundResult;

            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void View_ExistingArticleId_ReturnsArticleViewModel()
        {
            _fakeArticleService.Setup(x => x.GetArticleById(It.IsAny<int>())).Returns(_testArticle);

            var result = _articleController.View(1) as ViewResult;

            Assert.IsInstanceOf<ArticleViewModel>(result.Model);
        }

        [Test]
        public void Add_NoParams_ReturnsArticleAddModel()
        {
            var result = _articleController.Add();

            Assert.IsInstanceOf<ArticleAddModel>(result.Model);
        }

        [Test]
        public void SubmitArticle_InvalidModel_RedirectToAdd()
        {
            var result = _articleController.SubmitArticle(new ArticleAddModel());

            Assert.AreEqual("Add", result.RouteValues["action"]);
        }

        [Test]
        public void SubmitArticle_ValidModelDBFailsToCreateArticle_RedirectToAdd()
        {
            _fakeArticleService.Setup(x => x.AddArticle(It.IsAny<ArticleAddModel>(), It.IsAny<string>())).Returns((Article)null);

            var model = new ArticleAddModel {Body = "Test", Title = "Test article"};

            var result = _articleController.SubmitArticle(model);

            Assert.AreEqual("Add", result.RouteValues["action"]);
        }

        [Test]
        public void SubmitArticle_ValidModelDBCreatesArticle_RedirectToCreatesArticle()
        {
            _fakeArticleService.Setup(x => x.AddArticle(It.IsAny<ArticleAddModel>(), It.IsAny<string>())).Returns(_testArticle);

            var model = new ArticleAddModel {Body = "Test", Title = "Test article"};

            var result = _articleController.SubmitArticle(model);

            var rvd = new RouteValueDictionary
            {
                {"id", _testArticle.Id },
                {"action", "View"}
            };

            Assert.AreEqual(rvd, result.RouteValues);
        }

        [Test]
        public void Edit_InvalidId_Returns404()
        {
            var result = _articleController.Edit(1) as HttpNotFoundResult;

            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void Edit_ValidId_ReturnsArticleAddModel()
        {
            _fakeArticleService.Setup(x => x.GetArticleById(It.IsAny<int>())).Returns(_testArticle);

            var result = _articleController.Edit(1) as ViewResult;

            Assert.IsInstanceOf<ArticleAddModel>(result.Model);
        }

        [Test]
        public void ModifyArticle_InvalidModel_RedirectsToEdit()
        {
            var model = new ArticleAddModel();

            var result = _articleController.ModifyArticle(model);

            var rvd = new RouteValueDictionary
            {
                {"id", model.Id },
                {"action", "Edit"}
            };

            Assert.AreEqual(rvd, result.RouteValues);
        }

        [Test]
        public void ModifyArticle_ValidModelDBDoesNotUpdateArticle_RedirectsToEdit()
        {
            var model = new ArticleAddModel { Title = "Test article", Id = 1, Body = "Body"};

            var result = _articleController.ModifyArticle(model);

            var rvd = new RouteValueDictionary
            {
                {"id", model.Id },
                {"action", "Edit"}
            };

            Assert.AreEqual(rvd, result.RouteValues);
        }

        [Test]
        public void ModifyArticle_ValidModelDBUpdatesArticle_RedirectsToEditedArticle()
        {
            _fakeArticleService.Setup(x => x.ModifyArticle(It.IsAny<ArticleAddModel>(), It.IsAny<string>())).Returns(true);

            var model = new ArticleAddModel { Title = "Test article", Id = 1, Body = "Body"};

            var result = _articleController.ModifyArticle(model);

            var rvd = new RouteValueDictionary
            {
                {"id", model.Id },
                {"action", "View"}
            };

            Assert.AreEqual(rvd, result.RouteValues);
        }

        [Test]
        public void DeleteArticle_DBDeletesArticle_RedirectToIndex()
        {
            _fakeArticleService.Setup(x => x.DeleteArticle(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            var result = _articleController.DeleteArticle(1);

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void DeleteArticle_DBDoesNotDeleteArticle_RedirectToEditPage()
        {
            _fakeArticleService.Setup(x => x.DeleteArticle(It.IsAny<string>(), It.IsAny<int>())).Returns(false);

            var result = _articleController.DeleteArticle(1);

            var rvd = new RouteValueDictionary
            {
                {"id", 1 },
                {"action", "Edit"}
            };

            Assert.AreEqual(rvd, result.RouteValues);
        }

        [Test]
        public void AddComment_InvalidModel_ReturnFalse()
        {
            var result = _articleController.AddComment(0, String.Empty);

            Assert.AreEqual(false, result.Data);
        }

        [Test]
        public void AddComment_ValidModel_ReturnCommentViewModel()
        {
            var comment = new Comment
            {
                ArticleId = 1,
                Text = "Good article",
                DateCreated = DateTime.Now,
                User = new User
                {
                    Forename = "Test",
                    Surname = "User"
                }
            };

            _fakeArticleService.Setup(x => x.AddComment(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(comment);

            var result = _articleController.AddComment(comment.ArticleId, comment.Text);

            Assert.IsInstanceOf<CommentViewModel>(result.Data);
        }

        [Test]
        public void ToggleLike_DBSuccessfullyTogglesLike_ReturnTrue()
        {
            _fakeArticleService.Setup(x => x.ToggleLike(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            var result = _articleController.ToggleLike(1);

            Assert.AreEqual(true, result.Data);
        }

        [Test]
        public void ToggleLike_DBDoesNotToggleLike_ReturnFalse()
        {
            _fakeArticleService.Setup(x => x.ToggleLike(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            var result = _articleController.ToggleLike(1);

            Assert.AreEqual(false, result.Data);
        }
    }
}