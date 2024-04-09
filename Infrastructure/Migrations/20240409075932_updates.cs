using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6025d525-5abc-4d7f-962a-57e7ecfc63bb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b821298a-3dee-4e80-8d7a-1caebda595b1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d70b88a3-ebf0-49e5-86e4-36646cdb5907");

            migrationBuilder.DropColumn(
                name: "AvailableFrom",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "AvailableTo",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ReservationProviders");

            migrationBuilder.RenameColumn(
                name: "profession",
                table: "ServiceProviders",
                newName: "Profession");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "AspNetUsers",
                newName: "Street");

            migrationBuilder.AddColumn<bool>(
                name: "Availability",
                table: "ServiceProviders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ServiceProviders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "ServiceProviders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ReservationProviders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "ReservationProviders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "35feef3b-bfa2-41ae-9b80-0872ef76c5be", "0", "Customer", "Customer" },
                    { "43523ced-8c4f-47dc-9e18-b05fdad6b74e", "1", "Freelancer", "Freelancer" },
                    { "9949757f-2ef3-4867-a44a-709fd7dc2c6a", "2", "ReservationProvider", "ReservationProvider" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35feef3b-bfa2-41ae-9b80-0872ef76c5be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43523ced-8c4f-47dc-9e18-b05fdad6b74e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9949757f-2ef3-4867-a44a-709fd7dc2c6a");

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "City",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "City",
                table: "ReservationProviders");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "ReservationProviders");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Profession",
                table: "ServiceProviders",
                newName: "profession");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "AspNetUsers",
                newName: "Location");

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableFrom",
                table: "ServiceProviders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableTo",
                table: "ServiceProviders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ServiceProviders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ReservationProviders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6025d525-5abc-4d7f-962a-57e7ecfc63bb", "1", "Freelancer", "Freelancer" },
                    { "b821298a-3dee-4e80-8d7a-1caebda595b1", "2", "ReservationProvider", "ReservationProvider" },
                    { "d70b88a3-ebf0-49e5-86e4-36646cdb5907", "0", "Customer", "Customer" }
                });
        }
    }
}
