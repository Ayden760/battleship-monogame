using BattleShip.GameData;
using Microsoft.Xna.Framework;

namespace BattleShip.Features.Game;


public class GameController
{
    public GameState State { get; private set; }
    private readonly GameSettings _settings;
    private GameSession _session;

    private const double TurnDelaySeconds = 2.0;
    private double _turnDelayTimer;
    private bool _turnDelayActive;
    public GameController(GameSession session, GameSettings settings)
    {
        _session = session;
        _settings = settings;
    }
    public string GetCurrentPlayerText()
    {
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
    }
    public void Update(GameTime gameTime)
    {
        if (State == GameState.GameOver)
        {
            return;
        }

        _session.CurrentPlayer.Update(_session.OldPlayer.ShipBases);

        if (_session.CurrentPlayer.MadeMove)
        {
            if (!_turnDelayActive)
            {
                _turnDelayActive = true;
                _turnDelayTimer = 0;
            }

            _turnDelayTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (_turnDelayTimer >= TurnDelaySeconds)
            {
                HandleTurn();
                _turnDelayActive = false;
                _turnDelayTimer = 0;
            }
        }
        else
        {
            _turnDelayActive = false;
            _turnDelayTimer = 0;
        }

        CheckWin();
    }
    public void TriggerTurnDelay()
    {
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
                    //set to AI
                }


            }
            else
            {
                _session.CurrentPlayer = _session.Player1;
            }

        }
    }
    public void CheckWin()
    {
        if (_session.CurrentPlayer.HasWon(_session.OldPlayer.ShipBases))
        {
            State = GameState.GameOver;
        }
    }

}