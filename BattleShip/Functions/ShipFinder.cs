using System.Collections.Generic;
namespace BattleShip.Functions;

public static class ShipFinder
{


    public static ShipBase FindShipAt(int x, int y, List<ShipBase> ships)
    {
        foreach (var ship in ships)
        {

            if (Contains(ship, x, y))
                return ship;
        }

        return null;
    }



    public static int GetSegmentIndex(ShipBase ship, int x, int y)
    {
        for (int i = 0; i < ship.Location.Count; i++)
        {
            if (ship.Location[i].X == x && ship.Location[i].Y == y)
                return i;
        }

        return -1;
    }

    public static bool Contains(ShipBase ship, int x, int y)
    {
        for (int i = 0; i < ship.Location.Count; i++)
        {
            if (ship.Location[i].X == x && ship.Location[i].Y == y)
                return true;
        }

        return false;
    }
}