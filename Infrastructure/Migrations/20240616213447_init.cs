using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "10673ff3-fdfe-4256-866f-e6d8460dba19");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81838bce-f069-4b33-8dd8-c50f2fbda6a8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "3485668d-20f7-4436-926d-0477dec361c0", "Customer", "CUSTOMER" },
                    { "2", "5594d36b-b49a-4a97-b67c-4317a1f48925", "Freelancer", "FREELANCER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "10673ff3-fdfe-4256-866f-e6d8460dba19", "4d3b2a1b-f094-40fb-b441-7e19022c340d", "Freelancer", "FREELANCER" },
                    { "81838bce-f069-4b33-8dd8-c50f2fbda6a8", "3b3a59d7-4aee-40bc-8a20-d134b7beefd9", "Customer", "CUSTOMER" }
                });
        }
    }
}
