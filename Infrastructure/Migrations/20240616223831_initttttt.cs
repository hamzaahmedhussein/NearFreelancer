using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initttttt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "902d6b48-91a1-4f91-917d-2a2e09188c92");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "7750258e-44f4-4e8a-8c11-8f99b1bd946b");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "598696b6-dc37-4ad2-b4b8-9a557cf523a4", "CUSTOMER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "396008e8-868a-43ed-b0c7-e30d8a039342", "FREELANCER" });
        }
    }
}
