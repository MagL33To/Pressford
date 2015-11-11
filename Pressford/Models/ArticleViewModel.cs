using System;
using System.Collections.Generic;

namespace Pressford.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DatePublished { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Author { get; set; }
        public int Likes { get; set; }
        public IList<CommentViewModel> Comments { get; set; }
        public bool IsUserAdmin { get; set; }
        public bool IsLikedByUser { get; set; }
    }
}