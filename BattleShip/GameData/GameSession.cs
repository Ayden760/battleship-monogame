using System;
using System.Linq;
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

    private GameDbContext _dbContext;

    //Game Options

    public GameSession(GameSettings settings, InputHandler handler, GameValidations gameValidations, GameDbContext dbContext)
    {
        _settings = settings;
        _inputHandler = handler;
        _gamValidations = gameValidations;
        _dbContext = dbContext;

    }
    public void InitializeSession()
    {
        Player1 = new Player(_settings.Rows, _settings.Columns, "Test1", _inputHandler, _gamValidations);

        if (!_settings.Ai_Mode)
        {
            Player2 = new Player(_settings.Rows, _settings.Columns, "Test2", _inputHandler, _gamValidations);
            Ai = null;
        }
        else
        {
            Ai = new AI(_settings.Rows, _settings.Columns, "AI_1", _inputHandler, _settings, _gamValidations);
            Player2 = null;
        }


        //for the Player scores

        var mode = _settings.Ai_Mode ? GameMode.AI : GameMode.PvP;
        Player1.InitializeScore(mode, _settings.Number_ShipCells);

        EnsureScoreExists(Player1, mode, _settings.Number_ShipCells);

        if (!_settings.Ai_Mode)
        {
            Player2.InitializeScore(mode, _settings.Number_ShipCells);
            EnsureScoreExists(Player2, mode, _settings.Number_ShipCells);
        }
        /* else
         {
             Ai.InitializeScore(mode, _settings.TotalShips);
             EnsureScoreExists(Ai, db);
         }
 */

    }

    private void EnsureScoreExists(Player player, GameMode mode, int numberShipCells)
    {
        if (player == null || player.Data_Player == null)
        {
            return;
        }

        var existing = _dbContext.Players_Data
            .FirstOrDefault(s => s.PlayerName == player.Data_Player.PlayerName);

        if (existing == null)
        {
            player.Data_Player.ResetEntry(mode, numberShipCells);
            _dbContext.Players_Data.Add(player.Data_Player);
            _dbContext.SaveChanges();
            return;
        }

        existing.ResetEntry(mode, numberShipCells);
        _dbContext.SaveChanges();
        player.Data_Player = existing;
    }
}