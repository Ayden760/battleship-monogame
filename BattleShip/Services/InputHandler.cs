using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Input;
namespace BattleShip.Services;

using Data = GameData.GameData;

public static class InputHandler
{

    private static KeyboardInfo s_keyboard => Core.Input.Keyboard;
    private static MouseInfo s_mouse => Core.Input.Mouse;



    public static bool MoveUp()
    {


        return s_keyboard.WasKeyJustPressed(Keys.W) ||
               s_keyboard.WasKeyJustPressed(Keys.Up);
    }

    public static bool MoveDown()
    {
        return s_keyboard.WasKeyJustPressed(Keys.S) ||
               s_keyboard.WasKeyJustPressed(Keys.Down);
    }

    public static bool MoveLeft()
    {
        return s_keyboard.WasKeyJustPressed(Keys.A) ||
               s_keyboard.WasKeyJustPressed(Keys.Left);
    }

    public static bool MoveRight()
    {
        return s_keyboard.WasKeyJustPressed(Keys.D) ||
               s_keyboard.WasKeyJustPressed(Keys.Right);
    }

    public static bool RotateShip()
    {
        return s_keyboard.WasKeyJustPressed(Keys.R);
    }
    public static bool CheckFieldClicked(ref int y, ref int x)
    {

        if (s_mouse.WasButtonJustPressed(MouseButton.Left))
        {
            Point click = s_mouse.Position;

            int tileSize = 20 * 3;

            x = click.X / tileSize;
            y = click.Y / tileSize;


            bool insideMap = x > 0 && x <= Data.Settings.Rows &&
           y > 0 && y <= Data.Settings.Columns;

            if (insideMap)
            {

                return true;

            }
        }
        return false;
    }
}