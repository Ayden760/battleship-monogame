using System.Collections.Generic;
using System;
using BattleShip.GameData;
namespace BattleShip.Functions;
using Data = GameData.GameData;

public static class GameValidations
{
    public static bool CanPlaceShip(
     List<ShipBase> existingShips,
     List<Cell> newShipCells

    )
    {

        int rows = Data.Settings.Rows;
        int columns = Data.Settings.Columns;
        // check gamefield borders
        foreach (var cell in newShipCells)
        {
            if (cell.X < 0 || cell.X >= columns ||
            cell.Y < 0 || cell.Y >= rows)
            {
                return false;
            }
        }


        foreach (var ship in existingShips)
        {
            foreach (var existingCell in ship.Location)
            {
                foreach (var newCell in newShipCells)
                {
                    //checks every new Cell against all already existing cells


                    if (existingCell.X == newCell.X &&
                        existingCell.Y == newCell.Y)
                    {

                        return false;
                    }

                    if (Data.Settings.DistanceMode)
                    {
                        int dx = Math.Abs(existingCell.X - newCell.X);
                        int dy = Math.Abs(existingCell.Y - newCell.Y);

                        if (dx <= 1 && dy <= 1)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }
    public static (bool found, int location) IsThereShip(List<ShipBase> existingShips, int y, int x)
    {

        for (int i = 0; i < existingShips.Count; i++)
        {
            foreach (var cell in existingShips[i].Location)
            {
                if (cell.Y == y && cell.X == x)
                {
                    return (true, i);
                }
            }
        }
        return (false, -1);
    }
    public static bool Check_Set_Hit(List<ShipBase> shipBases, int x, int y, ref FieldState[,] enemyField)
    {
        if (enemyField[y, x] == FieldState.Hit ||
            enemyField[y, x] == FieldState.Miss)
        {
            return false;
        }

        bool hit = false;

        foreach (ShipBase shipBase in shipBases)
        {

            if (ShipFinder.Contains(shipBase, x, y))
            {
                hit = true;
                enemyField[y, x] = FieldState.Hit;
                shipBase.Hits++;

                if (Data.Settings.DistanceMode && shipBase.Destroyed)
                {
                    SetSurroundingCellsAsWater(shipBase, ref enemyField);
                }
                break;
            }
        }

        if (!hit)
        {
            enemyField[y, x] = FieldState.Miss;
        }
        return hit;

    }
    public static void SetSurroundingCellsAsWater(ShipBase shipBase, ref FieldState[,] enemyField)
    {
        foreach (Cell cell in shipBase.Location)
        {
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int posY = cell.Y + y;
                    int posX = cell.X + x;
                    if (posY < 0 || posY >= Data.Settings.Rows || posX < 0 || posX >= Data.Settings.Columns)
                    {
                        continue;
                    }
                    if (enemyField[posY, posX] != FieldState.Miss && enemyField[posY, posX] != FieldState.Hit)
                    {
                        enemyField[posY, posX] = FieldState.Miss;
                    }
                }
            }
        }

    }
}