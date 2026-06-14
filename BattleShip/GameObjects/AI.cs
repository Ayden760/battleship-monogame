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


    private Direction HitDirection;
    private bool foundDirection = false;
    private int x;
    private int y;
    private int firstHitx;
    private int firstHity;
    private bool foundShip = false;
    private bool lastMoveWasHit = false;

    private ShipBase currentTargetShip = null;


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
    public override void Update(List<ShipBase> shipBases)
    {
        AIUpdate(shipBases);
    }

    public void AIUpdate(List<ShipBase> shipBases)
    {


        if (!MadeMove)
        {



            if (!foundShip)
            {
                do
                {
                    x = Random.Shared.Next(0, 10);
                    y = Random.Shared.Next(0, 10);
                }
                while ((x + y) % 2 != 0);
                firstHitx = x;
                firstHity = y;

                var (madeHit, madeMove) = GameValidations.Check_Set_Hit(shipBases, x, y, ref _Field);
                MadeHit = madeHit;
                foundShip = madeHit;
                MadeMove = madeMove;


                //made so the ai knows what ship it currently trys to destroy
                if (madeHit && currentTargetShip == null)
                {
                    var result = GameValidations.IsThereShip(shipBases, x, y);
                    if (result.found)
                    {
                        currentTargetShip = shipBases[result.location];
                    }
                }

            }
            else
            {

                if (!foundDirection)
                {

                    y = firstHity;
                    x = firstHitx;

                    do
                    {
                        // select random direction from available directions
                        int index = Random.Shared.Next(availableDirections.Count);
                        HitDirection = availableDirections[index];
                        //remove selected direction
                        availableDirections.RemoveAt(index);



                    } while (!TryApplyDirection(HitDirection, ref x, ref y));



                    var (madeHit, madeMove) = GameValidations.Check_Set_Hit(shipBases, x, y, ref _Field);
                    MadeHit = madeHit;
                    MadeMove = madeMove;
                    if (madeHit)
                    {

                        foundDirection = true;
                        lastMoveWasHit = true;
                    }


                }
                else
                {


                    if (!lastMoveWasHit)
                    {

                        ChangeDirections();
                        ResetToFirstHit();
                    }

                    bool rffg = TryApplyDirection(HitDirection, ref x, ref y);
                    //Moving in one direction is not working properly
                    var (madeHit, madeMove) = GameValidations.Check_Set_Hit(shipBases, x, y, ref _Field);
                    Console.WriteLine(madeMove);
                    MadeHit = madeHit;
                    MadeMove = madeMove;
                    if (!madeHit)
                    {
                        lastMoveWasHit = false;
                    }


                }

            }


        }

        if (currentTargetShip != null && currentTargetShip.Destroyed)
        {
            //reset AI state
            foundDirection = false;
            foundShip = false;
            lastMoveWasHit = false;
            currentTargetShip = null;
            //reset directions for new ship
            availableDirections = new()
                        {
                            Direction.Up,
                            Direction.Down,
                            Direction.Left,
                            Direction.Right
                        };


        }
    }
    private bool TryApplyDirection(Direction direction, ref int x, ref int y)
    {
        int newX = x;
        int newY = y;

        switch (direction)
        {
            case Direction.Up:
                newY -= 1;
                break;
            case Direction.Down:
                newY += 1;
                break;
            case Direction.Left:
                newX -= 1;
                break;
            case Direction.Right:
                newX += 1;
                break;
        }

        // check bounds
        if (!GameValidations.IsInsideField(newX, newY))
        {
            lastMoveWasHit = false;
            return false;

        }


        //change the old coords
        x = newX;
        y = newY;

        return true;
    }

    private void ChangeDirections()
    {
        switch (HitDirection)
        {
            case Direction.Up:
                HitDirection = Direction.Down;
                break;

            case Direction.Down:
                HitDirection = Direction.Up;
                break;

            case Direction.Left:
                HitDirection = Direction.Right;
                break;

            case Direction.Right:
                HitDirection = Direction.Left;
                break;
        }
    }
    private void ResetToFirstHit()
    {
        y = firstHity;
        x = firstHitx;
    }

}