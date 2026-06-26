using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Input;
using BattleShip.GameData;
namespace BattleShip.Services;


public class InputHandler
{

    private KeyboardInfo s_keyboard => Core.Input.Keyboard;
    private MouseInfo s_mouse => Core.Input.Mouse;
    private readonly GameSettings _settings;


    public InputHandler(GameSettings settings)
    {
        _settings = settings;
    }
    public bool MoveUp()
    {


        return s_keyboard.WasKeyJustPressed(Keys.W) ||
               s_keyboard.WasKeyJustPressed(Keys.Up);
    }

    public bool MoveDown()
    {
        return s_keyboard.WasKeyJustPressed(Keys.S) ||
               s_keyboard.WasKeyJustPressed(Keys.Down);
    }

    public bool MoveLeft()
    {
        return s_keyboard.WasKeyJustPressed(Keys.A) ||
               s_keyboard.WasKeyJustPressed(Keys.Left);
    }

    public bool MoveRight()
    {
        return s_keyboard.WasKeyJustPressed(Keys.D) ||
               s_keyboard.WasKeyJustPressed(Keys.Right);
    }

    public bool RotateShip()
    {
        return s_keyboard.WasKeyJustPressed(Keys.R);
    }
    public bool CheckFieldClicked(ref int y, ref int x)
    {

        if (s_mouse.WasButtonJustPressed(MouseButton.Left))
        {
            Point click = s_mouse.Position;

            int tileSize = 20 * 3;

            x = click.X / tileSize;
            y = click.Y / tileSize;


            bool insideMap = x > 0 && x <= _settings.Rows &&
           y > 0 && y <= _settings.Columns;

            if (insideMap)
            {

                return true;

            }
        }
        return false;
    }
    public bool CheckLeftMouseButtonClicked()
    {
        return s_mouse.WasButtonJustPressed(MouseButton.Left);
    }
    public bool WasKeyJustPressed(Keys key)
    {
        return s_keyboard.WasKeyJustPressed(key);
    }
}