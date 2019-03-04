using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Guandian.Data.Migrations
{
    public partial class NewReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Body = table.Column<string>(maxLength: 2000, nullable: true),
                    Url = table.Column<string>(nullable: true),
                    DiffUrl = table.Column<string>(maxLength: 300, nullable: true),
                    CommitsUrl = table.Column<string>(maxLength: 300, nullable: true),
                    Number = table.Column<int>(nullable: false),
                    MergeCommitSha = table.Column<string>(maxLength: 100, nullable: true),
                    Merged = table.Column<bool>(nullable: false),
                    MergeTime = table.Column<DateTimeOffset>(nullable: false),
                    MergeStatus = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReviewComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Url = table.Column<string>(maxLength: 300, nullable: true),
                    Sha = table.Column<string>(maxLength: 100, nullable: true),
                    Content = table.Column<string>(maxLength: 2000, nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    ReviewId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewComments_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComments_ReviewId",
                table: "ReviewComments",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComments_UserId",
                table: "ReviewComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewComments");

            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
