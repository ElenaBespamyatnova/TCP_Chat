using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatServer.Migrations
{
    /// <inheritdoc />
    public partial class M4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSessionStarted",
                table: "UserMessages");

            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "UserMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "UserMessages");

            migrationBuilder.AddColumn<bool>(
                name: "IsSessionStarted",
                table: "UserMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
