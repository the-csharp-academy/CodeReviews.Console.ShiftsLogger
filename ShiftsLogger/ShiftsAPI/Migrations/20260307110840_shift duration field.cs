using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftsAPI.Migrations
{
    /// <inheritdoc />
    public partial class shiftdurationfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Shifts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Shifts");
        }
    }
}
