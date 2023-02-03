using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Event.Dal.Migrations
{
    public partial class NewDbStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Organizer",
                table: "Event");

            migrationBuilder.RenameColumn(
                name: "Speaker",
                table: "Event",
                newName: "CreatedBy");

            migrationBuilder.CreateTable(
                name: "Organizer",
                columns: table => new
                {
                    OrganizerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizer", x => x.OrganizerId);
                    table.ForeignKey(
                        name: "FK_Organizer_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Speaker",
                columns: table => new
                {
                    SponsorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Speaker", x => x.SponsorId);
                    table.ForeignKey(
                        name: "FK_Speaker_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizer_EventId",
                table: "Organizer",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Speaker_EventId",
                table: "Speaker",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Organizer");

            migrationBuilder.DropTable(
                name: "Speaker");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Event",
                newName: "Speaker");

            migrationBuilder.AddColumn<string>(
                name: "Organizer",
                table: "Event",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
