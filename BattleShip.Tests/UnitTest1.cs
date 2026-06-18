using BattleShip.Functions;
using BattleShip.GameData;
using BattleShip.GameObjects;

namespace BattleShip.Tests;

public class AiTests
{
    [Fact]
    public void AI_Update_Makes_A_Move_On_Empty_Board()
    {
        var settings = CreateSettings(difficulty: 1);
        var validations = new GameValidations(settings);
        var ai = new AI(3, 3, "AI", null!, settings, validations);

        ai.Update(new List<ShipBase>());

        Assert.True(ai.MadeMove, "AI should make one move on an empty board.");
        Assert.False(ai.MadeHit, "AI should not score a hit on an empty board.");
    }

    [Fact]
    public void AI_Update_Hits_A_Targeted_Ship_Cell()
    {
        var settings = CreateSettings(difficulty: 1);
        var validations = new GameValidations(settings);
        var ai = new AI(3, 3, "AI", null!, settings, validations);

        var ship = new ShipBase(1)
        {
            IsPlaced = true,
            Location = new List<Cell> { new Cell(0, 0) }
        };

        ai.Update(new List<ShipBase> { ship });

        Assert.True(ai.MadeMove, "AI should make a move when a ship is present.");
        Assert.True(ai.MadeHit, "AI should hit the ship at the chosen cell.");
        Assert.Equal(1, ship.Hits);
    }

    private static GameSettings CreateSettings(int difficulty)
    {
        var settings = new GameSettings();
        settings.Initialize(new GameOptions
        {
            Two_tile = 0,
            Three_tile = 0,
            Four_tile = 0,
            Five_tile = 0,
            Difficulty = difficulty,
            Ai_Mode = true,
            DistanceMode = false,
            BonusShotOnHit = false
        });

        return settings;
    }
}
