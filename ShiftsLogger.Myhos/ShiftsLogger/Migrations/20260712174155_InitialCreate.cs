using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShiftsLogger.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkerShifts",
                columns: table => new
                {
                    ShiftId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkerName = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    HoursWorked = table.Column<decimal>(type: "TEXT", nullable: false),
                    ShiftTime = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerShifts", x => x.ShiftId);
                });

            migrationBuilder.InsertData(
                table: "WorkerShifts",
                columns: new[] { "ShiftId", "CreatedAt", "Date", "EndTime", "HoursWorked", "ShiftTime", "StartTime", "UpdateAt", "WorkerName" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 1, 5, 50, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 7, 1), new TimeOnly(14, 0, 0), 8.0m, "Morning", new TimeOnly(6, 0, 0), new DateTime(2026, 7, 1, 14, 5, 0, 0, DateTimeKind.Unspecified), "Juan Pérez" },
                    { 2, new DateTime(2026, 7, 1, 13, 50, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 7, 1), new TimeOnly(22, 0, 0), 8.0m, "Afternoon", new TimeOnly(14, 0, 0), new DateTime(2026, 7, 1, 22, 5, 0, 0, DateTimeKind.Unspecified), "María Gómez" },
                    { 3, new DateTime(2026, 7, 2, 21, 50, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 7, 2), new TimeOnly(6, 0, 0), 8.0m, "Night", new TimeOnly(22, 0, 0), new DateTime(2026, 7, 3, 6, 5, 0, 0, DateTimeKind.Unspecified), "Carlos Rodríguez" },
                    { 4, new DateTime(2026, 7, 2, 6, 50, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 7, 2), new TimeOnly(15, 0, 0), 8.0m, "Morning", new TimeOnly(7, 0, 0), new DateTime(2026, 7, 2, 15, 5, 0, 0, DateTimeKind.Unspecified), "Ana Martínez" },
                    { 5, new DateTime(2026, 7, 3, 7, 50, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 7, 3), new TimeOnly(16, 0, 0), 8.0m, "Morning", new TimeOnly(8, 0, 0), new DateTime(2026, 7, 3, 16, 5, 0, 0, DateTimeKind.Unspecified), "Luis Fernández" },
                    { 6, new DateTime(2026, 7, 3, 14, 50, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 7, 3), new TimeOnly(23, 0, 0), 8.0m, "Afternoon", new TimeOnly(15, 0, 0), new DateTime(2026, 7, 3, 23, 5, 0, 0, DateTimeKind.Unspecified), "Laura Sánchez" },
                    { 7, new DateTime(2026, 7, 4, 6, 20, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 7, 4), new TimeOnly(14, 30, 0), 8.0m, "Morning", new TimeOnly(6, 30, 0), new DateTime(2026, 7, 4, 14, 35, 0, 0, DateTimeKind.Unspecified), "Pedro Ramírez" },
                    { 8, new DateTime(2026, 7, 4, 13, 20, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 7, 4), new TimeOnly(21, 30, 0), 8.0m, "Afternoon", new TimeOnly(13, 30, 0), new DateTime(2026, 7, 4, 21, 35, 0, 0, DateTimeKind.Unspecified), "Sofía Torres" },
                    { 9, new DateTime(2026, 7, 5, 21, 50, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 7, 5), new TimeOnly(6, 0, 0), 8.0m, "Night", new TimeOnly(22, 0, 0), new DateTime(2026, 7, 6, 6, 5, 0, 0, DateTimeKind.Unspecified), "Diego Morales" },
                    { 10, new DateTime(2026, 7, 5, 8, 50, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 7, 5), new TimeOnly(17, 0, 0), 8.0m, "Morning", new TimeOnly(9, 0, 0), new DateTime(2026, 7, 5, 17, 5, 0, 0, DateTimeKind.Unspecified), "Camila Herrera" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkerShifts");
        }
    }
}
