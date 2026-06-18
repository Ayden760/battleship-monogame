using BattleShip.GameData;
using Microsoft.Xna.Framework;
using System;

namespace BattleShip.Features.Game;


public class GameController
{
    public GameState State { get; private set; }
    public MatchState MatchState { get; private set; }
    private readonly GameSettings _settings;
    private GameSession _session;

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
    public GameController(GameSession session, GameSettings settings)
    {
        _session = session;
        _settings = settings;
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
}