using System.Linq;
using BattleShip.Functions;
using BattleShip.InputChecker;
using BattleShip.GameData;
namespace BattleShip.GameObjects;
using System.Collections.Generic;

public class Player
{
    public string Name { get; set; }

    private FieldState[,] _Field;       //Feld wo geschossen wird
    public List<ShipBase> ShipBases { get; set; } = new List<ShipBase>();
    public bool ShipsSet { get; set; } = false;
    public bool MadeMove { get; set; }
    public bool MadeHit { get; set; } = false;


    public Player(int rows, int columns, string name)
    {

        Name = name;

        _Field = new FieldState[rows, columns];


    }


    public void Set_Own_Ships(List<ShipBase> shipBases, bool set)
    {
        // Copies ships to prevent both players from sharing the same instances
        ShipBases = shipBases
            .Select(ship => new ShipBase(ship))
            .ToList();

        ShipsSet = set;
    }
    public void Update(List<ShipBase> shipBases)
    {

        int y = 0;
        int x = 0;
        if (!MadeMove)
        {

            if (GameController.CheckFieldClicked(ref y, ref x))
            {
                y -= 1;
                x -= 1;

                MadeMove = true;
                MadeHit = GameValidations.Check_Set_Hit(shipBases, x, y, ref _Field);
            }
        }



    }
    public void DrawField(List<ShipBase> shipBases)
    {
        int tileSize = 20 * 3;
        int pixelX = 0;
        int pixelY = 0;
        //bei getroffenen Gegnerschiffen darauf achten das Start Middle und End verwendet werden für Später!
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
}




