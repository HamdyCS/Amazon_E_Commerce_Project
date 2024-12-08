using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductCategoryandProductCategoryImagetables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Users_UserId",
                table: "ProductCategories");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ProductCategories",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_UserId",
                table: "ProductCategories",
                newName: "IX_ProductCategories_CreatedBy");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "ProductCategoryImages",
                type: "varbinary(max)",
                nullable: false
               );

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

            migrationBuilder.AlterColumn<string>(
                name: "Description_En",
                table: "ProductCategories",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Description_Ar",
                table: "ProductCategories",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDeletion",
                table: "ProductCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Users_CreatedBy",
                table: "ProductCategories",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Users_CreatedBy",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "DateOfDeletion",
                table: "ProductCategoryImages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductCategoryImages");

            migrationBuilder.DropColumn(
                name: "DateOfDeletion",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductCategories");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ProductCategories",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_CreatedBy",
                table: "ProductCategories",
                newName: "IX_ProductCategories_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "ProductCategoryImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description_En",
                table: "ProductCategories",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description_Ar",
                table: "ProductCategories",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Users_UserId",
                table: "ProductCategories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
