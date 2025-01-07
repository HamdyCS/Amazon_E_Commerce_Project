using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSellerProductname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerProductReviews_Users_SellerId",
                table: "SellerProductReviews");

            migrationBuilder.RenameTable(
                name: "SellerProductReviews",
                newName: "SellerProducts");

            migrationBuilder.RenameIndex(
                name: "IX_SellerProductReviews_SellerId",
                table: "SellerProducts",
                newName: "IX_SellerProducts_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_SellerProductReviews_ProductId",
                table: "SellerProducts",
                newName: "IX_SellerProducts_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerProducts_Users_SellerId",
                table: "SellerProducts",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerProducts_Users_SellerId",
                table: "SellerProducts");

            migrationBuilder.RenameTable(
                name: "SellerProducts",
                newName: "SellerProductReviews");

            migrationBuilder.RenameIndex(
                name: "IX_SellerProducts_SellerId",
                table: "SellerProductReviews",
                newName: "IX_SellerProductReviews_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_SellerProducts_ProductId",
                table: "SellerProductReviews",
                newName: "IX_SellerProductReviews_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerProductReviews_Users_SellerId",
                table: "SellerProductReviews",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
