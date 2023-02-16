using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCapp.Migrations
{
    /// <inheritdoc />
    public partial class updateuserandlogger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Loggers_LoggerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_LoggerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LoggerId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Loggers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loggers_UserId",
                table: "Loggers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loggers_Users_UserId",
                table: "Loggers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loggers_Users_UserId",
                table: "Loggers");

            migrationBuilder.DropIndex(
                name: "IX_Loggers_UserId",
                table: "Loggers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Loggers");

            migrationBuilder.AddColumn<int>(
                name: "LoggerId",
                table: "Users",
                type: "int",
                nullable: true);

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
    }
}
