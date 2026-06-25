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

    private readonly GameDbContext _dbContext;
    public Match CurrentMatch { get; private set; }
    public Player_Data Player1Data { get; private set; }
    public Player_Data Player2Data { get; private set; }
    public Player_Data AiData { get; private set; }

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
        CurrentMatch = new Match
        {
            MatchSetTime = DateTime.UtcNow,
            GameStartTime = DateTime.UtcNow,
            GameEndTime = DateTime.UtcNow,
            Aborted = true,
            Mode = _settings.Ai_Mode ? GameMode.AI : GameMode.PvP,
            DistanceMode = _settings.DistanceMode,
            BonusShotOnHit = _settings.BonusShotOnHit

        };

        _dbContext.Matches.Add(CurrentMatch);
        _dbContext.SaveChanges();

        Player1 = new Player(_settings.Rows, _settings.Columns, "Test1", _inputHandler, _gamValidations);
        Player1Data = GetOrCreatePlayerData(Player1.Name, isAi: false, aiDifficulty: 0);

        if (!_settings.Ai_Mode)
        {
            Player2 = new Player(_settings.Rows, _settings.Columns, "Test2", _inputHandler, _gamValidations);
            Player2Data = GetOrCreatePlayerData(Player2.Name, isAi: false, aiDifficulty: 0);
            AiData = null;
            Ai = null;
        }
        else
        {
            Ai = new AI(_settings.Rows, _settings.Columns, "AI_1", _inputHandler, _settings, _gamValidations);
            AiData = GetOrCreatePlayerData(Ai.Name, isAi: true, aiDifficulty: _settings.Difficulty);
            Player2Data = null;
            Player2 = null;
        }

        _dbContext.SaveChanges();

    }

    private Player_Data GetOrCreatePlayerData(string playerName, bool isAi, int aiDifficulty)
    {
        //makes every letter lowercase so for example this : Test1, teSt1 is the same Player
        var normalizedName = playerName.Trim().ToLowerInvariant();
        var playerData = _dbContext.Players_Data
            .FirstOrDefault(p => p.PlayerName != null && p.PlayerName.Trim().ToLower() == normalizedName);

        if (playerData == null)
        {
            playerData = new Player_Data
            {
                PlayerName = playerName.Trim(),
                IsAI = isAi,
                AiDifficulty = aiDifficulty,
                HasWon = false
            };

            _dbContext.Players_Data.Add(playerData);
            return playerData;
        }

        playerData.IsAI = isAi;
        playerData.AiDifficulty = aiDifficulty;

        return playerData;
    }


}