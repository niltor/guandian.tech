using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Guandian.Data.Migrations
{
    public partial class FileNode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileNode",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    IsFile = table.Column<bool>(nullable: false),
                    FileName = table.Column<string>(maxLength: 200, nullable: true),
                    SHA = table.Column<string>(maxLength: 200, nullable: true),
                    GithubLink = table.Column<string>(maxLength: 500, nullable: true),
                    Path = table.Column<string>(maxLength: 200, nullable: true),
                    ParentNodeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileNode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileNode_FileNode_ParentNodeId",
                        column: x => x.ParentNodeId,
                        principalTable: "FileNode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileNode_ParentNodeId",
                table: "FileNode",
                column: "ParentNodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileNode");
        }
    }
}
