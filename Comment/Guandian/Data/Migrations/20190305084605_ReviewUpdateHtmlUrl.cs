using Microsoft.EntityFrameworkCore.Migrations;

namespace Guandian.Data.Migrations
{
    public partial class ReviewUpdateHtmlUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Reviews",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HtmlUrl",
                table: "Reviews",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HtmlUrl",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Reviews",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);
        }
    }
}
