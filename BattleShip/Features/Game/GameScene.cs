using System;
using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameLibrary.Scenes;
using Microsoft.Extensions.DependencyInjection;

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
        _panel = _serviceProvider.GetRequiredService<GamePanel>();
        _controller = _serviceProvider.GetRequiredService<GameController>();
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