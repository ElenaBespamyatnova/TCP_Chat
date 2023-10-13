using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Migrations
{
    /// <inheritdoc />
    public partial class V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Message_UserMessagesId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UserMessagesId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserMessagesId",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserId",
                table: "Message",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_UserId",
                table: "Message",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_UserId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_UserId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Message");

            migrationBuilder.AddColumn<int>(
                name: "UserMessagesId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserMessagesId",
                table: "User",
                column: "UserMessagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Message_UserMessagesId",
                table: "User",
                column: "UserMessagesId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
