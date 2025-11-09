using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WebShop.Models;

#nullable disable

namespace WebShop.Migrations
{
    public partial class AddDocumentsTasksAndUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Role column to AspNetUsers (string mapped from enum).
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "Performer");

            // Documents table
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 250, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false, defaultValue: "Draft")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Status",
                table: "Documents",
                column: "Status");

            // Tasks table
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssigneeId = table.Column<string>(nullable: true),
                    DocumentId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false, defaultValue: "Open"),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_Tasks_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssigneeId",
                table: "Tasks",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_DocumentId",
                table: "Tasks",
                column: "DocumentId");

            // Index on user Role
            migrationBuilder.CreateIndex(
                name: "IX_Users_Role",
                table: "AspNetUsers",
                column: "Role");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Users_Role",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");
        }
    }
}