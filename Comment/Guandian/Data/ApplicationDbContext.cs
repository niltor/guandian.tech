using Guandian.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Guandian.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Entity.Comment> Comments { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public new DbSet<User> Users { get; set; }
        /// <summary>
        /// 践识
        /// </summary>
        public DbSet<Practknow> Practknow { get; set; }
        /// <summary>
        /// 文件节点
        /// </summary>
        public DbSet<FileNode> FileNodes { get; set; }
        /// <summary>
        /// 贡献:用户与践识关联表
        /// </summary>
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<ReviewComment> ReviewComments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Author>().HasBaseType<User>();

            builder.Entity<User>(e =>
            {
                e.HasIndex(u => u.GitId).IsUnique();
            });

            builder.Entity<FileNode>(e =>
            {
                e.HasOne(f => f.ParentNode)
                    .WithMany(p => p.ChildrenNodes);
                e.HasIndex(f => f.FileName);
            });

            builder.Entity<Repository>()
                .HasIndex(r => r.Tag);

            builder.Entity<Contribution>(e =>
            {
                e.HasOne(c => c.User).WithMany(u => u.Contributions);
                e.HasOne(c => c.Practknow).WithMany(p => p.Contributions);
            });
            base.OnModelCreating(builder);

        }
    }
}
