using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class DeletePhonesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Fk_Users_PhoneId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Phones");

            migrationBuilder.DropIndex(
                name: "IX_Users_PhoneId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.AddColumn<long>(
                name: "PhoneId",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Phones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Phones__3214EC076BAE186A", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneId",
                table: "Users",
                column: "PhoneId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "Fk_Users_PhoneId",
                table: "Users",
                column: "PhoneId",
                principalTable: "Phones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
