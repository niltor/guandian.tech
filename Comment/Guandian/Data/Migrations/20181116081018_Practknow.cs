using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Guandian.Data.Migrations
{
    public partial class Practknow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Comments",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Comments",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PractknowId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Practknow",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    AuthorName = table.Column<string>(maxLength: 120, nullable: true),
                    AuthorId = table.Column<string>(nullable: true),
                    ViewNunmber = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Keywords = table.Column<string>(maxLength: 200, nullable: true),
                    Summary = table.Column<string>(maxLength: 1000, nullable: true),
                    FileNodeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practknow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Practknow_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Practknow_FileNode_FileNodeId",
                        column: x => x.FileNodeId,
                        principalTable: "FileNode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PractknowId",
                table: "Comments",
                column: "PractknowId");

            migrationBuilder.CreateIndex(
                name: "IX_Practknow_AuthorId",
                table: "Practknow",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Practknow_FileNodeId",
                table: "Practknow",
                column: "FileNodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Practknow_PractknowId",
                table: "Comments",
                column: "PractknowId",
                principalTable: "Practknow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Practknow_PractknowId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "Practknow");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PractknowId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PractknowId",
                table: "Comments");
        }
    }
}
