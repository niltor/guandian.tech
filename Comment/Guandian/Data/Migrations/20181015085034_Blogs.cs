using Microsoft.EntityFrameworkCore.Migrations;

namespace Guandian.Data.Migrations
{
    public partial class Blogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Articles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "Articles",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentEn",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Articles",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "Articles",
                maxLength: 120,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ContentEn",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "Articles");
        }
    }
}
