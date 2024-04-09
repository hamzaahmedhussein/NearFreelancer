using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35feef3b-bfa2-41ae-9b80-0872ef76c5be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43523ced-8c4f-47dc-9e18-b05fdad6b74e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9949757f-2ef3-4867-a44a-709fd7dc2c6a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "67ef1d23-556a-47ae-baee-37f950f40e76", "1", "Freelancer", "Freelancer" },
                    { "85e10ccc-fe0e-4055-b830-6248dd040598", "2", "ReservationProvider", "ReservationProvider" },
                    { "96d0e9b3-fc1d-447e-93e5-7dae40aff685", "0", "Customer", "Customer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "67ef1d23-556a-47ae-baee-37f950f40e76");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85e10ccc-fe0e-4055-b830-6248dd040598");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96d0e9b3-fc1d-447e-93e5-7dae40aff685");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "35feef3b-bfa2-41ae-9b80-0872ef76c5be", "0", "Customer", "Customer" },
                    { "43523ced-8c4f-47dc-9e18-b05fdad6b74e", "1", "Freelancer", "Freelancer" },
                    { "9949757f-2ef3-4867-a44a-709fd7dc2c6a", "2", "ReservationProvider", "ReservationProvider" }
                });
        }
    }
}
