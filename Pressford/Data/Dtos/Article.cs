using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pressford.Data.Dtos
{
    public partial class Article
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Article()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Like> Likes { get; set; }
    }
}
