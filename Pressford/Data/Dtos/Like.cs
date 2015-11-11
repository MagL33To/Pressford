using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pressford.Data.Dtos
{
    public partial class Like
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ArticleId { get; set; }

        public DateTime Date { get; set; }

        public virtual Article Article { get; set; }

        public virtual User User { get; set; }
    }
}
