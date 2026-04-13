using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserDefaultAddressIndexFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Ix_User_Default_Address",
                table: "UserAddresses");

            migrationBuilder.CreateIndex(
                name: "Ix_User_Default_Address",
                table: "UserAddresses",
                column: "UserId",
                unique: true,
                filter: "[IsDefault] = 1 AND [IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Ix_User_Default_Address",
                table: "UserAddresses");

            migrationBuilder.CreateIndex(
                name: "Ix_User_Default_Address",
                table: "UserAddresses",
                column: "UserId",
                unique: true,
                filter: "[IsDefault] = 1");
        }
    }
}
