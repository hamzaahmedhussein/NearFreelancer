using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class aaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "29b64d24-c486-4f97-9439-04922b348deb", "0", "Customer", "Customer" },
                    { "56b99aa5-b285-4926-9766-736fa334ede7", "1", "Freelancer", "Freelancer" },
                    { "ddd7fa91-3de5-4495-939a-5e0df7bee8a4", "2", "ReservationProvider", "ReservationProvider" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29b64d24-c486-4f97-9439-04922b348deb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "56b99aa5-b285-4926-9766-736fa334ede7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ddd7fa91-3de5-4495-939a-5e0df7bee8a4");

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
    }
}
