using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class safadhaddg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d47bd50-50e1-4d79-8895-cf9ea632fc71");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "760cb559-f95e-4215-ae10-4fe12873269b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a700b0ec-c28d-4f92-a975-6c2684736819");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e7ace63-d292-4623-839a-7076127e276e", "5325c35d-ee51-4e9f-a029-57e165af4a77", "Customer", "CUSTOMER" },
                    { "26f68c14-b5fe-43a1-93ac-8245da0d7e97", "75990cd8-603f-4162-b66e-f7f3bc0d8b96", "ReservationProvider", "RESERVATIONPROVIDER" },
                    { "918028f6-7f9c-4d9d-b357-ea8c1114f14d", "6399591c-4447-407f-ba6a-883c55103e46", "Freelancer", "FREELANCER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e7ace63-d292-4623-839a-7076127e276e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26f68c14-b5fe-43a1-93ac-8245da0d7e97");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "918028f6-7f9c-4d9d-b357-ea8c1114f14d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3d47bd50-50e1-4d79-8895-cf9ea632fc71", "7ed12fd0-6bd5-4011-9ec3-1c9c0b50a030", "Freelancer", "FREELANCER" },
                    { "760cb559-f95e-4215-ae10-4fe12873269b", "ac0c5e61-1945-4e6c-8a68-e16e261e5fa3", "Customer", "CUSTOMER" },
                    { "a700b0ec-c28d-4f92-a975-6c2684736819", "845365c9-1287-41a4-8f88-338f72d224fe", "ReservationProvider", "RESERVATIONPROVIDER" }
                });
        }
    }
}
