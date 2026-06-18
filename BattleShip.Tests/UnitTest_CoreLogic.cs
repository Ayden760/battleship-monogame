using BattleShip.Functions;
using BattleShip.GameData;

namespace BattleShip.Tests;

public class CoreLogicTests
{
    [Fact]
    public void CanPlaceShip_Should_ReturnFalse_When_OutOfBounds()
    {
        var settings = CreateSettings(rows: 2, columns: 2);
        var validations = new GameValidations(settings);

        var newShipCells = new List<Cell>
        {
            new Cell(0, 0),
            new Cell(3, 0)
        };

        var canPlace = validations.CanPlaceShip(new List<ShipBase>(), newShipCells);

        Assert.False(canPlace);
    }

    [Fact]
    public void CanPlaceShip_Should_Respect_DistanceMode()
    {
        var settings = CreateSettings(rows: 5, columns: 5, distanceMode: true);
        var validations = new GameValidations(settings);

        var existing = new ShipBase(1)
        {
            IsPlaced = true,
            Location = new List<Cell> { new Cell(2, 2) }
        };

        var newShipCells = new List<Cell> { new Cell(3, 3) };

        var canPlace = validations.CanPlaceShip(new List<ShipBase> { existing }, newShipCells);

        Assert.False(canPlace);
    }

    [Fact]
    public void CheckSetHit_Should_Set_Hit_And_Increase_Hits()
    {
        var settings = CreateSettings(rows: 3, columns: 3);
        var validations = new GameValidations(settings);
        var field = new FieldState[3, 3];

        var ship = new ShipBase(1)
        {
            IsPlaced = true,
            Location = new List<Cell> { new Cell(1, 1) }
        };

        var result = validations.Check_Set_Hit(new List<ShipBase> { ship }, 1, 1, ref field);

        Assert.True(result.madeMove);
        Assert.True(result.madeHit);
        Assert.Equal(FieldState.Hit, field[1, 1]);
        Assert.Equal(1, ship.Hits);
    }

    [Fact]
    public void CheckSetHit_Should_Set_Miss_When_No_Ship()
    {
        var settings = CreateSettings(rows: 3, columns: 3);
        var validations = new GameValidations(settings);
        var field = new FieldState[3, 3];

        var result = validations.Check_Set_Hit(new List<ShipBase>(), 0, 0, ref field);

        Assert.True(result.madeMove);
        Assert.False(result.madeHit);
        Assert.Equal(FieldState.Miss, field[0, 0]);
    }

    [Fact]
    public void CheckSetHit_Should_Not_Allow_Same_Cell_Twice()
    {
        var settings = CreateSettings(rows: 2, columns: 2);
        var validations = new GameValidations(settings);
        var field = new FieldState[2, 2];

        var first = validations.Check_Set_Hit(new List<ShipBase>(), 1, 1, ref field);
        var second = validations.Check_Set_Hit(new List<ShipBase>(), 1, 1, ref field);

        Assert.True(first.madeMove);
        Assert.False(second.madeMove);
        Assert.False(second.madeHit);
    }

    [Fact]
    public void CheckSetHit_Should_ReturnFalse_When_Outside_Field()
    {
        var settings = CreateSettings(rows: 2, columns: 2);
        var validations = new GameValidations(settings);
        var field = new FieldState[2, 2];

        var result = validations.Check_Set_Hit(new List<ShipBase>(), -1, 0, ref field);

        Assert.False(result.madeMove);
        Assert.False(result.madeHit);
    }

    [Fact]
    public void SetSurroundingCellsAsWater_Should_Mark_Neighbors_As_Miss_And_Keep_Hit()
    {
        var settings = CreateSettings(rows: 3, columns: 3);
        var validations = new GameValidations(settings);
        var field = new FieldState[3, 3];

        var ship = new ShipBase(1)
        {
            IsPlaced = true,
            Location = new List<Cell> { new Cell(1, 1) }
        };

        field[1, 1] = FieldState.Hit;
        validations.SetSurroundingCellsAsWater(ship, ref field);

        Assert.Equal(FieldState.Hit, field[1, 1]);
        Assert.Equal(FieldState.Miss, field[0, 0]);
        Assert.Equal(FieldState.Miss, field[0, 1]);
        Assert.Equal(FieldState.Miss, field[0, 2]);
        Assert.Equal(FieldState.Miss, field[1, 0]);
        Assert.Equal(FieldState.Miss, field[1, 2]);
        Assert.Equal(FieldState.Miss, field[2, 0]);
        Assert.Equal(FieldState.Miss, field[2, 1]);
        Assert.Equal(FieldState.Miss, field[2, 2]);
    }

    [Fact]
    public void HasWon_Should_Return_True_Only_When_All_Destroyed()
    {
        var settings = CreateSettings(rows: 3, columns: 3);
        var validations = new GameValidations(settings);

        var destroyed = new ShipBase(1)
        {
            IsPlaced = true,
            Location = new List<Cell> { new Cell(0, 0) },
            Hits = 1
        };

        var alive = new ShipBase(1)
        {
            IsPlaced = true,
            Location = new List<Cell> { new Cell(1, 1) },
            Hits = 0
        };

        Assert.False(validations.HasWon(new List<ShipBase> { destroyed, alive }));
        Assert.True(validations.HasWon(new List<ShipBase> { destroyed }));
    }

    [Fact]
    public void ShipFinder_Should_Find_Ship_And_Index_And_Contains()
    {
        var ship = new ShipBase(2)
        {
            IsPlaced = true,
            Location = new List<Cell> { new Cell(0, 0), new Cell(1, 0) }
        };

        var ships = new List<ShipBase> { ship };

        Assert.Same(ship, ShipFinder.FindShipAt(1, 0, ships));
        Assert.Equal(1, ShipFinder.GetSegmentIndex(ship, 1, 0));
        Assert.Equal(-1, ShipFinder.GetSegmentIndex(ship, 3, 3));
        Assert.True(ShipFinder.Contains(ship, 0, 0));
        Assert.False(ShipFinder.Contains(ship, 5, 5));
    }

    [Fact]
    public void ShipMover_Should_Move_In_All_Directions_And_Rotate()
    {
        var cells = new List<Cell>
        {
            new Cell(2, 2),
            new Cell(3, 2)
        };

        var up = ShipMover.MoveUp(cells);
        var down = ShipMover.MoveDown(cells);
        var left = ShipMover.MoveLeft(cells);
        var right = ShipMover.MoveRight(cells);
        var rotated = ShipMover.Rotate(cells);

        Assert.Equal((2, 1), (up[0].X, up[0].Y));
        Assert.Equal((3, 1), (up[1].X, up[1].Y));

        Assert.Equal((2, 3), (down[0].X, down[0].Y));
        Assert.Equal((3, 3), (down[1].X, down[1].Y));

        Assert.Equal((1, 2), (left[0].X, left[0].Y));
        Assert.Equal((2, 2), (left[1].X, left[1].Y));

        Assert.Equal((3, 2), (right[0].X, right[0].Y));
        Assert.Equal((4, 2), (right[1].X, right[1].Y));

        Assert.Equal((2, 2), (rotated[0].X, rotated[0].Y));
        Assert.Equal((2, 3), (rotated[1].X, rotated[1].Y));
    }

    private static GameSettings CreateSettings(
        int rows,
        int columns,
        bool distanceMode = false,
        int difficulty = 1,
        int two = 0,
        int three = 0,
        int four = 0,
        int five = 0)
    {
        var settings = new GameSettings();
        settings.Initialize(new GameOptions
        {
            Two_tile = two,
            Three_tile = three,
            Four_tile = four,
            Five_tile = five,
            Difficulty = difficulty,
            Ai_Mode = true,
            Rows = rows,
            Columns = columns,
            DistanceMode = distanceMode,
            BonusShotOnHit = false
        });

        return settings;
    }
}