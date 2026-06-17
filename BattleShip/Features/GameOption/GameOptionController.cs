using System;
using System.Collections.Generic;
using BattleShip.GameData;
using CsvHelper.Configuration.Attributes;
using Microsoft.Extensions.DependencyInjection;
namespace BattleShip.Features.GameOption;


public class GameOptionController
{
    public GameOptions Options { get; private set; }
    private GameSettings _settings;
    private GameSession _session;


    public GameOptionController(GameOptions gameOptions, GameSettings settings, GameSession session)
    {

        Options = gameOptions;
        _settings = settings;
        _session = session;
    }
    public void SetAi(bool enabled)
    {
        Options.Ai_Mode = enabled;
    }

    public void ChangeDifficulty(int delta)
    {
        if (!Options.Ai_Mode)
            return;

        Options.Difficulty += delta;

        if (Options.Difficulty < 1)
            Options.Difficulty = 1;

        if (Options.Difficulty > 4)
            Options.Difficulty = 4;
    }
    public void ChangeShip(ShipType type, int delta)
    {
        if (delta > 0)
        {
            if (!CanAddShip()) return;
            switch (type)
            {
                case ShipType.Two:
                    Options.Two_tile++;
                    break;
                case ShipType.Three:
                    Options.Three_tile++;
                    break;
                case ShipType.Four:
                    if (Options.Four_tile < 3)
                        Options.Four_tile++;
                    break;
                case ShipType.Five:
                    if (Options.Five_tile < 2)
                        Options.Five_tile++;
                    break;
            }
        }
        else
        {
            if (!CanRemoveShip()) return;

            switch (type)
            {
                case ShipType.Two:
                    if (Options.Two_tile > 0)
                        Options.Two_tile--;
                    break;
                case ShipType.Three:
                    if (Options.Three_tile > 0)
                        Options.Three_tile--;
                    break;
                case ShipType.Four:
                    if (Options.Four_tile > 0)
                        Options.Four_tile--;
                    break;
                case ShipType.Five:
                    if (Options.Five_tile > 0)
                        Options.Five_tile--;
                    break;
            }
        }


    }
    private bool CanAddShip()
    {
        return Options.TotalShips < 9;
    }

    private bool CanRemoveShip()
    {
        return Options.TotalShips > 1;
    }
    public void SetDistance(bool enabled)
    {
        Options.DistanceMode = enabled;
    }
    public void SetBonusShot(bool enabled)
    {
        Options.BonusShotOnHit = enabled;
    }
    public void ApplyToGameData()
    {

        _settings.Initialize(Options);
        _session.InitializeSession();
    }
}