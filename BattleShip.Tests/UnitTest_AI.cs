using BattleShip.Functions;
using BattleShip.GameData;
using BattleShip.GameObjects;
using BattleShip.Services;
using Microsoft.EntityFrameworkCore;

namespace BattleShip.Tests;

public class AiTests
{
    [Fact]
    public void AI_Update_Makes_A_Move_On_Empty_Board()
    {
        var settings = CreateSettings(rows: 1, columns: 1, difficulty: 1);
        var validations = new GameValidations(settings);
        var ai = new AI(1, 1, "AI", null!, settings, validations);

        ai.Update(new List<ShipBase>());

        Assert.True(ai.MadeMove);
        Assert.False(ai.MadeHit);
    }

    [Fact]
    public void AI_Update_Hits_A_Ship_When_It_Picks_That_Cell()
    {
        var settings = CreateSettings(rows: 1, columns: 1, difficulty: 1);
        var validations = new GameValidations(settings);
        var ai = new AI(1, 1, "AI", null!, settings, validations);

        var ship = new ShipBase(1)
        {
            IsPlaced = true,
            Location = new List<Cell> { new Cell(0, 0) }
        };

        ai.Update(new List<ShipBase> { ship });

        Assert.True(ai.MadeMove);
        Assert.True(ai.MadeHit);
        Assert.Equal(1, ship.Hits);
    }

    [Fact]
    public void AI_Update_Should_Not_Hit_Unplaced_Ship()
    {
        var settings = CreateSettings(rows: 1, columns: 1, difficulty: 1);
        var validations = new GameValidations(settings);
        var ai = new AI(1, 1, "AI", null!, settings, validations);

        var ship = new ShipBase(1)
        {
            IsPlaced = false,
            Location = new List<Cell> { new Cell(0, 0) }
        };

        ai.Update(new List<ShipBase> { ship });

        Assert.True(ai.MadeMove);
        Assert.False(ai.MadeHit);
        Assert.Equal(0, ship.Hits);
        Assert.False(ship.Destroyed);
    }
    [Fact]
    public void AI_Update_Should_Increase_Hits_Exactly_Once_Per_New_Hit()
    {
        var settings = CreateSettings(rows: 1, columns: 2, difficulty: 1);
        var validations = new GameValidations(settings);
        var ai = new AI(1, 2, "AI", null!, settings, validations);

        var ship = new ShipBase(2)
        {
            IsPlaced = true,
            Location = new List<Cell> { new Cell(0, 0), new Cell(1, 0) }
        };

        ai.Update(new List<ShipBase> { ship });
        var hitsAfterFirst = ship.Hits;


        ai.MadeMove = false;
        ai.Update(new List<ShipBase> { ship });

        Assert.InRange(ship.Hits, hitsAfterFirst, hitsAfterFirst + 1);
        Assert.InRange(ship.Hits, 1, 2);
    }
    [Fact]
    public void AI_Update_Should_Destroy_Only_After_All_Segments_Are_Hit()
    {
        var settings = CreateSettings(rows: 1, columns: 2, difficulty: 1);
        var validations = new GameValidations(settings);
        var ai = new AI(1, 2, "AI", null!, settings, validations);

        var ship = new ShipBase(2)
        {
            IsPlaced = true,
            Location = new List<Cell> { new Cell(0, 0), new Cell(1, 0) }
        };

        ai.Update(new List<ShipBase> { ship });


        if (ship.Hits == 1)
        {
            Assert.False(ship.Destroyed);
        }

        ai.MadeMove = false;
        ai.Update(new List<ShipBase> { ship });

        Assert.True(ship.Destroyed);
    }
    //AI ShipSetting Test
    [Fact]
    public void AIShipSetter_Should_Place_Ships_Within_Bounds_And_Without_Overlap()
    {
        var settings = CreateSettings(rows: 8, columns: 8, five: 1, four: 1, three: 2, two: 2);
        var validations = new GameValidations(settings);
        var inputHandler = new InputHandler(settings);
        using var dbContext = CreateTestDbContext();
        var session = new GameSession(settings, inputHandler, validations, dbContext);
        var shipPlacer = new ShipPlacer(validations);
        var aiShipSetter = new AiShipSetter(settings, session, shipPlacer);

        aiShipSetter.InitializeFromSettings(settings);
        var ships = aiShipSetter.PlaceAllShipsRandomly();


        //HashSet because it doesnt allow Duplicates
        var occupied = new HashSet<(int X, int Y)>();

        foreach (var ship in ships)
        {
            Assert.True(ship.IsPlaced);
            Assert.Equal(ship.Length, ship.Location.Count);

            foreach (var cell in ship.Location)
            {
                Assert.InRange(cell.X, 0, settings.Columns - 1);
                Assert.InRange(cell.Y, 0, settings.Rows - 1);
                Assert.True(occupied.Add((cell.X, cell.Y)));
            }
        }
    }
    [Fact]
    public void AI_PlaceAllShips_Should_Place_All_GivenShips()
    {
        var settings = CreateSettings(rows: 5, columns: 5, five: 3, three: 2, two: 1);
        var validations = new GameValidations(settings);
        var inputHandler = new InputHandler(settings);
        using var dbContext = CreateTestDbContext();
        var session = new GameSession(settings, inputHandler, validations, dbContext);
        var shipPlacer = new ShipPlacer(validations);
        var aiShipSetter = new AiShipSetter(settings, session, shipPlacer);

        aiShipSetter.InitializeFromSettings(settings);
        var placedShips = aiShipSetter.PlaceAllShipsRandomly();

        Assert.Equal(settings.TotalShips, placedShips.Count);
        Assert.Equal(settings.Two_tile, placedShips.Count(ship => ship.Length == 2));
        Assert.Equal(settings.Three_tile, placedShips.Count(ship => ship.Length == 3));
        Assert.Equal(settings.Four_tile, placedShips.Count(ship => ship.Length == 4));
        Assert.Equal(settings.Five_tile, placedShips.Count(ship => ship.Length == 5));
        Assert.All(placedShips, ship =>
        {
            Assert.True(ship.IsPlaced);
            Assert.Equal(ship.Length, ship.Location.Count);
        });
    }
    [Fact]
    public void AIShipSetter_RandomPlacement_Should_Be_Stable_Over_Many_Runs()
    {
        var settings = CreateSettings(rows: 8, columns: 8, five: 1, four: 1, three: 2, two: 2);
        var validations = new GameValidations(settings);
        var inputHandler = new InputHandler(settings);
        using var dbContext = CreateTestDbContext();
        var session = new GameSession(settings, inputHandler, validations, dbContext);
        var shipPlacer = new ShipPlacer(validations);
        var aiShipSetter = new AiShipSetter(settings, session, shipPlacer);

        for (int i = 0; i < 100; i++)
        {
            aiShipSetter.InitializeFromSettings(settings);
            var ships = aiShipSetter.PlaceAllShipsRandomly();

            Assert.Equal(settings.TotalShips, ships.Count);

            var occupied = new HashSet<(int X, int Y)>();
            foreach (var ship in ships)
            {
                Assert.True(ship.IsPlaced);
                Assert.Equal(ship.Length, ship.Location.Count);

                foreach (var cell in ship.Location)
                {
                    Assert.InRange(cell.X, 0, settings.Columns - 1);
                    Assert.InRange(cell.Y, 0, settings.Rows - 1);
                    Assert.True(occupied.Add((cell.X, cell.Y)));
                }
            }
        }
    }
    private static GameSettings CreateSettings(int rows, int columns, int difficulty = 1, int two = 0, int three = 0, int four = 0, int five = 0)
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
            DistanceMode = false,
            BonusShotOnHit = false
        });

        return settings;
    }
    private static GameDbContext CreateTestDbContext()
    {
        var options = new DbContextOptionsBuilder<GameDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // tests have own DB
            .Options;

        return new GameDbContext(options);
    }

}
