using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2fd18829-46c3-40be-9c73-63a0c235d9ac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36696970-c4ee-4d4b-abb6-c9b38bf9ffcb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5de1c0d8-abd3-430d-8ddc-1a27f6514675");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1baab264-8852-452f-880b-3e974646f1c2", "0", "Customer", "Customer" },
                    { "6a9f84c5-cc2b-4c9a-8595-52b2326ae4ec", "2", "ReservationProvider", "ReservationProvider" },
                    { "ab2230a3-4641-4d53-86d2-c60e77678262", "1", "Freelancer", "Freelancer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1baab264-8852-452f-880b-3e974646f1c2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a9f84c5-cc2b-4c9a-8595-52b2326ae4ec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab2230a3-4641-4d53-86d2-c60e77678262");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2fd18829-46c3-40be-9c73-63a0c235d9ac", "0", "Customer", "Customer" },
                    { "36696970-c4ee-4d4b-abb6-c9b38bf9ffcb", "2", "ReservationProvider", "ReservationProvider" },
                    { "5de1c0d8-abd3-430d-8ddc-1a27f6514675", "1", "Freelancer", "Freelancer" }
                });
        }
    }
}
