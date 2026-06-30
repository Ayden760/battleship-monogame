using BattleShip.GameData;
using BattleShip.Functions;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using BattleShip.Services;



namespace BattleShip.UI;

public static class FieldRenderer
{
    public static void DrawField(List<ShipBase> shipBases, FieldState[,] _Field)
    {

        int tileSize = 20 * 3;
        int pixelX = 0;
        int pixelY = 0;
        for (int y = 0; y < _Field.GetLength(0); y++)
        {
            for (int x = 0; x < _Field.GetLength(1); x++)
            {

                if (_Field[y, x] == FieldState.Miss)
                {

                    pixelX = (x + 1) * tileSize;
                    pixelY = (y + 1) * tileSize;

                    ShipDraw.DrawMiss(pixelX, pixelY);
                }
                else if (_Field[y, x] == FieldState.Hit)
                {
                    pixelX = (x + 1) * tileSize;
                    pixelY = (y + 1) * tileSize;

                    ShipBase ship = ShipFinder.FindShipAt(x, y, shipBases);

                    if (ship != null)
                    {
                        int i = ShipFinder.GetSegmentIndex(ship, x, y);

                        if (i == 0)
                        {
                            if (ship.Destroyed)
                                ShipDraw.DrawDestroyedStart(pixelX, pixelY);
                            else
                                ShipDraw.DrawHitStart(pixelX, pixelY);
                        }
                        else if (i == ship.Location.Count - 1)
                        {
                            if (ship.Destroyed)
                                ShipDraw.DrawDrestroyedEnd(pixelX, pixelY);
                            else
                                ShipDraw.DrawHitEnd(pixelX, pixelY);
                        }
                        else
                        {
                            if (ship.Destroyed)
                                ShipDraw.DrawDestroyedMiddle(pixelX, pixelY);
                            else
                                ShipDraw.DrawMiddle(pixelX, pixelY);
                        }
                    }
                }








            }
        }
    }
    public static void DrawShips(List<ShipBase> shipBases)
    {
        int tileSize = 20 * 3;
        foreach (ShipBase shipBase in shipBases)
        {
            foreach (Cell cell in shipBase.Location)
            {
                int pixelX = (cell.X + 1) * tileSize;
                int pixelY = (cell.Y + 1) * tileSize;
                Assets.B_CrossSprite.Draw(Core.SpriteBatch, new Vector2(pixelX, pixelY));

            }
        }
    }
}