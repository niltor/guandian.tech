using Guandian.Data.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Guandian.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Entity.Comment> Comments { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FileNode>()
                .HasOne(f => f.ParentNode)
                .WithMany(p => p.ChildrenNodes);

            base.OnModelCreating(builder);
        }

        public DbSet<Guandian.Data.Entity.Practknow> Practknow { get; set; }
    }
}
