using BattleShip.GameData;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BattleShip.Features.Game;


public class GameController
{
    public GameState State { get; private set; }
    public MatchState MatchState { get; private set; }
    private readonly GameSettings _settings;
    private GameSession _session;
    private bool _MatchPlayerSaved;
    private const double TurnDelaySeconds = 2.0;

    private double _turnDelayTimer;
    private bool _turnDelayActive;

    //AI
    private const double AiMinThinkDelaySeconds = 0.5;
    private const double AiMaxThinkDelaySeconds = 1.0;
    private readonly Random _random = new();
    private bool _isWaitingForAiMove;
    private bool _aiReadyToAct;
    private double _aiThinkDelayTimer;
    private double _aiThinkDelayTarget;
    private GameDbContext _dbContext;

    public string ConfigText = "Test";
    public string CurrentPlayerStatsText;

    public List<string> Highscores;
    public GameController(GameSession session, GameSettings settings, GameDbContext dbContext)
    {

        _session = session;
        _settings = settings;
        _dbContext = dbContext;
        ConfigText = $"Mode: {(_settings.Ai_Mode ? $"AI\nDiff: {_settings.Difficulty}" : "Player")}\nDIST Mode: {(_settings.DistanceMode ? "Yes" : "No")}\nBonusshot: {(_settings.BonusShotOnHit ? "Yes" : "No")}\nNumCells : {_settings.Number_ShipCells}";

    }
    public string GetCurrentPlayerText()
    {
        if (MatchState == MatchState.AiThinking)
        {
            return "AI is thinking...";
        }

        return $"Player {_session.CurrentPlayer.Name}'s Turn";
    }
    public void Initialize()
    {

        LoadHighscores();
        _session.CurrentMatch.GameStartTime = DateTime.UtcNow;
        _session.CurrentPlayer = _session.Player1;
        if (_settings.Ai_Mode)
        {
            _session.OldPlayer = _session.Ai;
        }
        else
        {
            _session.OldPlayer = _session.Player2;
        }
        State = GameState.Playing;
        MatchState = MatchState.PlayerTurn;


    }
    public void Update(GameTime gameTime)
    {
        UpdateActivePlayerTime(gameTime);

        switch (MatchState)
        {
            case MatchState.GameOver:
                return;
            case MatchState.PlayerTurn:
                HandlePlayerTurn(gameTime);
                break;
            case MatchState.AiThinking:
                HandleAiThinking(gameTime);
                break;
            case MatchState.TurnTransition:
                HandleTurnTransition(gameTime);
                break;
        }

        if (CheckWin())
        {
            if (!_MatchPlayerSaved)
            {
                SaveMatchPlayer();

                _MatchPlayerSaved = true;
            }

            MatchState = MatchState.GameOver;
            State = GameState.GameOver;
        }
        UpdateCurrentPlayerText();



    }

    private void UpdateActivePlayerTime(GameTime gameTime)
    {
        if (MatchState == MatchState.GameOver || MatchState == MatchState.TurnTransition)
        {
            return;
        }

        var delta = gameTime.ElapsedGameTime;

        if (_session.CurrentPlayer == _session.Player1)
        {
            _session.Player1.Time += delta;
            return;
        }

        if (!_settings.Ai_Mode && _session.CurrentPlayer == _session.Player2)
        {
            _session.Player2.Time += delta;
            return;
        }

        if (_settings.Ai_Mode && _session.CurrentPlayer == _session.Ai)
        {
            _session.Ai.Time += delta;
        }
    }
    public void TriggerTurnDelay()
    {

        //checks if player did his Move before pressing Continue Button
        if (!_session.CurrentPlayer.MadeMove)
        {
            return;
        }
        if (_session.CurrentPlayer == _session.Ai)
        {
            return;
        }

        _turnDelayActive = true;
        _turnDelayTimer = TurnDelaySeconds;
        MatchState = MatchState.TurnTransition;
    }

    private void HandlePlayerTurn(GameTime gameTime)
    {
        if (_session.CurrentPlayer == _session.Ai)
        {
            if (!_aiReadyToAct)
            {
                StartAiThinking();
                MatchState = MatchState.AiThinking;
                return;
            }

            _aiReadyToAct = false;
            _isWaitingForAiMove = false;
            _aiThinkDelayTimer = 0;
            _aiThinkDelayTarget = 0;
        }

        _session.CurrentPlayer.Update(_session.OldPlayer.ShipBases);

        if (_session.CurrentPlayer.MadeMove)
        {
            MatchState = MatchState.TurnTransition;
            HandleTurnTransition(gameTime);
            return;
        }

        _turnDelayActive = false;
        _turnDelayTimer = 0;
    }

    private void StartAiThinking()
    {
        _aiReadyToAct = false;

        if (_isWaitingForAiMove)
        {
            return;
        }

        _isWaitingForAiMove = true;
        _aiThinkDelayTimer = 0;
        _aiThinkDelayTarget = AiMinThinkDelaySeconds + (_random.NextDouble() * (AiMaxThinkDelaySeconds - AiMinThinkDelaySeconds));
    }

    private void HandleAiThinking(GameTime gameTime)
    {
        if (_session.CurrentPlayer != _session.Ai)
        {
            _isWaitingForAiMove = false;
            _aiThinkDelayTimer = 0;
            _aiThinkDelayTarget = 0;
            MatchState = MatchState.PlayerTurn;
            return;
        }

        StartAiThinking();

        _aiThinkDelayTimer += gameTime.ElapsedGameTime.TotalSeconds;
        if (_aiThinkDelayTimer < _aiThinkDelayTarget)
        {
            return;
        }

        _isWaitingForAiMove = false;
        _aiThinkDelayTimer = 0;
        _aiThinkDelayTarget = 0;
        _aiReadyToAct = true;
        MatchState = MatchState.PlayerTurn;
    }

    private void HandleTurnTransition(GameTime gameTime)
    {
        if (!_session.CurrentPlayer.MadeMove)
        {
            _turnDelayActive = false;
            _turnDelayTimer = 0;
            MatchState = MatchState.PlayerTurn;
            return;
        }

        //starts the delay timer
        if (!_turnDelayActive)
        {
            _turnDelayActive = true;
            _turnDelayTimer = 0;
        }

        _turnDelayTimer += gameTime.ElapsedGameTime.TotalSeconds;


        //switches to second player if true
        if (_turnDelayTimer >= TurnDelaySeconds)
        {
            HandleTurn();
            _turnDelayActive = false;
            _turnDelayTimer = 0;

            if (MatchState != MatchState.GameOver)
            {
                MatchState = MatchState.PlayerTurn;
            }
        }
    }

    public void HandleTurn()
    {

        if (_session.CurrentPlayer.MadeMove)
        {
            _session.CurrentPlayer.MadeMove = false;
            if (_session.CurrentPlayer.MadeHit && _settings.BonusShotOnHit)
            {
                return;
            }
            _session.OldPlayer = _session.CurrentPlayer;

            if (_session.CurrentPlayer == _session.Player1)
            {
                if (!_settings.Ai_Mode)
                {
                    _session.CurrentPlayer = _session.Player2;
                }
                else
                {
                    _session.CurrentPlayer = _session.Ai;
                }
            }
            else
            {
                _session.CurrentPlayer = _session.Player1;
            }
        }
    }
    public bool CheckWin()
    {
        if (_session.CurrentPlayer.HasWon(_session.OldPlayer.ShipBases))
        {
            return true;
        }
        return false;
    }

    private void SaveMatchPlayer()
    {
        if (_session.CurrentMatch == null)
        {
            return;
        }

        _session.CurrentMatch.GameEndTime = DateTime.UtcNow;
        _session.CurrentMatch.Aborted = false;

        var winnerName = _session.CurrentPlayer.Name;

        if (_session.Player1Data != null)
        {
            _dbContext.MatchPlayers.Add(new MatchPlayer
            {
                DataPlayerId = _session.Player1Data.Id,
                MatchDataId = _session.CurrentMatch.Id,
                PlayerAttempts = _session.Player1.Attempts,
                NumberShipCells = _session.Player1.ShipBases.Sum(s => s.Length),
                PlayersTime = _session.Player1.Time,
                HasWon = _session.Player1.Name == winnerName
            });
        }

        if (_session.Player2Data != null && _session.Player2 != null)
        {
            _dbContext.MatchPlayers.Add(new MatchPlayer
            {
                DataPlayerId = _session.Player2Data.Id,
                MatchDataId = _session.CurrentMatch.Id,
                PlayerAttempts = _session.Player2.Attempts,
                NumberShipCells = _session.Player2.ShipBases.Sum(s => s.Length),
                PlayersTime = _session.Player2.Time,
                HasWon = _session.Player2.Name == winnerName
            });
        }

        if (_session.AiData != null && _session.Ai != null)
        {
            _dbContext.MatchPlayers.Add(new MatchPlayer
            {
                DataPlayerId = _session.AiData.Id,
                MatchDataId = _session.CurrentMatch.Id,
                PlayerAttempts = _session.Ai.Attempts,
                NumberShipCells = _session.Ai.ShipBases.Sum(s => s.Length),
                PlayersTime = _session.Ai.Time,
                HasWon = _session.Ai.Name == winnerName
            });
        }

        _dbContext.SaveChanges();
    }
    public void UpdateCurrentPlayerText()
    {
        CurrentPlayerStatsText = $"CurrentPlayer Stats\nAttempts: {_session.CurrentPlayer.Attempts}\nTime: {_session.CurrentPlayer.Time.TotalSeconds.ToString("F2", CultureInfo.InvariantCulture)}s";
    }
    public void LoadHighscores()
    {
        var currentMode = _settings.Ai_Mode ? GameMode.AI : GameMode.PvP;

        var query = _dbContext.MatchPlayers
            .AsNoTracking()
            .Where(mp => mp.HasWon)
            .Where(mp => mp.MatchData != null)
            .Where(mp => mp.MatchData.ModePlayer == currentMode)
            .Where(mp => mp.MatchData.DistanceMode == _settings.DistanceMode)
            .Where(mp => mp.MatchData.BonusShotOnHit == _settings.BonusShotOnHit)
            .Where(mp => mp.NumberShipCells == _settings.Number_ShipCells);


        if (_settings.Ai_Mode)
        {
            query = query.Where(mp => mp.MatchData.AiDifficulty == _settings.Difficulty);
        }

        Highscores = query
            .OrderBy(mp => mp.PlayerAttempts)
            .ThenBy(mp => mp.Id)


            //gets only relevant Data
            .Select(mp => new
            {
                Name = mp.DataPlayer != null && !string.IsNullOrWhiteSpace(mp.DataPlayer.PlayerName)
                    ? mp.DataPlayer.PlayerName
                    : "Unknown",
                Attempts = mp.PlayerAttempts,
                PlayerTimeSeconds = mp.PlayersTime.TotalSeconds,
                mp.Id
            })
            //changes to c#
            .AsEnumerable()
            .OrderBy(x => x.Attempts)
            .ThenBy(x => x.PlayerTimeSeconds)
            .ThenBy(x => x.Id)
            .Select(x =>
                $"{x.Name}: {x.Attempts} attempts    Time: {Math.Round(x.PlayerTimeSeconds, 2).ToString("F2", CultureInfo.InvariantCulture)}s")
            .Take(5)
            .ToList();

        if (Highscores.Count == 0)
        {
            Highscores.Add("No highscores for current settings.");
        }
    }

}