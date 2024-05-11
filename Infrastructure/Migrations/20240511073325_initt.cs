using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0051e6ef-1b1e-4f8a-8391-2c1b25b96bd5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "400e0f51-650c-483a-a4c2-7d4626deb13c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf769c4b-6ac3-439f-a6c8-91eabf661d06");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "0051e6ef-1b1e-4f8a-8391-2c1b25b96bd5", "a519f8ba-f308-461c-a339-19bc828f4eaa", "ReservationProvider", "RESERVATIONPROVIDER" },
                    { "400e0f51-650c-483a-a4c2-7d4626deb13c", "26092a56-9b0d-409f-8fed-b6d0fa1d3490", "Customer", "CUSTOMER" },
                    { "cf769c4b-6ac3-439f-a6c8-91eabf661d06", "982a9f99-ac2a-47c6-96dd-70a9cd4f98ab", "Freelancer", "FREELANCER" }
                });
        }
    }
}
