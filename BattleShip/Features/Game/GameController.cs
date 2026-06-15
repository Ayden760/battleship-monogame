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
        return $"Player {Data.Ship.CurrentPlayer.Name}'s Turn";
    }
    public void Initialize()
    {
        Data.Ship.CurrentPlayer = Data.Ship.Player1;
        if (Data.Settings.Ai_Mode)
        {
            Data.Ship.OldPlayer = Data.Ship.Ai;
        }
        else
        {
            Data.Ship.OldPlayer = Data.Ship.Player2;
        }
        State = GameState.Playing;
    }
    public void Update(GameTime gameTime)
    {
        if (State == GameState.GameOver)
        {
            return;
        }

        Data.Ship.CurrentPlayer.Update(Data.Ship.OldPlayer.ShipBases);

        if (Data.Ship.CurrentPlayer.MadeMove)
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
    public void HandleTurn()
    {
        if (Data.Ship.CurrentPlayer.MadeMove)
        {
            Data.Ship.CurrentPlayer.MadeMove = false;
            if (Data.Ship.CurrentPlayer.MadeHit && Data.Settings.BonusShotOnHit)
            {
                return;
            }
            Data.Ship.OldPlayer = Data.Ship.CurrentPlayer;

            if (Data.Ship.CurrentPlayer == Data.Ship.Player1)
            {



                if (!Data.Settings.Ai_Mode)
                {
                    Data.Ship.CurrentPlayer = Data.Ship.Player2;
                }
                else
                {
                    Data.Ship.CurrentPlayer = Data.Ship.Ai;
                    //set to AI
                }


            }
            else
            {
                Data.Ship.CurrentPlayer = Data.Ship.Player1;
            }

        }
    }
    public void CheckWin()
    {
        if (Data.Ship.CurrentPlayer.HasWon(Data.Ship.OldPlayer.ShipBases))
        {
            State = GameState.GameOver;
        }
    }

}