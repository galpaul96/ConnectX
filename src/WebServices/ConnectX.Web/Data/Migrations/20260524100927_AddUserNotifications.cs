using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectX.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientUserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IssuerUserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Topic = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EventStartsAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EventEndsAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ActionUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    SourceType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SourceId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotifications_AspNetUsers_IssuerUserId",
                        column: x => x.IssuerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserNotifications_AspNetUsers_RecipientUserId",
                        column: x => x.RecipientUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_ExternalId",
                table: "UserNotifications",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_IsDeleted_UpdatedDate",
                table: "UserNotifications",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_IssuerUserId",
                table: "UserNotifications",
                column: "IssuerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_RecipientUserId_CreatedAt",
                table: "UserNotifications",
                columns: new[] { "RecipientUserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_RecipientUserId_EventStartsAt",
                table: "UserNotifications",
                columns: new[] { "RecipientUserId", "EventStartsAt" });

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_RecipientUserId_SourceType_SourceId",
                table: "UserNotifications",
                columns: new[] { "RecipientUserId", "SourceType", "SourceId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserNotifications");
        }
    }
}
