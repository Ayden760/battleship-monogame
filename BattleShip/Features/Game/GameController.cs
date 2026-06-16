using BattleShip.GameData;
using Microsoft.Xna.Framework;

namespace BattleShip.Features.Game;

using Data = GameData.GameData;
public class GameController
{
    public GameState State { get; private set; }

    private const double TurnDelaySeconds = 2.0;
    private double _turnDelayTimer;
    private bool _turnDelayActive;

    public string GetCurrentPlayerText()
    {
        return $"Player {Data.Session.CurrentPlayer.Name}'s Turn";
    }
    public void Initialize()
    {
        Data.Session.CurrentPlayer = Data.Session.Player1;
        if (Data.Settings.Ai_Mode)
        {
            Data.Session.OldPlayer = Data.Session.Ai;
        }
        else
        {
            Data.Session.OldPlayer = Data.Session.Player2;
        }
        State = GameState.Playing;
    }
    public void Update(GameTime gameTime)
    {
        if (State == GameState.GameOver)
        {
            return;
        }

        Data.Session.CurrentPlayer.Update(Data.Session.OldPlayer.ShipBases);

        if (Data.Session.CurrentPlayer.MadeMove)
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
        if (!Data.Session.CurrentPlayer.MadeMove)
        {
            return;
        }
        if (Data.Session.CurrentPlayer == Data.Session.Ai)
        {
            return;
        }

        _turnDelayActive = true;
        _turnDelayTimer = TurnDelaySeconds;
    }

    public void HandleTurn()
    {

        if (Data.Session.CurrentPlayer.MadeMove)
        {

            Data.Session.CurrentPlayer.MadeMove = false;
            if (Data.Session.CurrentPlayer.MadeHit && Data.Settings.BonusShotOnHit)
            {
                return;
            }
            Data.Session.OldPlayer = Data.Session.CurrentPlayer;

            if (Data.Session.CurrentPlayer == Data.Session.Player1)
            {



                if (!Data.Settings.Ai_Mode)
                {
                    Data.Session.CurrentPlayer = Data.Session.Player2;
                }
                else
                {
                    Data.Session.CurrentPlayer = Data.Session.Ai;
                    //set to AI
                }


            }
            else
            {
                Data.Session.CurrentPlayer = Data.Session.Player1;
            }

        }
    }
    public void CheckWin()
    {
        if (Data.Session.CurrentPlayer.HasWon(Data.Session.OldPlayer.ShipBases))
        {
            State = GameState.GameOver;
        }
    }

}