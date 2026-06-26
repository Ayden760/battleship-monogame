using System;
using System.Collections.Generic;
using BattleShip.GameData;
using BattleShip.GameObjects;
using BattleShip.Services;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Input;
namespace BattleShip.Features.GameOption;
#nullable enable

public class GameOptionController
{
    private const string DefaultPlayer1Name = "Name";
    private const string DefaultPlayer2Name = "Name";

    public GameOptions Options { get; private set; }
    private GameSettings _settings;
    private GameSession _session;
    private InputHandler _InputHandler;


    public bool IsEditingPlayer1Name { get; private set; }
    public bool IsEditingPlayer2Name { get; private set; }


    public GameOptionController(GameOptions gameOptions, GameSettings settings, GameSession session, InputHandler handler)
    {

        Options = gameOptions;
        _settings = settings;
        _session = session;
        _InputHandler = handler;
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
    public void SetEditingName(bool enabled, PlayerId Id)
    {
        switch (Id)
        {
            case PlayerId.Player1:
                IsEditingPlayer1Name = enabled;
                if (enabled && IsDefaultName(Options.Player1Name, DefaultPlayer1Name))
                {
                    Options.Player1Name = string.Empty;
                }
                break;
            case PlayerId.Player2:
                if (Options.Ai_Mode)
                {
                    break;
                }
                IsEditingPlayer2Name = enabled;
                if (enabled && IsDefaultName(Options.Player2Name, DefaultPlayer2Name))
                {
                    Options.Player2Name = string.Empty;
                }
                break;
        }

    }

    private static bool IsDefaultName(string currentName, string fallbackName)
    {
        if (string.IsNullOrWhiteSpace(currentName))
        {
            return false;
        }

        return string.Equals(currentName.Trim(), fallbackName, StringComparison.OrdinalIgnoreCase);
    }


    public void CancelNameEditingOnClick()
    {
        if (_InputHandler.CheckLeftMouseButtonClicked())
        {
            IsEditingPlayer1Name = false;
            IsEditingPlayer2Name = false;

        }

    }


    public void ApplyToGameData()
    {
        _settings.Initialize(Options);
        _session.InitializeSession(Options.Player1Name, Options.Player2Name);
    }

    private void HandleBackspace()
    {

        if (_InputHandler.WasKeyJustPressed(Keys.Back))
        {

            if (IsEditingPlayer1Name && Options.Player1Name.Length > 0)
            {
                Options.Player1Name = Options.Player1Name[..^1];

            }

            if (IsEditingPlayer2Name && Options.Player2Name.Length > 0)
            {
                Options.Player2Name = Options.Player2Name[..^1];
            }
        }
    }

    public void HandleTextInput(char c)
    {

        if (c == '\b')
            return;
        if (!IsEditingPlayer1Name && !IsEditingPlayer2Name)
            return;

        if (IsEditingPlayer1Name && Options.Player1Name.Length <= 18)
            Options.Player1Name += c;

        if (IsEditingPlayer2Name && Options.Player2Name.Length <= 18)
            Options.Player2Name += c;
    }
    public void Update()
    {
        CancelNameEditingOnClick();
        HandleBackspace();
    }

}