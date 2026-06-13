using BattleShip.GameData;
namespace BattleShip.Features.GameOption;

using Data = GameData.GameData;
public class GameOptionController
{
    public GameOptions Options { get; set; } = new();

    public void SetAi(bool enabled)
    {
        Options.Ai_Mode = enabled;
    }

    public void ChangeDifficulty(int delta)
    {
        if (!Options.Ai_Mode)
            return;

        Options.Difficulty += delta;

        if (Options.Difficulty < 1)
            Options.Difficulty = 1;

        if (Options.Difficulty > 4)
            Options.Difficulty = 4;
    }
    public void ChangeShip(ShipType type, int delta)
    {
        if (delta > 0)
        {
            if (!CanAddShip()) return;
            switch (type)
            {
                case ShipType.Two:
                    Options.Two_tile++;
                    break;
                case ShipType.Three:
                    Options.Three_tile++;
                    break;
                case ShipType.Four:
                    if (Options.Four_tile < 3)
                        Options.Four_tile++;
                    break;
                case ShipType.Five:
                    if (Options.Five_tile < 2)
                        Options.Five_tile++;
                    break;
            }
        }
        else
        {
            if (!CanRemoveShip()) return;

            switch (type)
            {
                case ShipType.Two:
                    if (Options.Two_tile > 0)
                        Options.Two_tile--;
                    break;
                case ShipType.Three:
                    if (Options.Three_tile > 0)
                        Options.Three_tile--;
                    break;
                case ShipType.Four:
                    if (Options.Four_tile > 0)
                        Options.Four_tile--;
                    break;
                case ShipType.Five:
                    if (Options.Five_tile > 0)
                        Options.Five_tile--;
                    break;
            }
        }


    }
    private bool CanAddShip()
    {
        return Options.TotalShips < 9;
    }

    private bool CanRemoveShip()
    {
        return Options.TotalShips > 1;
    }
    public void SetDistance(bool enabled)
    {
        Options.DistanceMode = enabled;
    }
    public void SetBonusShot(bool enabled)
    {
        Options.BonusShotOnHit = enabled;
    }



    public void ApplyToGameData()
    {
        Data.Settings = new GameSettings(Options);
        Data.Ship = new GameShip();
    }
}