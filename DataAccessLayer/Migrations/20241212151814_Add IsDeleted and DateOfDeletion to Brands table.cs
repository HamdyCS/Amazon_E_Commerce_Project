using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedandDateOfDeletiontoBrandstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Users_UserId",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Brands",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Brands_UserId",
                table: "Brands",
                newName: "IX_Brands_CreatedBy");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDeletion",
                table: "Brands",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Brands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Users_CreatedBy",
                table: "Brands",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Users_CreatedBy",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DateOfDeletion",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Brands",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Brands_CreatedBy",
                table: "Brands",
                newName: "IX_Brands_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Users_UserId",
                table: "Brands",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
