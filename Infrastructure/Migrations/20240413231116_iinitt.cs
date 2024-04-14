using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class iinitt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Freelancers_AspNetUsers_OwnerId",
                table: "Freelancers");

            migrationBuilder.DropIndex(
                name: "IX_Freelancers_OwnerId",
                table: "Freelancers");

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

            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "Freelancers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Freelancers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4acb414c-fdf6-48ff-ad70-70a205e1b0b7", "0", "Customer", "Customer" },
                    { "7515b25d-96f8-4d17-b66e-8d91ea967250", "2", "ReservationProvider", "ReservationProvider" },
                    { "87760280-3116-4851-8f5f-d8727a8b0468", "1", "Freelancer", "Freelancer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Freelancers_OwnerId",
                table: "Freelancers",
                column: "OwnerId",
                unique: true,
                filter: "[OwnerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Freelancers_AspNetUsers_OwnerId",
                table: "Freelancers",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Freelancers_AspNetUsers_OwnerId",
                table: "Freelancers");

            migrationBuilder.DropIndex(
                name: "IX_Freelancers_OwnerId",
                table: "Freelancers");

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

            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "Freelancers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Freelancers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "019b3309-2b72-4759-b0bb-5343f1294bac", "1", "Freelancer", "Freelancer" },
                    { "76118eb1-d614-4b5c-8329-10211b6a23d5", "2", "ReservationProvider", "ReservationProvider" },
                    { "a25a113e-a5cc-4656-bcc1-0b78439c95de", "0", "Customer", "Customer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Freelancers_OwnerId",
                table: "Freelancers",
                column: "OwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Freelancers_AspNetUsers_OwnerId",
                table: "Freelancers",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
