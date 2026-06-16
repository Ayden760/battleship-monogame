using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using BattleShip.GameData;
using BattleShip.Functions;
using BattleShip.Services;
namespace BattleShip.GameObjects;

using BattleShip.UI;
using Data = GameData.GameData;

public static class ShipSetter
{

    private static List<ShipBase> _shipBases = new List<ShipBase>();

    private static bool AllShipsSet = false;

    private static int Selectionlocation;
    public static bool Ship_Selected { get; set; }



    public static int CurrentShip_Length { get; set; }
    public static bool Set_Mode { get; set; }

    public static int Two_tile { get; private set; }
    public static int Three_tile { get; private set; }
    public static int Four_tile { get; private set; }
    public static int Five_tile { get; private set; }




    public static void CheckAllSet()
    {
        bool allset = true;

        if (Two_tile > 0 || Three_tile > 0 || Four_tile > 0 || Five_tile > 0)
        {
            allset = false;
        }

        if (allset)
            AllShipsSet = true;


    }
    public static void InitializeFromSettings(GameSettings settings)
    {
        _shipBases.Clear();

        AllShipsSet = false;
        Ship_Selected = false;



        Two_tile = settings.Two_tile;
        Three_tile = settings.Three_tile;
        Four_tile = settings.Four_tile;
        Five_tile = settings.Five_tile;

    }
    public static void Reset_And_Set_PlayerField()
    {

        if (Data.Session.CurrentPlayer == Data.Session.Player1)
        {
            Data.Session.Player1.Set_Own_Ships(_shipBases, AllShipsSet);
            InitializeFromSettings(Data.Settings);
        }
        else if (Data.Session.CurrentPlayer == Data.Session.Player2)
        {
            Data.Session.Player2.Set_Own_Ships(_shipBases, AllShipsSet);
        }
    }
    public static void Select_CurrentShip(int y, int x)
    {
        if (Set_Mode)
        {

            //set ships new
            Set_Mode = false;
            ShipBase shipBase = new ShipBase(CurrentShip_Length);
            bool Can_Set = ShipPlacer.PlaceShip(y, x, ref shipBase, _shipBases, CurrentShip_Length);
            if (Can_Set)
            {
                bool NotAlreadySet = true;


                if (CurrentShip_Length == 2 && Two_tile > 0)
                {
                    NotAlreadySet = false;
                    Two_tile--;
                }
                else if (CurrentShip_Length == 3 && Three_tile > 0)
                {
                    NotAlreadySet = false;
                    Three_tile--;
                }
                else if (CurrentShip_Length == 4 && Four_tile > 0)
                {
                    NotAlreadySet = false;
                    Four_tile--;
                }
                else if (CurrentShip_Length == 5 && Five_tile > 0)
                {
                    NotAlreadySet = false;
                    Five_tile--;
                }
                if (!NotAlreadySet)
                {
                    shipBase.IsPlaced = true;
                    _shipBases.Add(shipBase);
                }
            }
            else
            {
                //could not place ship, show message?
            }
        }

        else
        {
            //schon gesetztes Bearbeiten
            var (found, location) = GameValidations.IsThereShip(_shipBases, x, y);


            if (found)
            {
                Selectionlocation = location;
                Ship_Selected = found;

                MoveShip();
            }


        }
    }

    public static void DrawShips()
    {
        FieldRenderer.DrawShips(_shipBases);
    }
    public static void MoveShip()
    {

        List<Cell> newCells = new List<Cell>();
        List<ShipBase> newShipBase = new List<ShipBase>(_shipBases);
        newShipBase.RemoveAt(Selectionlocation);

        if (GameController.MoveUp())
        {

            newCells = ShipMover.MoveUp(_shipBases[Selectionlocation].Location);
        }
        if (GameController.MoveDown())
        {
            newCells = ShipMover.MoveDown(_shipBases[Selectionlocation].Location);
        }
        if (GameController.MoveLeft())
        {
            newCells = ShipMover.MoveLeft(_shipBases[Selectionlocation].Location);
        }
        if (GameController.MoveRight())
        {
            newCells = ShipMover.MoveRight(_shipBases[Selectionlocation].Location);
        }
        if (GameController.RotateShip())
        {
            newCells = ShipMover.Rotate(_shipBases[Selectionlocation].Location);
        }
        if (newCells.Count > 0)
        {
            if (GameValidations.CanPlaceShip(newShipBase, newCells))
            {
                _shipBases[Selectionlocation].Location = newCells;

            }
        }

    }
    public static void Check_Confirm()
    {
        if (AllShipsSet)
        {

            Reset_And_Set_PlayerField();
        }
    }

    public static void GenerateShipsForCurrentPlayer()
    {
        _shipBases.Clear();
        _shipBases = AiShipSetter.PlaceAllShipsRandomly();
        Two_tile = 0;
        Three_tile = 0;
        Four_tile = 0;
        Five_tile = 0;

    }





}
