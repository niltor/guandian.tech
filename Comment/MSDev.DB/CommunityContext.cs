using Microsoft.EntityFrameworkCore;
using MSDev.DB.Community;
using MSDev.DB.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSDev.DB
{
    public class CommunityContext : DbContext
    {
        #region DBSet

        public DbSet<Community.Blog> Blog { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<LikeAnalysis> LikeAnalysis { get; set; }

        #endregion
        public CommunityContext(DbContextOptions<CommunityContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Community.Blog>().HasIndex(m => m.CatalogName);
            modelBuilder.Entity<Community.Blog>().HasIndex(m => m.UpdatedTime);

            modelBuilder.Entity<LikeAnalysis>().HasIndex(m => m.ObjectId);
            modelBuilder.Entity<LikeAnalysis>().HasIndex(m => m.UpdatedTime);
            modelBuilder.Entity<LikeAnalysis>().HasIndex(m => m.ObjectType);


        }
    }
}
