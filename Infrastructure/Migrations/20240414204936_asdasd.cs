using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asdasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6939322f-0555-46ae-8b31-c08d14557079");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "740d6156-ba8a-4d9a-8232-808f43eb2bb8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bcb097d2-effd-4d2a-9ac8-380b8d51845a");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "OfferedServices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6753db72-7549-4cb6-9112-941e95da9988", "0", "Customer", "Customer" },
                    { "880e57ec-6516-4e42-b454-b786b913459f", "1", "Freelancer", "Freelancer" },
                    { "9a50920b-3a5d-403d-9776-dc66eccc569d", "2", "ReservationProvider", "ReservationProvider" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6753db72-7549-4cb6-9112-941e95da9988");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "880e57ec-6516-4e42-b454-b786b913459f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a50920b-3a5d-403d-9776-dc66eccc569d");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "OfferedServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6939322f-0555-46ae-8b31-c08d14557079", "1", "Freelancer", "Freelancer" },
                    { "740d6156-ba8a-4d9a-8232-808f43eb2bb8", "2", "ReservationProvider", "ReservationProvider" },
                    { "bcb097d2-effd-4d2a-9ac8-380b8d51845a", "0", "Customer", "Customer" }
                });
        }
    }
}
