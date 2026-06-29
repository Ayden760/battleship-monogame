namespace BattleShip.GameData;

public class GameOptions
{



    //stores the number of ships of each ship size
    public int Two_tile { get; set; } = 1;
    public int Three_tile { get; set; } = 0;
    public int Four_tile { get; set; } = 0;
    public int Five_tile { get; set; } = 0;
    public int Difficulty { get; set; } = 1;
    public string Player1Name = "Name1";
    public string Player2Name = "Name2";
    public bool Ai_Mode { get; set; } = false;
    public int Rows { get; set; } = 10;
    public int Columns { get; set; } = 10;
    public bool DistanceMode { get; set; } = true;
    public bool BonusShotOnHit { get; set; } = false;

    public int TotalShips => Two_tile + Three_tile + Four_tile + Five_tile;
}