using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Pressford.Data.Dtos;
using Pressford.Data.Interfaces;
using Pressford.Models;

namespace Pressford.Data
{
    public class ArticleService : IArticleService
    {
        private readonly PressfordModel _db;

        public ArticleService(PressfordModel db)
        {
            _db = db;
        }

        public Article GetArticleById(int id)
        {
            return _db.Articles.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Article> GetAllArticles()
        {
            return _db.Articles.OrderByDescending(x => x.DateUpdated);
        }

        public IEnumerable<Article> GetArticlesByLikes(int take)
        {
            return _db.Articles.OrderByDescending(x => x.Likes.Count).Take(take);
        }

        public Article AddArticle(ArticleAddModel addModel, string email)
        {
            if (!IsUserAdmin(email))
                return null;

            var userId = _db.Users.Single(x => x.Email == email).Id;

            var article = new Article
            {
                Body = addModel.Body,
                Title = addModel.Title,
                CreatedBy = userId,
                UpdatedBy = userId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var addedArticle = _db.Articles.Add(article);
            _db.SaveChanges();

            return addedArticle;
        }

        public bool ModifyArticle(ArticleAddModel model, string email)
        {
            if (!IsUserAdmin(email))
                return false;

            var article = _db.Articles.Single(x => x.Id == model.Id);
            var userId = _db.Users.Single(x => x.Email == email).Id;

            article.Title = model.Title;
            article.Body = model.Body;
            article.UpdatedBy = userId;
            article.DateUpdated = DateTime.Now;

            _db.Articles.Attach(article);
            _db.Entry(article).State = EntityState.Modified;

            _db.SaveChanges();

            return true;
        }

        public bool DeleteArticle(string email, int articleId)
        {
            if (!IsUserAdmin(email))
                return false;

            var article = _db.Articles.Single(x => x.Id == articleId);
            var comments = _db.Comments.Where(x => x.ArticleId == articleId);
            var likes = _db.Likes.Where(x => x.ArticleId == articleId);

            _db.Comments.RemoveRange(comments);
            _db.Likes.RemoveRange(likes);
            _db.Articles.Remove(article);

            _db.SaveChanges();

            return true;
        }

        public bool ToggleLike(int maxDailyUserLikes, int articleId, string email)
        {
            var userId = _db.Users.Single(x => x.Email == email).Id;

            var userLikes = _db.Likes.Where(x => x.UserId == userId).Count(x => DbFunctions.TruncateTime(x.Date) == DateTime.Today);

            if (userLikes >= maxDailyUserLikes)
                return false;

            var existingLike = _db.Likes.SingleOrDefault(x => x.UserId == userId && x.ArticleId == articleId);

            if (existingLike == null)
                _db.Likes.Add(new Like { ArticleId = articleId, UserId = userId, Date = DateTime.Now });
            else
                _db.Likes.Remove(existingLike);

            _db.SaveChanges();
            return true;
        }

        public Comment AddComment(int articleId, string text, string email)
        {
            var userId = _db.Users.Single(x => x.Email == email).Id;

            var comment = new Comment
            {
                UserId = userId,
                DateCreated = DateTime.Now,
                ArticleId = articleId,
                Text = text
            };

            _db.Comments.Add(comment);
            _db.SaveChanges();

            return comment;
        }

        private bool IsUserAdmin(string email)
        {
            return _db.Users.Single(x => x.Email == email).IsAdmin;
        }
    }
}