using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class iinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14c717f9-3c3c-4211-98b7-eb3351d79c93");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d220b1a-b894-46fc-b547-10af01e359ac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89c62224-5e07-4dc4-abb1-1c3f408af649");

            migrationBuilder.DropColumn(
                name: "IsCustomerSent",
                table: "Messages");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomerSent",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "14c717f9-3c3c-4211-98b7-eb3351d79c93", "2", "ReservationProvider", "ReservationProvider" },
                    { "3d220b1a-b894-46fc-b547-10af01e359ac", "0", "Customer", "Customer" },
                    { "89c62224-5e07-4dc4-abb1-1c3f408af649", "1", "Freelancer", "Freelancer" }
                });
        }
    }
}
