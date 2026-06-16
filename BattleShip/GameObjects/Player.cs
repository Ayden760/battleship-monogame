using System.Linq;
using BattleShip.Functions;
using BattleShip.Services;
using BattleShip.GameData;

namespace BattleShip.GameObjects;

using BattleShip.UI;
using System.Collections.Generic;
using System.Data.Common;
using Data = GameData.GameData;

public class Player
{
    public string Name { get; set; }

    protected FieldState[,] _Field;
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
    //virtual so it can be overriten by AI Update
    public virtual void Update(List<ShipBase> shipBases)
    {


        int y = 0;
        int x = 0;
        if (!MadeMove)
        {

            if (InputHandler.CheckFieldClicked(ref y, ref x))
            {
                y -= 1;
                x -= 1;

                var (madeHit, madeMove) = GameValidations.Check_Set_Hit(shipBases, x, y, ref _Field);
                MadeHit = madeHit;
                MadeMove = madeMove;
            }
        }



    }
    public void DrawField(List<ShipBase> shipBases)
    {
        FieldRenderer.DrawField(shipBases, _Field);
    }
    public bool HasWon(List<ShipBase> shipBases)
    {
        return GameValidations.HasWon(shipBases);
    }
}




