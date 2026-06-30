using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameGum.GueDeriving;
using Gum.Forms.Controls;
using BattleShip.GameData;
using BattleShip.Features.CreateShips;
using Microsoft.Xna.Framework.Graphics;

using BattleShip.Services;
using MonoGameGum;

namespace BattleShip.Features.GameOption;

using System.Diagnostics;
using BattleShip.UiHelper;

public class TitlePanel : Panel
{
    public event Action StartClicked;



    public TitlePanel()
    {
        CreateTitlePanel();
    }


    public void CreateTitlePanel()
    {
        Dock(Gum.Wireframe.Dock.Fill);
        Button startbutton = new Button();
        startbutton.Anchor(Gum.Wireframe.Anchor.Bottom);
        startbutton.Height = 20;
        startbutton.Width = 80;
        startbutton.Y = -4;
        startbutton.Text = "Start";
        UiHelper.SetTextFontScale(startbutton, 0.5f);
        startbutton.Click += (_, _) =>
        {

            IsVisible = false;
            StartClicked?.Invoke();
        };

        AddChild(startbutton);

    }
    public void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        string text = "BattleShip";
        Vector2 size = Assets.ArialBig.MeasureString(text);
        Core.SpriteBatch.DrawString(
                Assets.ArialBig,
                text,
                new Vector2(
                    (Core.GraphicsDevice.Viewport.Width - size.X) / 2,
                    30),
                Color.White);

        Core.SpriteBatch.End();
        GumService.Default.Draw();
    }
}