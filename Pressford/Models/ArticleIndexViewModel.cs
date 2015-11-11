using System.Collections.Generic;

namespace Pressford.Models
{
    public class ArticleIndexViewModel
    {
        public IList<ArticleViewModel> Articles { get; set; }
        public IList<ArticleViewModel> MostLikedArticles { get; set; }
        public bool IsUserAdmin { get; set; }
    }
}