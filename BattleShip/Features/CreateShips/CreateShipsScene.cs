using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using BattleShip.GameObjects;
using System;
using BattleShip.Features.Game;
using Microsoft.Extensions.DependencyInjection;
using BattleShip.GameData;


namespace BattleShip.Features.CreateShips;

public class CreateShipsScene : Scene
{
    private CreateShipsPanel _panel;
    private CreateShipsController _controller;
    private readonly GameSceneManager _sceneManager;
    private readonly IServiceProvider _serviceProvider;
    private GameSession _gameSession;
    private readonly GameSettings _settings;
    private ShipSetter _shipsetter;
    private AiShipSetter _aiShipSetter;

    public CreateShipsScene(GameSceneManager sceneManager, IServiceProvider provider, GameSession session, GameSettings settings, ShipSetter shipSetter, AiShipSetter aiShipSetter)
    {
        _serviceProvider = provider;
        _sceneManager = sceneManager;
        _gameSession = session;
        _settings = settings;
        _shipsetter = shipSetter;
        _aiShipSetter = aiShipSetter;


    }
    public override void Initialize()
    {
        base.Initialize();
        _shipsetter.InitializeFromSettings(_settings);
        _aiShipSetter.InitializeFromSettings(_settings);
        _gameSession.CurrentPlayer = _gameSession.Player1;
        _panel = _serviceProvider.GetRequiredService<CreateShipsPanel>();
        _controller = _serviceProvider.GetRequiredService<CreateShipsController>();
        _panel.AddToRoot();

    }
    public override void Update(GameTime gameTime)
    {

        GumService.Default.Update(gameTime);
        _controller.Update();
        _panel.RefreshUi();
        if (_controller.ShouldSwitchToGameScene)
        {
            _panel.IsVisible = false;


            //DI
            _sceneManager.ChangeScene<GameScene>();
        }
    }
    public override void Draw(GameTime gameTime)
    {
        _panel.Draw(gameTime);

    }
}