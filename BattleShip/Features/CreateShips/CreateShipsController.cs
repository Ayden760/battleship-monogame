
using BattleShip.GameObjects;
using System;
using BattleShip.GameData;
using Microsoft.Xna.Framework.Graphics;
using BattleShip.InputChecker;

using MonoGameLibrary;


namespace BattleShip.Features.CreateShips;

using Data = GameData.GameData;
public class CreateShipsController
{

    public bool ShouldSwitchToGameScene { get; private set; }
    public void Update()
    {

        ShipSetter.CheckAllSet();

        if (Data.Ship.Player1.ShipsSet == false)
        {
            Data.Ship.CurrentPlayer = Data.Ship.Player1;
            Check_User_Input();
        }
        else if (Data.Ship.Player2.ShipsSet == false && Data.Settings.Ai_Mode == false)
        {
            Data.Ship.CurrentPlayer = Data.Ship.Player2;
            Check_User_Input();

        }
        else if (Data.Settings.Ai_Mode)
        {
            //create Ai fields
        }
        else
        {
            ShouldSwitchToGameScene = true;
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
        return $"Player {Data.Ship.CurrentPlayer.Name}'s Turn";
    }

}