using System.Data.Entity;

namespace Pressford.Data.Dtos
{
    public partial class PressfordModel : DbContext
    {
        public PressfordModel()
            : base("name=PressfordModel")
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Article)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Article>()
                .HasMany(e => e.Likes)
                .WithRequired(e => e.Article)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Articles)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Articles1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.UpdatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Likes)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
