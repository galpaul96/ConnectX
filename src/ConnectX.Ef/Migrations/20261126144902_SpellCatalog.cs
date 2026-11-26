using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectX.Ef.Migrations
{
    /// <inheritdoc />
    public partial class SpellCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "Spells",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    SourceId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Slug = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    StorageFile = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpellComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Type = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Label = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<JsonElement>(type: "jsonb", nullable: false),
                    Html = table.Column<string>(type: "text", nullable: false),
                    SpellId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpellComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpellComponents_Spells_SpellId",
                        column: x => x.SpellId,
                        principalTable: "Spells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpellComponents_IsDeleted_UpdatedDate",
                table: "SpellComponents",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_SpellComponents_SpellId_Position",
                table: "SpellComponents",
                columns: new[] { "SpellId", "Position" });

            migrationBuilder.CreateIndex(
                name: "IX_SpellComponents_SpellId_Type",
                table: "SpellComponents",
                columns: new[] { "SpellId", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spells_IsDeleted_UpdatedDate",
                table: "Spells",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Spells_Name_Id",
                table: "Spells",
                columns: new[] { "Name", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Spells_Slug",
                table: "Spells",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spells_SourceId",
                table: "Spells",
                column: "SourceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpellComponents");

            migrationBuilder.DropTable(
                name: "Spells");
        }
    }
}
