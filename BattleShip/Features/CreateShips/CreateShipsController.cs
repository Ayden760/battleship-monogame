using BattleShip.GameObjects;
using BattleShip.Services;

namespace BattleShip.Features.CreateShips;

using Data = GameData.GameData;
public class CreateShipsController
{

    public bool ShouldSwitchToGameScene { get; private set; }
    public void Update()
    {

        ShipSetter.CheckAllSet();

        if (!Data.Session.Player1.ShipsSet)
        {
            Data.Session.CurrentPlayer = Data.Session.Player1;
            Check_User_Input();
        }
        else if (Data.Session.Player2 != null)
        {
            if (!Data.Session.Player2.ShipsSet)
            {
                Data.Session.CurrentPlayer = Data.Session.Player2;
                Check_User_Input();
            }
            else
            {
                ShouldSwitchToGameScene = true;
            }


        }
        else if (Data.Session.Ai != null)
        {
            //create Ai fields
            if (!Data.Session.Ai.ShipsSet)
            {
                AiShipSetter.SetAiShips();
                Data.Session.Ai.ShipsSet = true;
            }
            else
            {
                ShouldSwitchToGameScene = true;
            }

            // set ships for the AI via a function that randomly places all the ships and uses the already made ShipPlacer class
        }


    }
    public void HandleShipClicked(int type)
    {
        ShipSetter.CurrentShip_Length = type;
        ShipSetter.Set_Mode = true;
        ShipSetter.Ship_Selected = false;
    }
    public void ConfirmClicked()
    {
        ShipSetter.Check_Confirm();
    }
    private void Check_User_Input()
    {
        int y = 0;
        int x = 0;
        if (GameController.CheckFieldClicked(ref y, ref x))
        {
            y -= 1;
            x -= 1;
            ShipSetter.Select_CurrentShip(y, x);
        }
        else if (ShipSetter.Ship_Selected)
        {
            ShipSetter.MoveShip();
        }

    }
    public string GetCurrentPlayerText()
    {
        return $"Player {Data.Session.CurrentPlayer.Name}'s Turn";
    }
    public void GenerateShipsClicked()
    {
        ShipSetter.GenerateShipsForCurrentPlayer();

    }

}