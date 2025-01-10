using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShippingCoststable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingCosts_Users_UserId",
                table: "ShippingCosts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ShippingCosts",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingCosts_UserId",
                table: "ShippingCosts",
                newName: "IX_ShippingCosts_CreatedBy");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ShippingCosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingCosts_Users_CreatedBy",
                table: "ShippingCosts",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingCosts_Users_CreatedBy",
                table: "ShippingCosts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ShippingCosts");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ShippingCosts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingCosts_CreatedBy",
                table: "ShippingCosts",
                newName: "IX_ShippingCosts_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingCosts_Users_UserId",
                table: "ShippingCosts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
