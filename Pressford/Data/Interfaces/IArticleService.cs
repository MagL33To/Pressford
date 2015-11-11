using System.Collections.Generic;
using Pressford.Data.Dtos;
using Pressford.Models;

namespace Pressford.Data.Interfaces
{
    public interface IArticleService
    {
        Article GetArticleById(int id);
        IEnumerable<Article> GetAllArticles();
        IEnumerable<Article> GetArticlesByLikes(int take);

        Article AddArticle(ArticleAddModel addModel, string email);
        bool ModifyArticle(ArticleAddModel model, string email);
        bool DeleteArticle(string email, int articleId);

        bool ToggleLike(int maxDailyUserLikes, int articleId, string email);
        Comment AddComment(int articleId, string text, string email);
    }
}