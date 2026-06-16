using Microsoft.Xna.Framework;
using MonoGameGum;
using System;
using MonoGameLibrary.Scenes;
using BattleShip.Features.CreateShips;
using BattleShip.GameData;
using Microsoft.Extensions.DependencyInjection;
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
        _panel = _serviceProvider.GetRequiredService<GameOptionPanel>();
        _controller = _serviceProvider.GetRequiredService<GameOptionController>();
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
        _controller.ApplyToGameData(_serviceProvider);
        _sceneManager.ChangeScene<CreateShipsScene>();
    }
}