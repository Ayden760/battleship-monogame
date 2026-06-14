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


    private Direction currentDirection;

    private int x;
    private int y;
    private AIState state = AIState.Searching;
    private int firstHitx;
    private int firstHity;

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
        if (MadeMove)
            return;

        switch (state)
        {
            case AIState.Searching:
                Searching(shipBases);
                break;

            case AIState.FoundHit:
                PickDirection(shipBases);
                break;

            case AIState.Targeting:
                FollowDirection(shipBases);
                break;
        }

        CheckShipDestroyed();
    }
    private void Searching(List<ShipBase> shipBases)
    {
        do
        {
            x = Random.Shared.Next(0, 10);
            y = Random.Shared.Next(0, 10);
        }
        while ((x + y) % 2 != 0);

        firstHitx = x;
        firstHity = y;

        var (hit, move) = GameValidations.Check_Set_Hit(shipBases, x, y, ref _Field);

        MadeHit = hit;
        MadeMove = move;
        if (hit && currentTargetShip == null)
        {
            var result = GameValidations.IsThereShip(shipBases, x, y);
            if (result.found)
            {
                currentTargetShip = shipBases[result.location];
            }
        }
        if (hit)
        {
            state = AIState.FoundHit;
        }
    }
    private void PickDirection(List<ShipBase> shipBases)
    {
        x = firstHitx;
        y = firstHity;

        if (availableDirections.Count == 0)
        {
            state = AIState.Searching;
            return;
        }

        int index = Random.Shared.Next(availableDirections.Count);
        currentDirection = availableDirections[index];
        availableDirections.RemoveAt(index);

        MoveInDirection(currentDirection, ref x, ref y);

        var (hit, move) = GameValidations.Check_Set_Hit(shipBases, x, y, ref _Field);

        MadeHit = hit;
        MadeMove = move;

        if (hit)
        {
            state = AIState.Targeting;

        }

    }
    private void FollowDirection(List<ShipBase> shipBases)
    {
        MoveInDirection(currentDirection, ref x, ref y);

        var (hit, move) = GameValidations.Check_Set_Hit(shipBases, x, y, ref _Field);

        MadeHit = hit;
        MadeMove = move;

        if (!hit)
        {
            //change direction
            currentDirection = ChangeDirections(currentDirection);
            ResetToFirstHit();
            state = AIState.Targeting;

        }

    }
    private void CheckShipDestroyed()
    {
        if (currentTargetShip != null && currentTargetShip.Destroyed)
        {
            state = AIState.Searching;
            currentTargetShip = null;

            availableDirections = new()
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right
        };
        }
    }



    private Direction ChangeDirections(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                direction = Direction.Down;
                break;

            case Direction.Down:
                direction = Direction.Up;
                break;

            case Direction.Left:
                direction = Direction.Right;
                break;

            case Direction.Right:
                direction = Direction.Left;
                break;
        }
        return direction;
    }
    private void ResetToFirstHit()
    {
        y = firstHity;
        x = firstHitx;
    }
    private void MoveInDirection(Direction direction, ref int newX, ref int newY)
    {
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
    }

}