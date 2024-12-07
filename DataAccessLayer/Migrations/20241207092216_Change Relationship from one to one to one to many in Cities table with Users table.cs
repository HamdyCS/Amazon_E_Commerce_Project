using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationshipfromonetoonetoonetomanyinCitiestablewithUserstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cities_CreatedBy",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CreatedBy",
                table: "Cities",
                column: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cities_CreatedBy",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CreatedBy",
                table: "Cities",
                column: "CreatedBy",
                unique: true);
        }
    }
}
