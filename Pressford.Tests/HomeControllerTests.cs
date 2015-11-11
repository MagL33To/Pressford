using Moq;
using NUnit.Framework;
using Pressford.Controllers;
using Pressford.Data.Dtos;
using System.Web.Mvc;
using System.Web.Routing;
using Pressford.Code.Interfaces;
using Pressford.Data.Interfaces;
using Pressford.Models;

namespace Pressford.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<IUserService> _fakeUserService;
        private Mock<IAuthenticationService> _fakeAuthService;
        private HomeController _homeController;

        [SetUp]
        public void Setup()
        {
            _fakeUserService = new Mock<IUserService>();
            _fakeAuthService = new Mock<IAuthenticationService>();
            _homeController = new HomeController(_fakeUserService.Object, _fakeAuthService.Object);
        }

        [Test]
        public void Index_NoParams_RedirectsToArticleControllerIndexAction()
        {
            var result = _homeController.Index();

            var rvd = new RouteValueDictionary
            {
                {"action", "Index" },
                {"controller", "Article"}
            };

            Assert.AreEqual(rvd, result.RouteValues);
        }

        [Test]
        public void Login_NoParamsUserIsAuthenticated_RedirectToArticleControllerIndexAction()
        {
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(x => x.HttpContext.User.Identity.IsAuthenticated).Returns(true);
            _homeController.ControllerContext = controllerContext.Object;

            var result = _homeController.Login() as RedirectToRouteResult;

            var rvd = new RouteValueDictionary
            {
                {"action", "Index" },
                {"controller", "Article"}
            };

            Assert.AreEqual(rvd, result.RouteValues);
        }

        [Test]
        public void Login_NoParamsUserIsNotAuthenticated_ReturnLoginModel()
        {
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(x => x.HttpContext.User.Identity.IsAuthenticated).Returns(false);
            _homeController.ControllerContext = controllerContext.Object;

            var result = _homeController.Login() as ViewResult;

            Assert.IsInstanceOf<LoginModel>(result.Model);
        }

        [Test]
        public void SignIn_InvalidModel_RedirectToLoginAction()
        {
            var model = new LoginModel();

            var result = _homeController.SignIn(model);

            Assert.AreEqual("Login", result.RouteValues["action"]);
        }

        [Test]
        public void SignIn_ValidModelButUserDoesNotExist_RedirectToLoginPage()
        {
            var model = new LoginModel {Email = "testuser@test.com", Password = "pass"};

            var result = _homeController.SignIn(model);

            Assert.AreEqual("Login", result.RouteValues["action"]);
        }

        [Test]
        public void SignIn_ValidModelAndUserExists_RedirectToIndexPage()
        {
            _fakeUserService.Setup(x => x.LoginUser(It.IsAny<string>(), It.IsAny<string>())).Returns(new User());

            var model = new LoginModel {Email = "test@test.com", Password = "testpass"};

            var result = _homeController.SignIn(model);

            var rvd = new RouteValueDictionary
            {
                {"action", "Index" },
                {"controller", "Article"}
            };

            Assert.AreEqual(rvd, result.RouteValues);
        }
    }
}