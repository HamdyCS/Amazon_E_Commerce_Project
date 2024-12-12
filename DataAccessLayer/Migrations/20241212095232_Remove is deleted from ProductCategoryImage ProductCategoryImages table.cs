using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveisdeletedfromProductCategoryImageProductCategoryImagestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfDeletion",
                table: "ProductCategoryImages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductCategoryImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDeletion",
                table: "ProductCategoryImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductCategoryImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
