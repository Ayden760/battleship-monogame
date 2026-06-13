using System.Linq;
using System;
using BattleShip.Functions;
using BattleShip.InputChecker;
using BattleShip.GameData;
using Microsoft.Xna.Framework;
using MonoGameLibrary;


namespace BattleShip.GameObjects;

using System.Collections.Generic;

public class AI : Player
{

    public AI(int rows, int columns, string name)
    : base(rows, columns, name)
    {
        ShipBases = new List<ShipBase>();
    }
    public void Set_Own_Ships(List<ShipBase> shipBases)
    {

        ShipBases = shipBases
            .Select(ship => new ShipBase(ship))
            .ToList();

        ShipsSet = true;
    }

    //for testing
    public void DrawShips()
    {
        int tileSize = 20 * 3;

        foreach (ShipBase shipBase in ShipBases)
        {
            foreach (Cell cell in shipBase.Location)
            {
                //int pixelX = cell.X * tileSize;
                //int pixelY = cell.Y * tileSize;
                int pixelX = (cell.X + 1) * tileSize;
                int pixelY = (cell.Y + 1) * tileSize;
                Assets.B_CrossSprite.Draw(Core.SpriteBatch, new Vector2(pixelX, pixelY));

            }
        }
    }
}