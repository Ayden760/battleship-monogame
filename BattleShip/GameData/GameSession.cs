using System;
using BattleShip.Functions;
using BattleShip.GameObjects;
using BattleShip.Services;
using Gum.Wireframe;
namespace BattleShip.GameData;

using Data = GameData;
public class GameSession
{
    private readonly GameSettings _settings;
    private readonly InputHandler _inputHandler;
    private readonly GameValidations _gamValidations;


    public AI Ai;
    public Player Player1;
    public Player Player2;

    public Player CurrentPlayer { get; set; }
    public Player OldPlayer { get; set; }

    //Game Options

    public GameSession(GameSettings settings, InputHandler handler, GameValidations gameValidations)
    {
        _settings = settings;
        _inputHandler = handler;
        _gamValidations = gameValidations;
    }
    public void InitializeSession()
    {
        Player1 = new Player(_settings.Rows, _settings.Columns, "Player1", _inputHandler, _gamValidations);

        if (!_settings.Ai_Mode)
        {
            Player2 = new Player(_settings.Rows, _settings.Columns, "Player2", _inputHandler, _gamValidations);
            Ai = null;
        }
        else
        {
            Ai = new AI(_settings.Rows, _settings.Columns, "AI_1", _inputHandler, _settings, _gamValidations);
            Player2 = null;
        }

    }
}