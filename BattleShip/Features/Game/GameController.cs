using BattleShip.GameData;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
    private bool _scoreSaved;
    private List<Player_Data> _top5Scores;

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
    public GameController(GameSession session, GameSettings settings, GameDbContext dbContext)
    {

        _session = session;
        _settings = settings;
        _dbContext = dbContext;

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

        _top5Scores = LoadTop5Scores();
    }
    public void Update(GameTime gameTime)
    {
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
            if (!_scoreSaved)
            {
                SaveWinnerScore();
                _scoreSaved = true;
            }

            MatchState = MatchState.GameOver;
            State = GameState.GameOver;
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

    private void SaveWinnerScore()
    {
        var winner = _session.CurrentPlayer;
        if (winner == null || winner.Data_Player == null)
        {
            return;
        }



        Player_Data scoreEntry = null;
        if (winner.Data_Player.Id != 0)
        {
            scoreEntry = _dbContext.Players_Data.FirstOrDefault(s => s.Id == winner.Data_Player.Id);
        }

        if (scoreEntry == null)
        {
            scoreEntry = _dbContext.Players_Data.FirstOrDefault(s => s.PlayerName == winner.Data_Player.PlayerName);
        }

        if (scoreEntry == null)
        {
            scoreEntry = winner.Data_Player;
            _dbContext.Players_Data.Add(scoreEntry);
        }

        scoreEntry.GetOrCreateEntry(winner.CurrentScoreMode, winner.CurrentScoreShipCells);
        _dbContext.SaveChanges();


        winner.Data_Player = scoreEntry;

        // Debug output: dump all score rows currently stored in the database.

        var allScores = _dbContext.Players_Data
     .Include(p => p.Highscores_PvP)
     .Include(p => p.HighScores_PvE)
     .ToList();

        Console.WriteLine("--- Database Dump: Scores ---");
        foreach (var dbScore in allScores)
        {
            Console.WriteLine($"ID: {dbScore.Id} Name: {dbScore.PlayerName}");

            Console.WriteLine("PvP: ");
            foreach (ShipSizeScore shipSizeScore in dbScore.Highscores_PvP)
            {
                Console.WriteLine($" shipCells: {shipSizeScore.Number_ShipCells} Attempts: {shipSizeScore.PlayerAttempts}");
            }
            Console.WriteLine("PvE: ");
            foreach (ShipSizeScore shipSizeScore in dbScore.HighScores_PvE)
            {
                Console.WriteLine($"ShipCells: {shipSizeScore.Number_ShipCells} Attempts: {shipSizeScore.PlayerAttempts}");
            }
            Console.WriteLine("New Player: ");

        }
        Console.WriteLine("--- End Database Dump ---");
    }
    private List<Player_Data> LoadTop5Scores()
    {


        GameMode mode = _settings.Ai_Mode ? GameMode.AI : GameMode.PvP;
        int number_ShipCells = _settings.Number_ShipCells;

        var top5 = _dbContext.Players_Data
            .AsEnumerable()
            .OrderBy(s => s.GetAttempts(mode, number_ShipCells))
            .Take(5)
            .ToList();

        return top5;
    }
}