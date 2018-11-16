using Microsoft.EntityFrameworkCore.Migrations;

namespace Guandian.Data.Migrations
{
    public partial class AddTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileNode_FileNode_ParentNodeId",
                table: "FileNode");

            migrationBuilder.DropForeignKey(
                name: "FK_Practknow_FileNode_FileNodeId",
                table: "Practknow");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileNode",
                table: "FileNode");

            migrationBuilder.RenameTable(
                name: "FileNode",
                newName: "FileNodes");

            migrationBuilder.RenameIndex(
                name: "IX_FileNode_ParentNodeId",
                table: "FileNodes",
                newName: "IX_FileNodes_ParentNodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileNodes",
                table: "FileNodes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileNodes_FileNodes_ParentNodeId",
                table: "FileNodes",
                column: "ParentNodeId",
                principalTable: "FileNodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Practknow_FileNodes_FileNodeId",
                table: "Practknow",
                column: "FileNodeId",
                principalTable: "FileNodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileNodes_FileNodes_ParentNodeId",
                table: "FileNodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Practknow_FileNodes_FileNodeId",
                table: "Practknow");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileNodes",
                table: "FileNodes");

            migrationBuilder.RenameTable(
                name: "FileNodes",
                newName: "FileNode");

            migrationBuilder.RenameIndex(
                name: "IX_FileNodes_ParentNodeId",
                table: "FileNode",
                newName: "IX_FileNode_ParentNodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileNode",
                table: "FileNode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileNode_FileNode_ParentNodeId",
                table: "FileNode",
                column: "ParentNodeId",
                principalTable: "FileNode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Practknow_FileNode_FileNodeId",
                table: "Practknow",
                column: "FileNodeId",
                principalTable: "FileNode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
