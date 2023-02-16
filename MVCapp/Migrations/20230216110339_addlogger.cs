using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCapp.Migrations
{
    /// <inheritdoc />
    public partial class addlogger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoggerId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Loggers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Information = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loggers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_LoggerId",
                table: "Users",
                column: "LoggerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Loggers_LoggerId",
                table: "Users",
                column: "LoggerId",
                principalTable: "Loggers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Loggers_LoggerId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Loggers");

            migrationBuilder.DropIndex(
                name: "IX_Users_LoggerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LoggerId",
                table: "Users");
        }
    }
}
