using System.Collections.Generic;
using BattleShip.GameData;
namespace BattleShip.Functions;

using System;
using System.Linq;

public static class ShipPlacer
{

    public static bool PlaceShip(
    int y,
    int x,
    ref ShipBase shipBase,
    List<ShipBase> shipBases,
    int length)
    {


        // get all directions and shuffel them
        List<Direction> directions = Enum.GetValues(typeof(Direction))
            .Cast<Direction>()
            .OrderBy(_ => Random.Shared.Next())
            .ToList();

        foreach (var dir in directions)
        {
            ShipBase new_base = new ShipBase(length);

            // Prepare cells
            for (int i = 0; i < length; i++)
            {
                new_base.Location.Add(new Cell());
            }

            // Set Positions according to direction
            for (int i = 0; i < length; i++)
            {
                Cell cell = new_base.Location[i];

                switch (dir)
                {
                    case Direction.Up:
                        cell.X = x;
                        cell.Y = y - i;
                        break;

                    case Direction.Down:
                        cell.X = x;
                        cell.Y = y + i;
                        break;

                    case Direction.Left:
                        cell.X = x - i;
                        cell.Y = y;
                        break;

                    case Direction.Right:
                        cell.X = x + i;
                        cell.Y = y;
                        break;
                }

                new_base.Location[i] = cell;
            }

            // check
            if (GameValidations.CanPlaceShip(shipBases, new_base.Location))
            {
                shipBase = new_base;
                return true;

            }
        }

        return false;
    }


}