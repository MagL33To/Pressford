using System;
using System.Linq;
using System.Web.Mvc;
using Pressford.Code.Interfaces;
using Pressford.Data.Dtos;
using Pressford.Data.Interfaces;
using Pressford.Models;

namespace Pressford.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IUserService _userService;
        private readonly IConfigurationService _configurationService;

        public ArticleController(IArticleService articleService, IUserService userService, IConfigurationService configurationService)
        {
            _articleService = articleService;
            _userService = userService;
            _configurationService = configurationService;
        }
        
        public ViewResult Index()
        {
            var allArticles = _articleService.GetAllArticles().Select(GetViewModel).ToList();
            var mostLikedArticles = _articleService.GetArticlesByLikes(_configurationService.GetLikedArticlesTakeAmount()).Select(GetViewModel).ToList();
            var isUserAdmin = _userService.IsUserAdmin(User.Identity.Name);
            return View(new ArticleIndexViewModel { Articles = allArticles, MostLikedArticles = mostLikedArticles, IsUserAdmin = isUserAdmin });
        }

        public ActionResult View(int id)
        {
            var article = _articleService.GetArticleById(id);

            if (article == null)
                return new HttpNotFoundResult("No such article Id");

            var model = GetViewModel(article);
            model.IsUserAdmin = _userService.IsUserAdmin(User.Identity.Name);
            return View(model);
        }

        public ViewResult Add()
        {
            return View(new ArticleAddModel());
        }

        public RedirectToRouteResult SubmitArticle(ArticleAddModel model)
        {
            if (!ModelState.IsValid || String.IsNullOrEmpty(model.Body) || String.IsNullOrEmpty(model.Title))
                return RedirectToAction("Add");

            var article = _articleService.AddArticle(model, User.Identity.Name);

            return article == null ? RedirectToAction("Add") : RedirectToAction("View", new {id = article.Id});
        }

        public ActionResult Edit(int id)
        {
            var article = _articleService.GetArticleById(id);

            if (article == null)
                return new HttpNotFoundResult("No such article Id");

            return View(new ArticleAddModel { Id = article.Id, Title = article.Title, Body = article.Body });
        }

        public RedirectToRouteResult ModifyArticle(ArticleAddModel model)
        {
            if (!ModelState.IsValid || String.IsNullOrEmpty(model.Body) || String.IsNullOrEmpty(model.Title) || model.Id < 1)
                return RedirectToAction("Edit", new { id = model.Id });

            var articleUpdated = _articleService.ModifyArticle(model, User.Identity.Name);

            return !articleUpdated ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("View", new {id = model.Id});
        }

        public RedirectToRouteResult DeleteArticle(int id)
        {
            var articleDeleted = _articleService.DeleteArticle(User.Identity.Name, id);

            return articleDeleted ? RedirectToAction("Index") : RedirectToAction("Edit", new {id});
        }

        public JsonResult AddComment(int articleId, string text)
        {
            if (articleId < 1 || String.IsNullOrEmpty(text))
                return Json(false, JsonRequestBehavior.AllowGet);

            var commentModel = GetViewModel(_articleService.AddComment(articleId, text, User.Identity.Name));

            return Json(commentModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ToggleLike(int articleId)
        {
            return Json(_articleService.ToggleLike(_configurationService.GetMaxDailyUserLikesAmount(), articleId, User.Identity.Name), JsonRequestBehavior.AllowGet);
        }

        private ArticleViewModel GetViewModel(Article articleDto)
        {
            return new ArticleViewModel
            {
                Id = articleDto.Id,
                Likes = articleDto.Likes.Count,
                Title = articleDto.Title,
                Body = articleDto.Body,
                DateUpdated = articleDto.DateUpdated,
                Comments = articleDto.Comments.Select(GetViewModel).ToList(),
                IsLikedByUser = articleDto.Likes.SingleOrDefault(x => x.User.Email == User.Identity.Name) != null,
                Author = $"{articleDto.User.Forename} {articleDto.User.Surname}{(articleDto.User1 != null && articleDto.User1.Id != articleDto.User.Id ? $" edited by: {articleDto.User1.Forename} {articleDto.User1.Surname}" : "")}",
                DatePublished = articleDto.DateCreated
            };
        }

        private static CommentViewModel GetViewModel(Comment commentDto)
        {
            return new CommentViewModel
            {
                Text = commentDto.Text,
                PostedBy = $"{commentDto.User.Forename} {commentDto.User.Surname}",
                PostedDate = commentDto.DateCreated.ToString()
            };
        }
    }
}