using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MSDev.DB.Entities;

namespace MSDev.DB
{
    public partial class MSDevContext : IdentityDbContext<User>
    {

        public MSDevContext(DbContextOptions<MSDevContext> options) : base(options)
        {

        }

        #region DbSet
        public DbSet<UserServices> UserServices { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Commodity> Commodity { get; set; }
        public DbSet<C9Event> C9Event { get; set; }
        public DbSet<EventVideo> EventVideo { get; set; }
        public DbSet<PracticeAnswer> PracticeAnswer { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<UserActivity> UserActivity { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<UserPractice> UserPractice { get; set; }
        public DbSet<Practice> Practice { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Config> Config { get; set; }
        public virtual DbSet<BingNews> BingNews { get; set; }
        public virtual DbSet<C9Articles> C9Articles { get; set; }
        public virtual DbSet<C9Videos> C9Videos { get; set; }
        public virtual DbSet<Catalog> Catalog { get; set; }
        public virtual DbSet<DevBlogs> DevBlogs { get; set; }
        public virtual DbSet<MvaVideos> MvaVideos { get; set; }
        public virtual DbSet<Resource> Resource { get; set; }
        public virtual DbSet<RssNews> RssNews { get; set; }
        public virtual DbSet<Sources> Sources { get; set; }
        public virtual DbSet<MvaDetails> MvaDetails { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=msdev;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(e => e.Member)
                .WithOne(m => m.User)
                .HasForeignKey<Member>(m => m.UserId);
            });

            modelBuilder.Entity<UserServices>(entity =>
            {
                entity.HasOne(e => e.User)
                .WithMany(u => u.UserServices)
                .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.SerialNumber);
                entity.HasOne(e => e.Commodity);

                entity.HasOne(e => e.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<Commodity>(entity =>
            {
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.SerialNumber).IsUnique();
                entity.HasIndex(e => e.TargetId).IsUnique();
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.HasOne(e => e.Video)
                .WithOne(v => v.Blog);
                entity.HasOne(e => e.Practice)
                .WithOne(p => p.Blog);
            });

            modelBuilder.Entity<Video>(entity =>
            {
                entity.HasOne(e => e.Blog)
                .WithOne(v => v.Video);
                entity.HasOne(e => e.Practice)
                .WithOne(p => p.Video);
            });
            modelBuilder.Entity<UserActivity>(entity =>
            {
                entity.HasKey(e => new { e.AcitvityId, e.UserId });
            });

            modelBuilder.Entity<UserActivity>()
            .HasOne(e => e.Activity)
            .WithMany(p => p.UserActivity)
            .HasForeignKey(e => e.AcitvityId);

            modelBuilder.Entity<UserActivity>()
                .HasOne(e => e.User)
                .WithMany(u => u.UserActivity)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<UserPractice>(entity =>
            {
                entity.HasKey(e => new { e.PracticeId, e.UserId });
            });

            modelBuilder.Entity<UserPractice>()
            .HasOne(e => e.Practice)
            .WithMany(p => p.UserPractice)
            .HasForeignKey(e => e.PracticeId);

            modelBuilder.Entity<UserPractice>()
                .HasOne(e => e.User)
                .WithMany(u => u.UserPractice)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.HasIndex(e => e.Title);
                entity.HasIndex(e => e.Tags);
                entity.HasIndex(e => e.AuthorId);
                entity.HasIndex(e => e.UpdateTime);
            });

            modelBuilder.Entity<RssNews>(entity =>
            {
                entity.HasIndex(e => e.Categories);
                entity.HasIndex(e => e.Title).IsUnique();
                entity.HasIndex(e => e.LastUpdateTime);
            });

            modelBuilder.Entity<MvaDetails>(entity =>
            {
                entity.HasIndex(e => e.MvaId).IsUnique();
                entity.HasIndex(e => e.Title);
            });



            modelBuilder.Entity<BingNews>(entity =>
            {
                entity.HasIndex(e => e.UpdatedTime)
                    .HasName("IX_BingNews_UpdatedTime");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<C9Articles>(entity =>
            {
                entity.ToTable("C9Articles");

                entity.HasIndex(e => e.SeriesTitle)
                    .HasName("IX_C9Articles_SeriesTitle");

                entity.HasIndex(e => e.Title)
                    .HasName("IX_C9Articles_Title");

                entity.HasIndex(e => e.UpdatedTime)
                    .HasName("IX_C9Articles_UpdatedTime");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Duration).HasMaxLength(32);

                entity.Property(e => e.SeriesTitle).HasMaxLength(128);

                entity.Property(e => e.SeriesTitleUrl).HasMaxLength(256);

                entity.Property(e => e.SourceUrl).HasMaxLength(256);

                entity.Property(e => e.ThumbnailUrl).HasMaxLength(256);

                entity.Property(e => e.Title).HasMaxLength(256);
            });

            modelBuilder.Entity<C9Videos>(entity =>
            {
                entity.ToTable("C9Videos");
                entity.HasIndex(e => e.SeriesType)
                    .HasName("IX_C9Videos_SeriesType");
                entity.HasIndex(e => e.Language)
                    .HasName("IX_C9Videos_Language");

                entity.HasIndex(e => e.SeriesTitle)
                    .HasName("IX_C9Videos_SeriesTitle");

                entity.HasIndex(e => e.Title)
                    .HasName("IX_C9Videos_Title");
                entity.HasIndex(e => e.Tags)
                  .HasName("IX_C9Videos_Tags");

                entity.HasIndex(e => e.UpdatedTime)
                    .HasName("IX_C9Videos_UpdatedTime");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Author).HasMaxLength(256);

                entity.Property(e => e.Description);

                entity.Property(e => e.Duration).HasMaxLength(32);

                entity.Property(e => e.Language).HasMaxLength(32);

                entity.Property(e => e.Mp3Url).HasMaxLength(512);

                entity.Property(e => e.Mp4HigUrl).HasMaxLength(512);

                entity.Property(e => e.Mp4LowUrl).HasMaxLength(512);

                entity.Property(e => e.Mp4MidUrl).HasMaxLength(512);

                entity.Property(e => e.SeriesTitle).HasMaxLength(512);

                entity.Property(e => e.SeriesTitleUrl).HasMaxLength(512);

                entity.Property(e => e.SourceUrl).HasMaxLength(512);

                entity.Property(e => e.Tags).HasMaxLength(512);

                entity.Property(e => e.ThumbnailUrl).HasMaxLength(512);

                entity.Property(e => e.Title).HasMaxLength(512);
            });

            modelBuilder.Entity<Catalog>(entity =>
            {
                entity.HasIndex(e => e.TopCatalogId)
                    .HasName("IX_CataLog_TopCatalogId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.TopCatalog)
                    .WithMany(p => p.InverseTopCatalog)
                    .HasForeignKey(d => d.TopCatalogId);
            });

            modelBuilder.Entity<DevBlogs>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Author).HasMaxLength(64);

                entity.Property(e => e.Category).HasMaxLength(32);

                entity.Property(e => e.Link).HasMaxLength(128);

                entity.Property(e => e.SourceTitle).HasMaxLength(128);

                entity.Property(e => e.Title).HasMaxLength(128);
            });

            modelBuilder.Entity<MvaVideos>(entity =>
            {
                entity.HasIndex(e => e.LanguageCode)
                    .HasName("IX_MvaVideos_LanguageCode");
                entity.HasIndex(e => e.Title)
                    .HasName("IX_MvaVideos_Title");
                entity.HasIndex(e => e.Tags)
                    .HasName("IX_MvaVideos_Title");
                entity.HasIndex(e => e.Tags)
                   .HasName("IX_MvaVideos_Technologies");
                entity.HasIndex(e => e.UpdatedTime)
                    .HasName("IX_MvaVideos_UpdatedTime");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Author).HasMaxLength(768);

                entity.Property(e => e.AuthorCompany).HasMaxLength(384);

                entity.Property(e => e.AuthorJobTitle).HasMaxLength(1024);

                entity.Property(e => e.CourseDuration).HasMaxLength(32);

                entity.Property(e => e.CourseImage).HasMaxLength(512);

                entity.Property(e => e.CourseLevel).HasMaxLength(32);

                entity.Property(e => e.CourseNumber).HasMaxLength(128);

                entity.Property(e => e.CourseStatus).HasMaxLength(32);

                entity.Property(e => e.LanguageCode).HasMaxLength(16);

                entity.Property(e => e.SourceUrl).HasMaxLength(512);

                entity.Property(e => e.Tags).HasMaxLength(384);

                entity.Property(e => e.Technologies).HasMaxLength(384);

                entity.Property(e => e.Title).HasMaxLength(256);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasIndex(e => e.CatalogId)
                    .HasName("IX_Resource_CatelogId");

                entity.HasIndex(e => e.Type)
                    .HasName("IX_Resource_Type");
                entity.HasIndex(e => e.UpdatedTime)
                  .HasName("IX_Resource_UpdatedTime");
                entity.HasIndex(e => e.Name)
                  .HasName("IX_Resource_Name");
                entity.HasIndex(e => e.Tag)
                  .HasName("IX_Resource_Tag");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AbsolutePath).HasMaxLength(256);

                entity.Property(e => e.Description).HasMaxLength(1024);

                entity.Property(e => e.Imgurl)
                    .HasColumnName("IMGUrl")
                    .HasMaxLength(256);

                entity.Property(e => e.Language).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Path).HasMaxLength(128);

                entity.HasOne(d => d.Catalog)
                    .WithMany(p => p.Resource)
                    .HasForeignKey(d => d.CatalogId)
                    .HasConstraintName("FK_Resource_CataLog_CatelogId");
            });

            modelBuilder.Entity<Sources>(entity =>
            {
                entity.HasIndex(e => e.ResourceId)
                    .HasName("IX_Sources_ResourceId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Hash).HasMaxLength(256);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.Property(e => e.Tag).HasMaxLength(32);

                entity.Property(e => e.Type).HasMaxLength(32);

                entity.Property(e => e.Url).HasMaxLength(256);

                entity.HasOne(d => d.Resource)
                    .WithMany(p => p.Sources)
                    .HasForeignKey(d => d.ResourceId);
            });
        }
    }
}