using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LinkSaver.Data.Migrations
{
    public partial class categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category",
                table: "Links");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UrlSlug = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Links",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Links_CategoryId",
                table: "Links",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Categories_CategoryId",
                table: "Links",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Categories_CategoryId",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Links_CategoryId",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Links");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "Links",
                nullable: true);
        }
    }
}
