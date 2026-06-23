
using BattleShip.GameData;

public class Score
{
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public GameMode Mode { get; set; } = GameMode.PvP;
    public int PlayerAttempts { get; set; }
    public int Number_ShipCells { get; set; }
}