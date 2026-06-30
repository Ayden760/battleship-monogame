namespace BattleShip.GameData;

public class GameOptions
{
    //stores the number of ships of each ship size
    public int Two_tile { get; set; }
    public int Three_tile { get; set; }
    public int Four_tile { get; set; }
    public int Five_tile { get; set; }
    public int Difficulty { get; set; }
    public string Player1Name { get; set; }
    public string Player2Name { get; set; }
    public bool Ai_Mode { get; set; }
    public int Rows { get; set; } = 10;
    public int Columns { get; set; } = 10;
    public bool DistanceMode { get; set; }
    public bool BonusShotOnHit { get; set; }

    public int TotalShips => Two_tile + Three_tile + Four_tile + Five_tile;

    public void Reset()
    {
        Two_tile = 1;
        Three_tile = 0;
        Four_tile = 0;
        Five_tile = 0;
        Difficulty = 1;
        Player1Name = "Name1";
        Player2Name = "Name2";
        Ai_Mode = false;
        Rows = 10;
        Columns = 10;
        DistanceMode = true;
        BonusShotOnHit = false;
    }
}