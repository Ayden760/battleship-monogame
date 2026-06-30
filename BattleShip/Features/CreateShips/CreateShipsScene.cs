using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using BattleShip.GameObjects;
using System;
using BattleShip.Features.Game;
using Microsoft.Extensions.DependencyInjection;
using BattleShip.GameData;
using BattleShip.Features.GameOption;
using BattleShip.Services;


namespace BattleShip.Features.CreateShips;

public class CreateShipsScene : Scene
{
    private CreateShipsPanel _panel;
    private CreateShipsController _controller;
    private EscPanel _escPanel;
    private readonly GameSceneManager _sceneManager;
    private readonly InputHandler _InputHandler;
    private readonly IServiceProvider _serviceProvider;
    private GameSession _gameSession;
    private readonly GameSettings _settings;
    private ShipSetter _shipsetter;
    private AiShipSetter _aiShipSetter;

    public CreateShipsScene(GameSceneManager sceneManager, IServiceProvider provider, GameSession session, GameSettings settings, ShipSetter shipSetter, AiShipSetter aiShipSetter, InputHandler handler)
    {
        _serviceProvider = provider;
        _sceneManager = sceneManager;
        _gameSession = session;
        _settings = settings;
        _shipsetter = shipSetter;
        _aiShipSetter = aiShipSetter;
        _InputHandler = handler;


    }
    public override void Initialize()
    {
        base.Initialize();
        Core.ExitOnEscape = false;
        _shipsetter.InitializeFromSettings(_settings);
        _aiShipSetter.InitializeFromSettings(_settings);
        _gameSession.CurrentPlayer = _gameSession.Player1;
        _panel = _serviceProvider.GetRequiredService<CreateShipsPanel>();
        _controller = _serviceProvider.GetRequiredService<CreateShipsController>();
        _escPanel = _serviceProvider.GetRequiredService<EscPanel>();
        _escPanel.ResumeClicked += OnResumeClicked;
        _escPanel.QuitClicked += OnQuitClicked;
        _panel.AddToRoot();
        _escPanel.AddToRoot();

    }
    public override void Update(GameTime gameTime)
    {
        if (_InputHandler.Pause())
        {
            _escPanel.IsVisible = !_escPanel.IsVisible;
        }

        GumService.Default.Update(gameTime);
        if (_escPanel.IsVisible)
        {
            return;
        }
        _controller.Update();
        _panel.RefreshUi();
        if (_controller.StateMatch == MatchState.SetupComplete)
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

    private void OnResumeClicked()
    {
        _escPanel.IsVisible = false;
    }

    private void OnQuitClicked()
    {
        _escPanel.IsVisible = false;
        _panel.IsVisible = false;
        _sceneManager.ChangeScene<TitleScene>();
    }
}