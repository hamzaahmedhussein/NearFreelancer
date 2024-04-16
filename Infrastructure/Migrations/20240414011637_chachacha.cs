using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class chachacha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4acb414c-fdf6-48ff-ad70-70a205e1b0b7");
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7515b25d-96f8-4d17-b66e-8d91ea967250");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87760280-3116-4851-8f5f-d8727a8b0468");

            migrationBuilder.DropColumn(
                name: "AvailableFrom",
                table: "ReservationProviders");

            migrationBuilder.DropColumn(
                name: "AvailableTo",
                table: "ReservationProviders");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "ReservationProviders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Availability",
                table: "ReservationProviders",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "ReservationProviders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Freelancers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Availability",
                table: "Freelancers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Freelancers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "ReservationProviders");

            migrationBuilder.DropColumn(
                name: "State",
                table: "ReservationProviders");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Freelancers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "ReservationProviders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableFrom",
                table: "ReservationProviders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableTo",
                table: "ReservationProviders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Freelancers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Availability",
                table: "Freelancers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4acb414c-fdf6-48ff-ad70-70a205e1b0b7", "0", "Customer", "Customer" },
                    { "7515b25d-96f8-4d17-b66e-8d91ea967250", "2", "ReservationProvider", "ReservationProvider" },
                    { "87760280-3116-4851-8f5f-d8727a8b0468", "1", "Freelancer", "Freelancer" }
                });
        }
    }
}
