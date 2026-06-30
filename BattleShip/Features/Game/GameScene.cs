using System;
using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using Microsoft.Extensions.DependencyInjection;
using BattleShip.Features.GameOption;
using BattleShip.GameData;
using BattleShip.Services;

namespace BattleShip.Features.Game;

public class GameScene : Scene
{
    private GamePanel _panel;
    private GameController _controller;
    private EscPanel _escPanel;
    private InputHandler _InputHandler;

    private readonly GameSceneManager _sceneManager;

    private readonly IServiceProvider _serviceProvider;
    public GameScene(GameSceneManager sceneManager, IServiceProvider provider, InputHandler handler)
    {
        _sceneManager = sceneManager;
        _serviceProvider = provider;
        _InputHandler = handler;


    }
    public override void Initialize()
    {

        base.Initialize();
        Core.ExitOnEscape = false;
        _panel = _serviceProvider.GetRequiredService<GamePanel>();
        _controller = _serviceProvider.GetRequiredService<GameController>();
        _escPanel = _serviceProvider.GetRequiredService<EscPanel>();
        _escPanel.ResumeClicked += OnResumeClicked;
        _escPanel.QuitClicked += OnQuitClicked;
        _panel.AddToRoot();
        _escPanel.AddToRoot();
        _controller.Initialize();
    }
    public override void Update(GameTime gameTime)
    {
        if (_InputHandler.Pause() && _controller.State != GameState.GameOver)
        {
            _escPanel.IsVisible = !_escPanel.IsVisible;
        }

        if (_controller.State == GameState.GameOver)
        {
            _escPanel.IsVisible = true;
        }

        GumService.Default.Update(gameTime);
        if (_escPanel.IsVisible)
        {
            return;
        }
        _controller.Update(gameTime);
        _panel.RefreshUi();

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