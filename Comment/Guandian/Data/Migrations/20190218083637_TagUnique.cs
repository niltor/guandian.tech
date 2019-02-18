using Microsoft.EntityFrameworkCore.Migrations;

namespace Guandian.Data.Migrations
{
    public partial class TagUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Repositories_Tag",
                table: "Repositories");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_Tag",
                table: "Repositories",
                column: "Tag");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Repositories_Tag",
                table: "Repositories");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_Tag",
                table: "Repositories",
                column: "Tag",
                unique: true,
                filter: "[Tag] IS NOT NULL");
        }
    }
}
