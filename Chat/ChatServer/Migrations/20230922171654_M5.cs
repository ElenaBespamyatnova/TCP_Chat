using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatServer.Migrations
{
    /// <inheritdoc />
    public partial class M5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "UserMessages");

            migrationBuilder.AddColumn<string>(
                name: "ChatName",
                table: "UserMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatName",
                table: "UserMessages");

            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "UserMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
