using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateApplicationOrdersandPaymentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "ApplicationOrders");

            migrationBuilder.AddColumn<long>(
                name: "ShoppingCartId",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "shippingCostId",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ApplicationOrders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_shippingCostId",
                table: "Payments",
                column: "shippingCostId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ShoppingCartId",
                table: "Payments",
                column: "ShoppingCartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationOrders_CreatedBy",
                table: "ApplicationOrders",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationOrders_Users_CreatedBy",
                table: "ApplicationOrders",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_ShippingCosts_shippingCostId",
                table: "Payments",
                column: "shippingCostId",
                principalTable: "ShippingCosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_ShoppingCarts_ShoppingCartId",
                table: "Payments",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationOrders_Users_CreatedBy",
                table: "ApplicationOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_ShippingCosts_shippingCostId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_ShoppingCarts_ShoppingCartId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_shippingCostId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ShoppingCartId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationOrders_CreatedBy",
                table: "ApplicationOrders");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "shippingCostId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ApplicationOrders");

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                table: "ApplicationOrders",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
