using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class CreateindexinProductstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_Name_Ar",
                table: "Products",
                column: "Name_Ar");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name_En",
                table: "Products",
                column: "Name_En");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Name_Ar",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Name_En",
                table: "Products");
        }
    }
}
