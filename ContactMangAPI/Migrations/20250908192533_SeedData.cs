using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactMangAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "BirthDate", "Email", "FirstName", "LastName", "PhoneNumber", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "sara.hassan@example.com", "Sara", "Hassan", "+201001234567", 1 },
                    { 2, new DateTime(1998, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "ali.khaled@example.com", "Ali", "Khaled", "+201112223334", 1 },
                    { 3, new DateTime(1995, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "mona.adel@example.com", "Mona", "Adel", "+201223344556", 1 },
                    { 4, new DateTime(1992, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "hany.tarek@example.com", "Hany", "Tarek", "+201334455667", 1 },
                    { 5, new DateTime(1999, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "nour.salem@example.com", "Nour", "Salem", "+201445566778", 1 },
                    { 6, new DateTime(2001, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "youssef.ibrahim@example.com", "Youssef", "Ibrahim", "+201556677889", 1 },
                    { 7, new DateTime(1996, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "laila.mahmoud@example.com", "Laila", "Mahmoud", "+201667788990", 1 },
                    { 8, new DateTime(1994, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "karim.omar@example.com", "Karim", "Omar", "+201778899001", 1 },
                    { 9, new DateTime(1997, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "dina.fouad@example.com", "Dina", "Fouad", "+201889900112", 1 },
                    { 10, new DateTime(1993, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "mostafa.samir@example.com", "Mostafa", "Samir", "+201990011223", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
