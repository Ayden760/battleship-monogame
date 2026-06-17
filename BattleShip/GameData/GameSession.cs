using BattleShip.Functions;
using BattleShip.GameObjects;
using BattleShip.Services;
using Gum.Wireframe;
namespace BattleShip.GameData;

using Data = GameData;
public class GameSession
{
    private readonly GameSettings _settings;

    public AI Ai;
    public Player Player1;
    public Player Player2;

    public Player CurrentPlayer { get; set; }
    public Player OldPlayer { get; set; }

    //Game Options

    public GameSession(GameSettings settings, InputHandler handler, GameValidations gameValidations)
    {
        _settings = settings;
        Player1 = new Player(_settings.Rows, _settings.Columns, "Player1", handler, gameValidations);

        if (!_settings.Ai_Mode)
        {
            Player2 = new Player(_settings.Rows, _settings.Columns, "Player2", handler, gameValidations);
            Ai = null;
        }
        else
        {
            Ai = new AI(_settings.Rows, _settings.Columns, "AI_1", handler, settings, gameValidations);
            Player2 = null;
        }






    }
}