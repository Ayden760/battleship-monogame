using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameLibrary.Scenes;
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
      _panel.Draw(gameTime);
    }
}