using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class RenameApplicationTableToAplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_AspNetUsers_UserId",
                table: "Application");

            migrationBuilder.RenameTable(
                name: "Application",
                newName: "Applications");

            migrationBuilder.RenameIndex(
                name: "IX_Application_UserId",
                table: "Applications",
                newName: "IX_Applications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Application_ApplicationTypeId",
                table: "Applications",
                newName: "IX_Applications_ApplicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_UserId",
                table: "Applications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_UserId",
                table: "Applications");

            migrationBuilder.RenameTable(
                name: "Applications",
                newName: "Application");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_UserId",
                table: "Application",
                newName: "IX_Application_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_ApplicationTypeId",
                table: "Application",
                newName: "IX_Application_ApplicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Application_AspNetUsers_UserId",
                table: "Application",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
