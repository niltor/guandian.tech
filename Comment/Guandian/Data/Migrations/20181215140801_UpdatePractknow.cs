using Microsoft.EntityFrameworkCore.Migrations;

namespace Guandian.Data.Migrations
{
    public partial class UpdatePractknow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Practknow_AspNetUsers_AuthorId",
                table: "Practknow");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Practknow",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Practknow_AuthorId",
                table: "Practknow",
                newName: "IX_Practknow_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Practknow_AspNetUsers_UserId",
                table: "Practknow",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Practknow_AspNetUsers_UserId",
                table: "Practknow");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Practknow",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Practknow_UserId",
                table: "Practknow",
                newName: "IX_Practknow_AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Practknow_AspNetUsers_AuthorId",
                table: "Practknow",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
