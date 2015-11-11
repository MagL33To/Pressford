using System.ComponentModel.DataAnnotations;

namespace Pressford.Models
{
    public class ArticleAddModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
    }
}