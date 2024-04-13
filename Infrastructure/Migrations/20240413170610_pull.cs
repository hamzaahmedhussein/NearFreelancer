using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class pull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "77d7312d-3e0b-4721-a3f0-36b93369ed63");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1c96699-fb57-410a-851c-58ce17268c2f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9cdb562-f49e-44c6-83ba-0dd87a3a10e5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "019b3309-2b72-4759-b0bb-5343f1294bac", "1", "Freelancer", "Freelancer" },
                    { "76118eb1-d614-4b5c-8329-10211b6a23d5", "2", "ReservationProvider", "ReservationProvider" },
                    { "a25a113e-a5cc-4656-bcc1-0b78439c95de", "0", "Customer", "Customer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019b3309-2b72-4759-b0bb-5343f1294bac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "76118eb1-d614-4b5c-8329-10211b6a23d5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a25a113e-a5cc-4656-bcc1-0b78439c95de");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "77d7312d-3e0b-4721-a3f0-36b93369ed63", "2", "ReservationProvider", "ReservationProvider" },
                    { "b1c96699-fb57-410a-851c-58ce17268c2f", "0", "Customer", "Customer" },
                    { "f9cdb562-f49e-44c6-83ba-0dd87a3a10e5", "1", "Freelancer", "Freelancer" }
                });
        }
    }
}
