namespace BattleShip.UiHelper;
using MonoGameGum.GueDeriving;

public static class UiHelper
{

    //makes it possible to set the Text Scale of the Buttons
    public static void SetTextFontScale(object ship, float fontScale)
    {
        var textRuntime = (ship as dynamic)?.Visual?.GetGraphicalUiElementByName("TextInstance") as TextRuntime;

        if (textRuntime != null)
        {
            textRuntime.FontScale = fontScale;
        }
    }
}

