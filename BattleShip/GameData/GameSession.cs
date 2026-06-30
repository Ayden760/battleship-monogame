using System;
using System.Linq;
using BattleShip.Functions;
using BattleShip.GameObjects;
using BattleShip.Services;
namespace BattleShip.GameData;

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


    public void InitializeSession(string player1Name, string player2Name)
    {
        player1Name = NormalizeName(player1Name, "NamelessPlayer1");
        player2Name = NormalizeName(player2Name, "NamelessPlayer2");

        FallBackOnSame(ref player1Name, ref player2Name);
        CurrentMatch = new Match
        {
            MatchSetTime = DateTime.UtcNow,
            GameStartTime = DateTime.UtcNow,
            GameEndTime = DateTime.UtcNow,
            Aborted = true,
            ModePlayer = _settings.Ai_Mode ? GameMode.AI : GameMode.PvP,
            DistanceMode = _settings.DistanceMode,
            BonusShotOnHit = _settings.BonusShotOnHit,
            AiDifficulty = _settings.Difficulty

        };

        _dbContext.Matches.Add(CurrentMatch);
        _dbContext.SaveChanges();

        Player1 = new Player(_settings.Rows, _settings.Columns, player1Name, _inputHandler, _gamValidations);
        Player1Data = GetOrCreatePlayerData(Player1.Name, isAi: false);

        if (!_settings.Ai_Mode)
        {
            Player2 = new Player(_settings.Rows, _settings.Columns, player2Name, _inputHandler, _gamValidations);
            Player2Data = GetOrCreatePlayerData(Player2.Name, isAi: false);
            AiData = null;
            Ai = null;
        }
        else
        {
            Ai = new AI(_settings.Rows, _settings.Columns, "AI_1", _inputHandler, _settings, _gamValidations);
            AiData = GetOrCreatePlayerData(Ai.Name, isAi: true);
            Player2Data = null;
            Player2 = null;
        }

        _dbContext.SaveChanges();

    }

    private Player_Data GetOrCreatePlayerData(string playerName, bool isAi)
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
            };

            _dbContext.Players_Data.Add(playerData);
            return playerData;
        }

        playerData.IsAI = isAi;
        return playerData;
    }
    private string NormalizeName(string name, string fallback)
    {
        if (string.IsNullOrWhiteSpace(name))
            return fallback;


        return name.Trim();
    }
    private void FallBackOnSame(ref string name1, ref string name2)
    {
        if (name1 == name2)
        {
            name1 = "emptyP1";
            name2 = "emptyP2";
        }
    }


}