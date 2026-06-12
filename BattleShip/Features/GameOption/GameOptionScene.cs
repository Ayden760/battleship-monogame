using BattleShip.Scenes;
using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using BattleShip.GameData;
using Microsoft.Xna.Framework.Graphics;

namespace BattleShip.Features.GameOption;

public class GameOptionScene : Scene
{
    private GameOptionPanel _panel;
    private GameOptionController _controller;

    public override void Initialize()
    {
        base.Initialize();
        _controller = new GameOptionController();

        _panel = new GameOptionPanel(_controller);
        _panel.AddToRoot();
    }
    public override void Update(GameTime gameTime)
    {
        GumService.Default.Update(gameTime);
        _panel.Update();
    }
    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        string text = "OPTIONS";
        Vector2 size = Assets.ArialBig.MeasureString(text);
        Core.SpriteBatch.DrawString(
                Assets.ArialBig,
                text,
                new Vector2(
                    (Core.GraphicsDevice.Viewport.Width - size.X) / 2,
                    10),
                Color.White);

        Core.SpriteBatch.End();
        GumService.Default.Draw();
    }
}