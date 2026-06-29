using Microsoft.Xna.Framework;
using MonoGameGum;
using System;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using BattleShip.Features.CreateShips;
using BattleShip.GameData;
using Microsoft.Extensions.DependencyInjection;
using BattleShip.Services;
namespace BattleShip.Features.GameOption;

public class GameOptionScene : Scene
{
    private GameOptionPanel _panel;
    private GameOptionController _controller;
    private EscPanel _escPanel;
    private readonly GameSceneManager _sceneManager;
    private InputHandler _handler;

    private readonly IServiceProvider _serviceProvider;
    public GameOptionScene(GameSceneManager sceneManager, IServiceProvider provider, InputHandler handler)
    {
        _serviceProvider = provider;
        _sceneManager = sceneManager;
        _handler = handler;

    }
    public override void Initialize()
    {

        base.Initialize();
        Core.ExitOnEscape = false;
        _panel = _serviceProvider.GetRequiredService<GameOptionPanel>();
        _controller = _serviceProvider.GetRequiredService<GameOptionController>();
        _escPanel = _serviceProvider.GetRequiredService<EscPanel>();
        _panel.StartClicked += OnStartClicked;
        _escPanel.ResumeClicked += OnResumeClicked;
        _escPanel.QuitClicked += OnQuitClicked;
        _panel.AddToRoot();
        _escPanel.AddToRoot();

    }
    public override void Update(GameTime gameTime)
    {
        if (_handler.Pause())
        {
            _escPanel.IsVisible = !_escPanel.IsVisible;
        }

        GumService.Default.Update(gameTime);
        if (_escPanel.IsVisible)
        {
            return;
        }
        _controller.Update();
        _panel.Update();
    }
    public override void Draw(GameTime gameTime)
    {
        _panel.Draw(gameTime);
    }
    private void OnStartClicked()
    {
        _controller.ApplyToGameData();
        _sceneManager.ChangeScene<CreateShipsScene>();
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