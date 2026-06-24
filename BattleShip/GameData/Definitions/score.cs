
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using BattleShip.GameData;

public class Player_Data
{
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public List<ShipSizeScore> HighScores_PvE { get; set; } = new();
    public List<ShipSizeScore> Highscores_PvP { get; set; } = new();

    public ShipSizeScore GetOrCreateEntry(GameMode mode, int numberShipCells)
    {
        //is either the AI or PvP list
        var list = GetScoreList(mode);
        if (list == null)
        {

            //creates list if no list exists
            list = new List<ShipSizeScore>();
            SetScoreList(mode, list);
        }
        //searches for the List item with the exact numberShipCells
        var entry = list.FirstOrDefault(item => item.Number_ShipCells == numberShipCells);
        if (entry == null)
        {
            //if it doesnt if any item it creates a new one
            entry = new ShipSizeScore
            {
                Number_ShipCells = numberShipCells,
                PlayerAttempts = 0
            };
            //adds new Item to the List
            list.Add(entry);
        }

        return entry;
    }

    public void ResetEntry(GameMode mode, int numberShipCells)
    {
        var entry = GetOrCreateEntry(mode, numberShipCells);
        entry.PlayerAttempts = 0;
    }

    public void IncrementAttempts(GameMode mode, int numberShipCells)
    {
        var entry = GetOrCreateEntry(mode, numberShipCells);
        entry.PlayerAttempts++;
    }

    public int GetAttempts(GameMode mode, int numberShipCells)
    {
        return GetOrCreateEntry(mode, numberShipCells).PlayerAttempts;
    }

    private List<ShipSizeScore> GetScoreList(GameMode mode)
    {
        return mode == GameMode.AI ? HighScores_PvE : Highscores_PvP;
    }

    private void SetScoreList(GameMode mode, List<ShipSizeScore> list)
    {
        if (mode == GameMode.AI)
        {
            HighScores_PvE = list;
            return;
        }

        Highscores_PvP = list;
    }
}

public class ShipSizeScore
{
    public int Id { get; set; }
    public int PlayerAttempts { get; set; }
    public int Number_ShipCells { get; set; }
}
