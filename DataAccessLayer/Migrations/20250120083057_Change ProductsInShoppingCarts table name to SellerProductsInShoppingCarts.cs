using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProductsInShoppingCartstablenametoSellerProductsInShoppingCarts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameTable("ProductsInShoppingCarts", newName: "SellerProductsInShoppingCarts");

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable("SellerProductsInShoppingCarts", newName: "ProductsInShoppingCarts");

        }
    }
}
