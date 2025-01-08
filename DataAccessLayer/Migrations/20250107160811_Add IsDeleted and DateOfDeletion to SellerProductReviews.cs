using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedandDateOfDeletiontoSellerProductReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Users_UserId",
                table: "ProductReviews");

            migrationBuilder.RenameTable(
                name: "ProductReviews",
                newName: "SellerProductReviews");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_UserId",
                table: "SellerProductReviews",
                newName: "IX_SellerProductReviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_SellerProductId",
                table: "SellerProductReviews",
                newName: "IX_SellerProductReviews_SellerProductId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDeletion",
                table: "SellerProductReviews",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SellerProductReviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_SellerProductReviews_Users_UserId",
                table: "SellerProductReviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerProductReviews_Users_UserId",
                table: "SellerProductReviews");

            migrationBuilder.DropColumn(
                name: "DateOfDeletion",
                table: "SellerProductReviews");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SellerProductReviews");

            migrationBuilder.RenameTable(
                name: "SellerProductReviews",
                newName: "ProductReviews");

            migrationBuilder.RenameIndex(
                name: "IX_SellerProductReviews_UserId",
                table: "ProductReviews",
                newName: "IX_ProductReviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SellerProductReviews_SellerProductId",
                table: "ProductReviews",
                newName: "IX_ProductReviews_SellerProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Users_UserId",
                table: "ProductReviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
