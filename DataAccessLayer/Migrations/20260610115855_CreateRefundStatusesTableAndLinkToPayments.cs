using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class CreateRefundStatusesTableAndLinkToPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RefundStatusId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RefundStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundStatuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RefundStatuses",
                columns: new[] { "Id", "DescriptionAr", "DescriptionEn", "Name" },
                values: new object[,]
                {
                    { 1, "طلب الاسترداد قيد المعالجة", "Refund request is being processed", "Pending" },
                    { 2, "تم تنفيذ عملية الاسترداد بنجاح", "Refund completed successfully", "Succeeded" },
                    { 3, "فشلت عملية الاسترداد", "Refund failed", "Failed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RefundStatusId",
                table: "Payments",
                column: "RefundStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_RefundStatuses_RefundStatusId",
                table: "Payments",
                column: "RefundStatusId",
                principalTable: "RefundStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_RefundStatuses_RefundStatusId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "RefundStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Payments_RefundStatusId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RefundStatusId",
                table: "Payments");
        }
    }
}
