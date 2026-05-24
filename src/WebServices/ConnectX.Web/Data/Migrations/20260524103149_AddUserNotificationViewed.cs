using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectX.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNotificationViewed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserViewed",
                table: "UserNotifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_RecipientUserId_UserViewed_CreatedAt",
                table: "UserNotifications",
                columns: new[] { "RecipientUserId", "UserViewed", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserNotifications_RecipientUserId_UserViewed_CreatedAt",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "UserViewed",
                table: "UserNotifications");
        }
    }
}
