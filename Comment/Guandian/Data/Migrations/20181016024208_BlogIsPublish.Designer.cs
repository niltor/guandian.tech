﻿// <auto-generated />
using System;
using Guandian.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Guandian.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181016024208_BlogIsPublish")]
    partial class BlogIsPublish
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Guandian.Data.Entity.Article", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<string>("AuthorName")
                        .HasMaxLength(60);

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Keywords")
                        .HasMaxLength(200);

                    b.Property<int>("Status");

                    b.Property<string>("Summary")
                        .HasMaxLength(500);

                    b.Property<string>("Title")
                        .HasMaxLength(100);

                    b.Property<DateTime>("UpdatedTime");

                    b.Property<int>("ViewNunmber");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Articles");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Article");
                });

            modelBuilder.Entity("Guandian.Data.Entity.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ArticleId");

                    b.Property<string>("AuthorId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<int>("Status");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Guandian.Data.Entity.News", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorName")
                        .HasMaxLength(100);

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Description")
                        .HasMaxLength(400);

                    b.Property<bool>("IsPublishToMP");

                    b.Property<string>("Provider")
                        .HasMaxLength(100);

                    b.Property<int>("Status");

                    b.Property<string>("Tags")
                        .HasMaxLength(100);

                    b.Property<string>("ThumbnailUrl")
                        .HasMaxLength(200);

                    b.Property<string>("Title")
                        .HasMaxLength(100);

                    b.Property<DateTime>("UpdatedTime");

                    b.Property<string>("Url")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("News");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Guandian.Data.Entity.Blog", b =>
                {
                    b.HasBaseType("Guandian.Data.Entity.Article");

                    b.Property<string>("Categories")
                        .HasMaxLength(300);

                    b.Property<string>("ContentEn");

                    b.Property<bool>("IsPublishMP");

                    b.Property<string>("Link")
                        .HasMaxLength(300);

                    b.Property<string>("TitleEn")
                        .HasMaxLength(200);

                    b.ToTable("Blog");

                    b.HasDiscriminator().HasValue("Blog");
                });

            modelBuilder.Entity("Guandian.Data.Entity.Author", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("IdentityCard")
                        .HasMaxLength(18);

                    b.Property<string>("NickName");

                    b.Property<string>("RealName");

                    b.ToTable("Author");

                    b.HasDiscriminator().HasValue("Author");
                });

            modelBuilder.Entity("Guandian.Data.Entity.Article", b =>
                {
                    b.HasOne("Guandian.Data.Entity.Author", "Author")
                        .WithMany("Articles")
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("Guandian.Data.Entity.Comment", b =>
                {
                    b.HasOne("Guandian.Data.Entity.Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId");

                    b.HasOne("Guandian.Data.Entity.Author", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
