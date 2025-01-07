using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSellerProductReviewtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "SellerProducts",
                newName: "SellerProductReviews");

            migrationBuilder.RenameIndex(
                name: "IX_SellerProducts_ProductId",
                table: "SellerProductReviews",
                newName: "IX_SellerProductReviews_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "SellerId",
                table: "SellerProductReviews",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_SellerProductReviews_SellerId",
                table: "SellerProductReviews",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerProductReviews_Users_SellerId",
                table: "SellerProductReviews",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerProductReviews_Users_SellerId",
                table: "SellerProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_SellerProductReviews_SellerId",
                table: "SellerProductReviews");

            migrationBuilder.RenameTable(
                name: "SellerProductReviews",
                newName: "SellerProducts");

            migrationBuilder.RenameIndex(
                name: "IX_SellerProductReviews_ProductId",
                table: "SellerProducts",
                newName: "IX_SellerProducts_ProductId");

            migrationBuilder.AlterColumn<long>(
                name: "SellerId",
                table: "SellerProducts",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
