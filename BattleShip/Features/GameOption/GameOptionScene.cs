using Microsoft.Xna.Framework;
using MonoGameGum;
using System;
using MonoGameLibrary.Scenes;
using BattleShip.Features.CreateShips;
namespace BattleShip.Features.GameOption;

public class GameOptionScene : Scene
{
    private GameOptionPanel _panel;
    private GameOptionController _controller;
    private readonly GameSceneManager _sceneManager;

    private readonly IServiceProvider _serviceProvider;
    public GameOptionScene(GameSceneManager sceneManager, IServiceProvider provider)
    {
        _serviceProvider = provider;
        _sceneManager = sceneManager;

    }
    public override void Initialize()
    {
        base.Initialize();
        _panel = (GameOptionPanel)_serviceProvider.GetService(typeof(GameOptionPanel));
        _controller = (GameOptionController)_serviceProvider.GetService(typeof(GameOptionController));
        _panel.StartClicked += OnStartClicked;
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
    private void OnStartClicked()
    {
        _sceneManager.ChangeScene<CreateShipsScene>();
    }
}