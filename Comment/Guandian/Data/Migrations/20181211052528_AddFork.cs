using Microsoft.EntityFrameworkCore.Migrations;

namespace Guandian.Data.Migrations
{
    public partial class AddFork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsForkPractknow",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForkPractknow",
                table: "AspNetUsers");
        }
    }
}
