using System.Linq;
using System;
using BattleShip.Functions;
using BattleShip.InputChecker;
using BattleShip.GameData;
using Microsoft.Xna.Framework;
using MonoGameLibrary;


namespace BattleShip.GameObjects;

using BattleShip.UI;
using System.Collections.Generic;

public class AI : Player
{


    private Direction HitDirection = Direction.None;
    private bool foundDirection = false;
    private int x;
    private int y;
    private bool foundShip = false;
    private bool lastMoveWasHit = false;


    private List<Direction> availableDirections = new()
{
    Direction.Up,
    Direction.Down,
    Direction.Left,
    Direction.Right
};

    public AI(int rows, int columns, string name)
    : base(rows, columns, name)
    {


    }

    public void AIUpdate(List<ShipBase> shipBases)
    {

        if (!MadeMove)
        {



            if (!foundShip)
            {
                x = Random.Shared.Next(0, 10);
                y = Random.Shared.Next(0, 10);

                var (madeHit, madeMove) = GameValidations.Check_Set_Hit(shipBases, x, y, ref _Field);
                MadeHit = madeHit;
                foundShip = madeHit;
                MadeMove = madeMove;

            }
            else
            {

                if (!foundDirection)
                {
                    int index = Random.Shared.Next(availableDirections.Count);
                    HitDirection = availableDirections[index];
                    availableDirections.RemoveAt(index);
                    HitDirection = (Direction)Random.Shared.Next(1, 5);
                    if (HitDirection == Direction.Up)
                    {
                        y -= 1;
                    }
                    else if (HitDirection == Direction.Down)
                    {
                        y += 1;
                    }
                    else if (HitDirection == Direction.Left)
                    {
                        x -= 1;
                    }
                    else if (HitDirection == Direction.Right)
                    {
                        x += 1;
                    }
                    var (madeHit, madeMove) = GameValidations.Check_Set_Hit(shipBases, x, y, ref _Field);
                    MadeHit = madeHit;
                    MadeMove = madeMove;
                    if (madeHit)
                    {
                        //reset directions for new ship
                        availableDirections = new()
                        {
                            Direction.Up,
                            Direction.Down,
                            Direction.Left,
                            Direction.Right
                        };
                        foundDirection = true;
                        lastMoveWasHit = true;
                    }


                }
                else if (lastMoveWasHit)
                {
                    if (HitDirection == Direction.Up)
                    {
                        y -= 1;
                    }
                    else if (HitDirection == Direction.Down)
                    {
                        y += 1;
                    }
                    else if (HitDirection == Direction.Left)
                    {
                        x -= 1;
                    }
                    else if (HitDirection == Direction.Right)
                    {
                        x += 1;
                    }
                    var (madeHit, madeMove) = GameValidations.Check_Set_Hit(shipBases, x, y, ref _Field);
                    MadeHit = madeHit;
                    MadeMove = madeMove;
                    if (!madeHit)
                    {
                        lastMoveWasHit = false;
                    }
                    // Implement logic to continue in the found direction
                }
                else
                {






                }
            }


        }
        if ()
        {
            //if Ship Destroyed reset all values
        }
    }

}