using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShiftsLoggerApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedShifts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "EndDate", "StartDate", "WorkerId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 2, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
