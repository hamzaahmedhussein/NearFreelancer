using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class aafafa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b413f899-9818-402a-aad2-43aa372cb177");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f101e3e8-339c-4901-a252-c85f191fe2ab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f29a64e8-8c73-4b1e-9386-b969e75efb2c");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "b413f899-9818-402a-aad2-43aa372cb177", "0f2a9617-99b6-458e-bbc3-b16c57f197de", "Freelancer", "FREELANCER" },
                    { "f101e3e8-339c-4901-a252-c85f191fe2ab", "210dc22a-17d4-4129-8210-6fbbaefc1bac", "ReservationProvider", "RESERVATIONPROVIDER" },
                    { "f29a64e8-8c73-4b1e-9386-b969e75efb2c", "6bdf0f28-7e72-4d21-a30b-9574736ada2e", "Customer", "CUSTOMER" }
                });
        }
    }
}
