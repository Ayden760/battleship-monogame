using System;
using System.Collections.Generic;
using BattleShip.GameData;

public class Player_Data
{
    public int Id { get; set; }
    public string PlayerName { get; set; } = "";
    public ICollection<MatchPlayer> MatchPlayers { get; set; } = new List<MatchPlayer>();

    public bool IsAI { get; set; }



}
public class MatchPlayer
{
    public int Id { get; set; }

    public int DataPlayerId { get; set; }
    public Player_Data DataPlayer { get; set; }

    public int MatchDataId { get; set; }
    public Match MatchData { get; set; }

    public int PlayerAttempts { get; set; }
    public int NumberShipCells { get; set; }
    public TimeSpan PlayersTime { get; set; } = TimeSpan.Zero;
    public bool HasWon { get; set; }

}
public class Match
{
    public int Id { get; set; }
    public DateTime MatchSetTime { get; set; }
    public DateTime GameStartTime { get; set; }
    public DateTime GameEndTime { get; set; }
    public bool Aborted { get; set; }
    public GameMode ModePlayer { get; set; }
    public bool DistanceMode { get; set; }
    public bool BonusShotOnHit { get; set; }
    public int AiDifficulty { get; set; } = 0;
    public ICollection<MatchPlayer> MatchPlayers { get; set; } = new List<MatchPlayer>();
}


