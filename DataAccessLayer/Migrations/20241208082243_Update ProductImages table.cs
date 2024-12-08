using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductImagestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "ProductImages",
                type: "varbinary(max)",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDeletion",
                table: "ProductImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfDeletion",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductImages");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "ProductImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }
    }
}
