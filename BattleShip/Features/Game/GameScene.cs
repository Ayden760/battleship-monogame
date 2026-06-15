using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameLibrary.Scenes;

namespace BattleShip.Features.Game;

public class GameScene : Scene
{
    private GamePanel _panel;
    private GameController _controller;

    public override void Initialize()
    {
        base.Initialize();
        _controller = new GameController();
        _panel = new GamePanel(_controller);
        _panel.AddToRoot();
        _controller.Initialize();
    }
    public override void Update(GameTime gameTime)
    {

        GumService.Default.Update(gameTime);
        _controller.Update(gameTime);
        _panel.RefreshUi();

    }
    public override void Draw(GameTime gameTime)
    {
        _panel.Draw(gameTime);
    }
}