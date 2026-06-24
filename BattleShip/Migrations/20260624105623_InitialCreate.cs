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
                name: "Players_Data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players_Data", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShipSizeScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerAttempts = table.Column<int>(type: "INTEGER", nullable: false),
                    Number_ShipCells = table.Column<int>(type: "INTEGER", nullable: false),
                    Player_DataId = table.Column<int>(type: "INTEGER", nullable: true),
                    Player_DataId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipSizeScore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipSizeScore_Players_Data_Player_DataId",
                        column: x => x.Player_DataId,
                        principalTable: "Players_Data",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShipSizeScore_Players_Data_Player_DataId1",
                        column: x => x.Player_DataId1,
                        principalTable: "Players_Data",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShipSizeScore_Player_DataId",
                table: "ShipSizeScore",
                column: "Player_DataId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipSizeScore_Player_DataId1",
                table: "ShipSizeScore",
                column: "Player_DataId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipSizeScore");

            migrationBuilder.DropTable(
                name: "Players_Data");
        }
    }
}
