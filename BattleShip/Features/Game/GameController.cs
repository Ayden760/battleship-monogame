using BattleShip.GameData;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShip.Features.Game;


public class GameController
{
    public GameState State { get; private set; }
    public MatchState MatchState { get; private set; }
    private readonly GameSettings _settings;
    private GameSession _session;
    private bool _scoreSaved;
    private List<Score> _top5Scores;

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
        Console.WriteLine(_session.CurrentPlayer.Score.Number_ShipCells);
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

        foreach (Score score in _top5Scores)
        {
            Console.WriteLine($"ID: {score.Id} Name: {score.PlayerName} Attemps: {score.PlayerAttempts} Number Ships {score.Number_ShipCells} GameMode: {score.Mode}");
        }
        Console.WriteLine($"Current Score: ID: {_session.CurrentPlayer.Score.Id} Name: {_session.CurrentPlayer.Score.PlayerName} Attemps: {_session.CurrentPlayer.Score.PlayerAttempts} Number ShipCells {_session.CurrentPlayer.Score.Number_ShipCells} GameMode: {_session.CurrentPlayer.Score.Mode}");
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
        if (winner == null || winner.Score == null)
        {
            return;
        }



        Score scoreEntry = null;
        if (winner.Score.Id != 0)
        {
            //search for the existing ID
            scoreEntry = _dbContext.Scores.FirstOrDefault(s => s.Id == winner.Score.Id);
        }

        if (scoreEntry == null)
        {
            scoreEntry = _dbContext.Scores.FirstOrDefault(s => s.PlayerName == winner.Score.PlayerName && s.Mode == winner.Score.Mode);
        }

        if (scoreEntry == null)
        {
            scoreEntry = winner.Score;
            _dbContext.Scores.Add(scoreEntry);
        }

        //scoreEntry.PlayerScore += 1;
        //scoreEntry.Number_Ships = winner.Score.Number_Ships;
        _dbContext.SaveChanges();

        winner.Score = scoreEntry;
    }
    private List<Score> LoadTop5Scores()
    {


        GameMode mode = _settings.Ai_Mode ? GameMode.AI : GameMode.PvP;
        int number_ShipCells = _settings.Number_ShipCells;

        var top5 = _dbContext.Scores
            .Where(s => s.Mode == mode && s.Number_ShipCells == number_ShipCells)
            .OrderBy(s => s.PlayerAttempts)
            .Take(5)
            .ToList();

        return top5;
    }
}