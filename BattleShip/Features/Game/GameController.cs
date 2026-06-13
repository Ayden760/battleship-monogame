using BattleShip.GameData;
namespace BattleShip.Features.Game;

using Data = GameData.GameData;
public class GameController
{
    public GameState State { get; private set; }


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
    public void Update()
    {
        if (State == GameState.GameOver)
        {
            return;
        }
        Data.Ship.CurrentPlayer.Update(Data.Ship.OldPlayer.ShipBases);
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
            else if (Data.Ship.CurrentPlayer == Data.Ship.Player2)
            {
                Data.Ship.CurrentPlayer = Data.Ship.Player1;
            }
            else
            {
                //Ai's turn

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