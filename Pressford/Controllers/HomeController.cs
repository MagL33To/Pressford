using System;
using System.Web.Mvc;
using Pressford.Code.Interfaces;
using Pressford.Data.Interfaces;
using Pressford.Models;

namespace Pressford.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public HomeController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }
        
        [Authorize]
        public RedirectToRouteResult Index()
        {
            return RedirectToAction("Index", "Article");
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Article");

            return View(new LoginModel());
        }

        public RedirectToRouteResult SignIn(LoginModel model)
        {
            if (!ModelState.IsValid || (String.IsNullOrEmpty(model.Email) || String.IsNullOrEmpty(model.Password)))
                return RedirectToAction("Login");

            var user = _userService.LoginUser(model.Email, model.Password);

            if (user == null)
                return RedirectToAction("Login");

            _authenticationService.Login(model.Email);

            return RedirectToAction("Index", "Article");
        }
    }
}