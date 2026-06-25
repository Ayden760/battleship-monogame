
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using BattleShip.GameData;

public class Player_Data
{
    public int Id { get; set; }
    public string PlayerName { get; set; } = "";
    public ICollection<Score> Scores { get; set; } = new List<Score>();
    public bool HasWon { get; set; }
    public bool IsAI { get; set; }
    public int AiDifficulty { get; set; } = 0;


}
public class Score
{
    public int Id { get; set; }
    public int PlayerID { get; set; }
    public Player_Data DataPlayer { get; set; }
    public int IMatchID { get; set; }
    public Match MatchData { get; set; }
    public int PlayerAttemps { get; set; }
    public int NumberShipCells { get; set; }

}
public class Match
{
    public int Id { get; set; }
    public DateTime MatchSetTime { get; set; }
    public DateTime GameStartTime { get; set; }
    public DateTime GameEndTime { get; set; }
    public bool Aborted { get; set; }
    public GameMode Mode { get; set; }
    public bool DistanceMode { get; set; }
    public bool BonusShotOnHit { get; set; }
    public ICollection<Score> Scores { get; set; } = new List<Score>();
}


