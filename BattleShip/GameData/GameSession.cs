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


        EnsureScoreExists(Player1);

        if (!_settings.Ai_Mode)
        {

            Player2.InitializeScore(mode, _settings.Number_ShipCells);
            EnsureScoreExists(Player2);
        }
        /* else
         {
             Ai.InitializeScore(mode, _settings.TotalShips);
             EnsureScoreExists(Ai, db);
         }
 */

    }

    private void EnsureScoreExists(Player player)
    {
        if (player == null || player.Score == null)
        {
            return;
        }

        var existing = _dbContext.Scores
            .FirstOrDefault(s => s.PlayerName == player.Score.PlayerName && s.Mode == player.Score.Mode);

        if (existing == null)
        {
            _dbContext.Scores.Add(player.Score);
            _dbContext.SaveChanges();
            return;
        }

        player.Score = existing;
    }
}