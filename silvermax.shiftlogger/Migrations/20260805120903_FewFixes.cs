using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace silvermax.shiftlogger.Migrations
{
    /// <inheritdoc />
    public partial class FewFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Shifts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Shifts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Shifts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "Shifts",
                type: "bigint",
                nullable: false,
                computedColumnSql: "DATEDIFF_BIG(SECOND, StartTime, EndTime)",
                stored: true);
        }
    }
}
