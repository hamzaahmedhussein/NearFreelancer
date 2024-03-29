using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedingRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "OfferedServices");

            migrationBuilder.DropColumn(
                name: "ProfileType",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Descrioption",
                table: "OfferedServices",
                newName: "Description");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3578afaf-de98-411f-add2-b306a57f482c", "2", "ReservationProvider", "ReservationProvider" },
                    { "6345193c-6b2f-4dd8-84aa-b216eafd7a67", "1", "Freelancer", "Freelancer" },
                    { "e2e073c1-e39e-4aa7-957d-ece85b665664", "0", "Customer", "Customer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3578afaf-de98-411f-add2-b306a57f482c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6345193c-6b2f-4dd8-84aa-b216eafd7a67");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2e073c1-e39e-4aa7-957d-ece85b665664");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "OfferedServices",
                newName: "Descrioption");

            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                table: "OfferedServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfileType",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
