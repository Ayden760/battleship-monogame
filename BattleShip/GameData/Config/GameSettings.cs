using System;
using BattleShip.GameData;

namespace BattleShip.GameData;

public class GameSettings
{



    //stores the number of ships of each ship size
    public int Two_tile { get; private set; }
    public int Three_tile { get; private set; }
    public int Four_tile { get; private set; }
    public int Five_tile { get; private set; }


    public int TotalShips => Two_tile + Three_tile + Four_tile + Five_tile;

    public int Difficulty { get; private set; } = 2;

    public bool Ai_Mode { get; private set; } = false;

    public int Rows { get; private set; } = 10;
    public int Columns { get; private set; } = 10;
    public bool DistanceMode { get; private set; } = false;

    public bool BonusShotOnHit { get; private set; } = false;




    public GameSettings(GameOptions gameOptions)
    {
        Two_tile = gameOptions.Two_tile;
        Three_tile = gameOptions.Three_tile;
        Four_tile = gameOptions.Four_tile;
        Five_tile = gameOptions.Five_tile;
        Ai_Mode = gameOptions.Ai_Mode;
        DistanceMode = gameOptions.DistanceMode;
        Difficulty = gameOptions.Difficulty;
        BonusShotOnHit = gameOptions.BonusShotOnHit;

    }





}