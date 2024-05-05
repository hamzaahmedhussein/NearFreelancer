using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class aaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "030b85b3-4978-42ad-b80b-d9e4c779c331");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93817bc2-e7c6-4c41-845e-ab1b0aa78b42");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb044256-3e04-407f-a87b-fba3dbada4a8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "38ccf09b-06e4-40b8-9cd3-a0bedd83f93f", "1", "Freelancer", "Freelancer" },
                    { "3b7792da-77d6-4ba5-97c6-e2f258b81250", "0", "Customer", "Customer" },
                    { "58566680-ec31-4283-a47c-3b61a1a451ab", "2", "ReservationProvider", "ReservationProvider" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38ccf09b-06e4-40b8-9cd3-a0bedd83f93f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b7792da-77d6-4ba5-97c6-e2f258b81250");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58566680-ec31-4283-a47c-3b61a1a451ab");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "030b85b3-4978-42ad-b80b-d9e4c779c331", "1", "Freelancer", "Freelancer" },
                    { "93817bc2-e7c6-4c41-845e-ab1b0aa78b42", "2", "ReservationProvider", "ReservationProvider" },
                    { "cb044256-3e04-407f-a87b-fba3dbada4a8", "0", "Customer", "Customer" }
                });
        }
    }
}
