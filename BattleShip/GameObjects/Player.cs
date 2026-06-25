using System.Linq;
using BattleShip.Functions;
using BattleShip.Services;
using BattleShip.GameData;

namespace BattleShip.GameObjects;

using BattleShip.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;

public class Player
{
    public string Name { get; set; }
    public int Attemps { get; protected set; } = 0;
    protected FieldState[,] _Field;
    public List<ShipBase> ShipBases { get; set; } = new List<ShipBase>();
    public bool ShipsSet { get; set; } = false;
    public bool MadeMove { get; set; }
    public bool MadeHit { get; set; } = false;

    private readonly InputHandler _inputHandler;
    private GameValidations _validations;
    public Player(int rows, int columns, string name, InputHandler handler, GameValidations validations)
    {

        Name = name;

        _Field = new FieldState[rows, columns];

        _inputHandler = handler;
        _validations = validations;
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

            if (_inputHandler.CheckFieldClicked(ref y, ref x))
            {
                y -= 1;
                x -= 1;

                var (madeHit, madeMove) = _validations.Check_Set_Hit(shipBases, x, y, ref _Field);
                MadeHit = madeHit;
                if (madeMove)
                {
                    Attemps++;
                }
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
        return _validations.HasWon(shipBases);
    }
}




