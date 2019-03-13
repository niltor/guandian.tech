using Microsoft.EntityFrameworkCore.Migrations;

namespace Guandian.Data.Migrations
{
    public partial class UpModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sha",
                table: "ReviewComments");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "ReviewComments",
                newName: "HtmlUrl");

            migrationBuilder.AddColumn<string>(
                name: "GitId",
                table: "AspNetUsers",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GitId",
                table: "AspNetUsers",
                column: "GitId",
                unique: true,
                filter: "[GitId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GitId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GitId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "HtmlUrl",
                table: "ReviewComments",
                newName: "Url");

            migrationBuilder.AddColumn<string>(
                name: "Sha",
                table: "ReviewComments",
                maxLength: 100,
                nullable: true);
        }
    }
}
