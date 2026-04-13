using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class RenameUsersAddressToUserAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_UsersAddresses_UserAddressId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersAddresses_Users_UserId",
                table: "UsersAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersAddresses",
                table: "UsersAddresses");

            migrationBuilder.RenameTable(
                name: "UsersAddresses",
                newName: "UserAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_UsersAddresses_UserId",
                table: "UserAddresses",
                newName: "IX_UserAddresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersAddresses_CityId",
                table: "UserAddresses",
                newName: "IX_UserAddresses_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAddresses",
                table: "UserAddresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_UserAddresses_UserAddressId",
                table: "Payments",
                column: "UserAddressId",
                principalTable: "UserAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddresses_Users_UserId",
                table: "UserAddresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_UserAddresses_UserAddressId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_Users_UserId",
                table: "UserAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAddresses",
                table: "UserAddresses");

            migrationBuilder.RenameTable(
                name: "UserAddresses",
                newName: "UsersAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddresses_UserId",
                table: "UsersAddresses",
                newName: "IX_UsersAddresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddresses_CityId",
                table: "UsersAddresses",
                newName: "IX_UsersAddresses_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersAddresses",
                table: "UsersAddresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_UsersAddresses_UserAddressId",
                table: "Payments",
                column: "UserAddressId",
                principalTable: "UsersAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAddresses_Users_UserId",
                table: "UsersAddresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
