using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCapp.Migrations
{
    /// <inheritdoc />
    public partial class addipaddressforeventlogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "EventLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "EventLogs");
        }
    }
}
