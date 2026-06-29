using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleShip.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MatchSetTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GameStartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GameEndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Aborted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModePlayer = table.Column<int>(type: "INTEGER", nullable: false),
                    DistanceMode = table.Column<bool>(type: "INTEGER", nullable: false),
                    BonusShotOnHit = table.Column<bool>(type: "INTEGER", nullable: false),
                    AiDifficulty = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players_Data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: true),
                    IsAI = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players_Data", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataPlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchDataId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerAttempts = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberShipCells = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayersTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    HasWon = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchPlayers_Matches_MatchDataId",
                        column: x => x.MatchDataId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchPlayers_Players_Data_DataPlayerId",
                        column: x => x.DataPlayerId,
                        principalTable: "Players_Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayers_DataPlayerId",
                table: "MatchPlayers",
                column: "DataPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayers_MatchDataId",
                table: "MatchPlayers",
                column: "MatchDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchPlayers");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Players_Data");
        }
    }
}
