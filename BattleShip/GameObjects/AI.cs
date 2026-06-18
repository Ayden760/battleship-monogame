using System.Linq;
using System;
using BattleShip.Functions;
using BattleShip.Services;
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

    private readonly GameSettings _settings;

    private GameValidations _validations;

    public AI(int rows, int columns, string name, InputHandler handler, GameSettings settings, GameValidations validations)
    : base(rows, columns, name, handler, validations)
    {
        _settings = settings;
        _validations = validations;
    }


    public override void Update(List<ShipBase> shipBases)
    {
        AIUpdate(shipBases);
    }
    public void AIUpdate(List<ShipBase> shipBases)
    {
        if (MadeMove)
            return;

        int difficulty = _settings?.Difficulty ?? 1;

        if (difficulty == 1 && state != AIState.Searching)
        {
            state = AIState.Searching;
        }

        switch (state)
        {
            case AIState.Searching:
                Searching(shipBases);
                break;

            case AIState.FoundHit:
                if (difficulty < 2)
                {
                    state = AIState.Searching;
                    break;
                }

                PickDirection(shipBases);
                break;

            case AIState.Targeting:
                if (difficulty < 3)
                {
                    state = AIState.Searching;
                    break;
                }

                FollowDirection(shipBases);
                break;
        }

        CheckShipDestroyed();
    }
    private void Searching(List<ShipBase> shipBases)
    {
        var (nextX, nextY) = GetNextSearchCell(shipBases);

        if (nextX < 0 || nextY < 0)
        {
            return;
        }

        x = nextX;
        y = nextY;

        firstHitx = x;
        firstHity = y;

        var (hit, move) = _validations.Check_Set_Hit(shipBases, x, y, ref _Field);

        MadeHit = hit;
        MadeMove = move;
        if (hit)
        {
            UpdateCurrentTargetShip(shipBases, x, y);
        }
        if (hit)
        {
            state = (_settings?.Difficulty ?? 1) == 1
                ? AIState.Searching
                : AIState.FoundHit;
        }
    }
    private (int x, int y) GetNextSearchCell(List<ShipBase> shipBases)
    {
        int difficulty = _settings?.Difficulty ?? 1;

        if (difficulty == 4 && Random.Shared.Next(100) < 20)
        {

            List<(int x, int y)> directHitCandidates = new();

            foreach (var ship in shipBases)
            {
                foreach (var cell in ship.Location)
                {
                    if (_Field[cell.Y, cell.X] != FieldState.Hit &&
                        _Field[cell.Y, cell.X] != FieldState.Miss)
                    {
                        directHitCandidates.Add((cell.X, cell.Y));
                    }
                }
            }

            if (directHitCandidates.Count > 0)
            {
                int index = Random.Shared.Next(directHitCandidates.Count);
                return directHitCandidates[index];
            }
        }

        bool useGrid = difficulty > 1;
        List<(int x, int y)> preferred = new();
        List<(int x, int y)> fallback = new();

        int rows = _Field.GetLength(0);
        int columns = _Field.GetLength(1);

        for (int yy = 0; yy < rows; yy++)
        {
            for (int xx = 0; xx < columns; xx++)
            {
                if (_Field[yy, xx] == FieldState.Hit || _Field[yy, xx] == FieldState.Miss)
                {
                    continue;
                }

                var cell = (xx, yy);
                if (useGrid && ((xx + yy) % 2 == 0))
                {
                    preferred.Add(cell);
                }
                else
                {
                    fallback.Add(cell);
                }
            }
        }

        if (preferred.Count > 0)
        {
            int index = Random.Shared.Next(preferred.Count);
            return preferred[index];
        }

        if (fallback.Count > 0)
        {
            int index = Random.Shared.Next(fallback.Count);
            return fallback[index];
        }

        return (-1, -1);
    }

    private void PickDirection(List<ShipBase> shipBases)
    {
        x = firstHitx;
        y = firstHity;

        if (availableDirections.Count == 0)
        {
            state = AIState.Searching;
            currentTargetShip = null;
            ResetAvailableDirections();
            return;
        }

        int index = Random.Shared.Next(availableDirections.Count);
        currentDirection = availableDirections[index];
        availableDirections.RemoveAt(index);

        MoveInDirection(currentDirection, ref x, ref y);

        var (hit, move) = _validations.Check_Set_Hit(shipBases, x, y, ref _Field);
        int difficulty = _settings?.Difficulty ?? 1;

        MadeHit = hit;
        MadeMove = move;

        if (hit)
        {
            UpdateCurrentTargetShip(shipBases, x, y);
            if (difficulty >= 3)
            {
                state = AIState.Targeting;
            }
            else
            {
                firstHitx = x;
                firstHity = y;
                ResetAvailableDirections();
                state = AIState.FoundHit;
            }
        }
    }
    private void FollowDirection(List<ShipBase> shipBases)
    {
        MoveInDirection(currentDirection, ref x, ref y);

        var (hit, move) = _validations.Check_Set_Hit(shipBases, x, y, ref _Field);

        MadeHit = hit;
        MadeMove = move;

        if (hit)
        {
            UpdateCurrentTargetShip(shipBases, x, y);
        }

        if (!hit)
        {
            //change direction
            currentDirection = ChangeDirections(currentDirection);
            ResetToFirstHit();
            state = AIState.Targeting;

        }

    }
    private void UpdateCurrentTargetShip(List<ShipBase> shipBases, int x, int y)
    {
        var result = _validations.IsThereShip(shipBases, x, y);
        if (result.found)
        {
            currentTargetShip = shipBases[result.location];
        }
    }

    private void CheckShipDestroyed()
    {
        if (currentTargetShip != null && currentTargetShip.Destroyed)
        {
            state = AIState.Searching;
            currentTargetShip = null;
            ResetAvailableDirections();
        }
    }

    private void ResetAvailableDirections()
    {
        availableDirections = new()
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right
        };
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