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
                    Mode = table.Column<int>(type: "INTEGER", nullable: false),
                    DistanceMode = table.Column<bool>(type: "INTEGER", nullable: false),
                    BonusShotOnHit = table.Column<bool>(type: "INTEGER", nullable: false)
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
                    HasWon = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsAI = table.Column<bool>(type: "INTEGER", nullable: false),
                    AiDifficulty = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players_Data", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerID = table.Column<int>(type: "INTEGER", nullable: false),
                    DataPlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    IMatchID = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchDataId = table.Column<int>(type: "INTEGER", nullable: true),
                    PlayerAttemps = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberShipCells = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scores_Matches_MatchDataId",
                        column: x => x.MatchDataId,
                        principalTable: "Matches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Scores_Players_Data_DataPlayerId",
                        column: x => x.DataPlayerId,
                        principalTable: "Players_Data",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scores_DataPlayerId",
                table: "Scores",
                column: "DataPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_MatchDataId",
                table: "Scores",
                column: "MatchDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Players_Data");
        }
    }
}
