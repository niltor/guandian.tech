using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Comment.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Articles",
                newName: "Content");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Comments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Articles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    AuthorName = table.Column<string>(maxLength: 100, nullable: true),
                    Content = table.Column<string>(maxLength: 4000, nullable: true),
                    Description = table.Column<string>(maxLength: 400, nullable: true),
                    Url = table.Column<string>(maxLength: 200, nullable: true),
                    ThumbnailUrl = table.Column<string>(maxLength: 200, nullable: true),
                    Tags = table.Column<string>(maxLength: 100, nullable: true),
                    Provider = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Articles",
                newName: "Description");
        }
    }
}
