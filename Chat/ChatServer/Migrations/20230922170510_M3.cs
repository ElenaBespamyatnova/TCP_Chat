using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatServer.Migrations
{
    /// <inheritdoc />
    public partial class M3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "UserMessages");

            migrationBuilder.AddColumn<bool>(
                name: "IsSessionStarted",
                table: "UserMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSessionStarted",
                table: "UserMessages");

            migrationBuilder.AddColumn<int>(
                name: "Identifier",
                table: "UserMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
