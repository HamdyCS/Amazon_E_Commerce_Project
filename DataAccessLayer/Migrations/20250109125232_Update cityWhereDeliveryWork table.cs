using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdatecityWhereDeliveryWorktable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationOrders_DeliveryId",
                table: "ApplicationOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_CitiesWhereDeliveiesWorks_DeliveryId",
                table: "CitiesWhereDeliveiesWorks");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryId",
                table: "CitiesWhereDeliveiesWorks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryId",
                table: "ApplicationOrders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationOrders_Users_DeliveryId",
                table: "ApplicationOrders",
                column: "DeliveryId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitiesWhereDeliveiesWorks_Users_DeliveryId",
                table: "CitiesWhereDeliveiesWorks",
                column: "DeliveryId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationOrders_Users_DeliveryId",
                table: "ApplicationOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_CitiesWhereDeliveiesWorks_Users_DeliveryId",
                table: "CitiesWhereDeliveiesWorks");

            migrationBuilder.AlterColumn<long>(
                name: "DeliveryId",
                table: "CitiesWhereDeliveiesWorks",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<long>(
                name: "DeliveryId",
                table: "ApplicationOrders",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Deliveri__3214EC07322C23DA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deliveries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_UserId",
                table: "Deliveries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationOrders_DeliveryId",
                table: "ApplicationOrders",
                column: "DeliveryId",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitiesWhereDeliveiesWorks_DeliveryId",
                table: "CitiesWhereDeliveiesWorks",
                column: "DeliveryId",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
