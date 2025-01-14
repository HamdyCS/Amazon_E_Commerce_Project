using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentsandApplicationOrderstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationOrders_UsersAddresses_UserAddressId",
                table: "ApplicationOrders");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationOrders_UserAddressId",
                table: "ApplicationOrders");

            migrationBuilder.DropColumn(
                name: "UserAddressId",
                table: "ApplicationOrders");

            migrationBuilder.AddColumn<long>(
                name: "UserAddressId",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryId",
                table: "ApplicationOrders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserAddressId",
                table: "Payments",
                column: "UserAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_UsersAddresses_UserAddressId",
                table: "Payments",
                column: "UserAddressId",
                principalTable: "UsersAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_UsersAddresses_UserAddressId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserAddressId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserAddressId",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryId",
                table: "ApplicationOrders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserAddressId",
                table: "ApplicationOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationOrders_UserAddressId",
                table: "ApplicationOrders",
                column: "UserAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationOrders_UsersAddresses_UserAddressId",
                table: "ApplicationOrders",
                column: "UserAddressId",
                principalTable: "UsersAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
