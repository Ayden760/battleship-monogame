using System;
using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameLibrary.Scenes;

namespace BattleShip.Features.Game;

public class GameScene : Scene
{
    private GamePanel _panel;
    private GameController _controller;

    private readonly GameSceneManager _sceneManager;

    private readonly IServiceProvider _serviceProvider;
    public GameScene(GameSceneManager sceneManager, IServiceProvider provider)
    {
        _sceneManager = sceneManager;
        _serviceProvider = provider;


    }
    public override void Initialize()
    {

        base.Initialize();
        _controller = (GameController)_serviceProvider.GetService(typeof(GameController));
        _panel = (GamePanel)_serviceProvider.GetService(typeof(GamePanel));
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