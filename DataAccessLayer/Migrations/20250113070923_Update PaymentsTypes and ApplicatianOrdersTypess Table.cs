using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentsTypesandApplicatianOrdersTypessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ApplicationOrdersTypes");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "PaymentsTypes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "PaymentsTypes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "ApplicationOrdersTypes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "ApplicationOrdersTypes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "PaymentsTypes");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "PaymentsTypes");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "ApplicationOrdersTypes");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "ApplicationOrdersTypes");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ApplicationOrdersTypes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
