using System.Collections.Generic;
using System;
using BattleShip.GameData;
namespace BattleShip.Functions;



public class GameValidations
{
    private readonly GameSettings _settings;

    public GameValidations(GameSettings settings)
    {
        _settings = settings;
    }
    public bool CanPlaceShip(
     List<ShipBase> existingShips,
     List<Cell> newShipCells

    )
    {

        int rows = _settings.Rows;
        int columns = _settings.Columns;
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

                    if (_settings.DistanceMode)
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
    public (bool found, int location) IsThereShip(List<ShipBase> existingShips, int x, int y)
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
    public (bool madeHit, bool madeMove) Check_Set_Hit(List<ShipBase> shipBases, int x, int y, ref FieldState[,] enemyField)
    {

        if (!IsInsideField(x, y))
        {

            return (false, false);
        }


        if (enemyField[y, x] == FieldState.Hit ||
            enemyField[y, x] == FieldState.Miss)
        {
            return (false, false);
        }
        bool hit = false;

        foreach (ShipBase shipBase in shipBases)
        {

            if (ShipFinder.Contains(shipBase, x, y))
            {
                hit = true;
                enemyField[y, x] = FieldState.Hit;
                shipBase.Hits++;

                if (_settings.DistanceMode && shipBase.Destroyed)
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
        return (hit, true);

    }
    public void SetSurroundingCellsAsWater(ShipBase shipBase, ref FieldState[,] enemyField)
    {
        foreach (Cell cell in shipBase.Location)
        {
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int posY = cell.Y + y;
                    int posX = cell.X + x;
                    if (posY < 0 || posY >= _settings.Rows || posX < 0 || posX >= _settings.Columns)
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
    public bool HasWon(List<ShipBase> shipBases)
    {
        foreach (ShipBase shipBase in shipBases)
        {
            if (!shipBase.Destroyed)
            {
                return false;
            }

        }
        return true;
    }
    public bool IsInsideField(int x, int y)
    {
        return x >= 0 &&
               x < _settings.Columns &&
               y >= 0 &&
               y < _settings.Rows;



    }
}