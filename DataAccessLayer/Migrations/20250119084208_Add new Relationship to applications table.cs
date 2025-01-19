using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddnewRelationshiptoapplicationstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Applications_ReturnApplicationId",
                table: "Applications",
                column: "ReturnApplicationId",
                unique: true,
                filter: "[ReturnApplicationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Applications_ReturnApplicationId",
                table: "Applications",
                column: "ReturnApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Applications_ReturnApplicationId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ReturnApplicationId",
                table: "Applications");
        }
    }
}
