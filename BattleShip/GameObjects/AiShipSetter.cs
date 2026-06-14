using System;
using System.Collections.Generic;
using BattleShip.GameData;
using BattleShip.GameObjects;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using BattleShip.Functions;
using BattleShip.InputChecker;
namespace BattleShip.GameObjects;

using BattleShip.Functions;
using Data = GameData.GameData;

public static class AiShipSetter
{

    private static List<int> shipsToPlace = new List<int>();

    public static void InitializeFromSettings(GameSettings settings)
    {


        shipsToPlace.Clear();
        for (int i = 0; i < settings.Five_tile; i++)
        {
            shipsToPlace.Add(5);
        }
        for (int i = 0; i < settings.Four_tile; i++)
        {
            shipsToPlace.Add(4);
        }
        for (int i = 0; i < settings.Three_tile; i++)
        {
            shipsToPlace.Add(3);
        }
        for (int i = 0; i < settings.Two_tile; i++)
        {
            shipsToPlace.Add(2);
        }



    }
    public static List<ShipBase> PlaceAllShipsRandomly()
    {
        int attempts = 0;
        bool CouldNotPlaceAllShips;
        List<ShipBase> placedShips = new List<ShipBase>();


        do
        {
            CouldNotPlaceAllShips = false;

            foreach (int shipLength in shipsToPlace)
            {
                attempts = 0;
                bool placed = false;
                while (!placed)
                {
                    attempts++;

                    int x = Random.Shared.Next(0, 10);
                    int y = Random.Shared.Next(0, 10);

                    ShipBase shipBase = new ShipBase(shipLength);
                    if (ShipPlacer.PlaceShip(y, x, ref shipBase, placedShips, shipLength))
                    {
                        shipBase.IsPlaced = true;
                        placedShips.Add(shipBase);
                        placed = true;
                    }
                    if (attempts > 500)
                    {

                        CouldNotPlaceAllShips = true;
                        break;
                    }
                }
                if (CouldNotPlaceAllShips)
                {
                    placedShips.Clear();
                    break;
                }

            }
        } while (CouldNotPlaceAllShips);
        return placedShips;


        //make it so that normal player can also use this function to set their ships if they want to
    }

    public static void SetAiShips()
    {

        Data.Ship.Ai.Set_Own_Ships(PlaceAllShipsRandomly(), true);


    }
}