using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using BattleShip.GameObjects;
using System;
using BattleShip.Features.Game;
using Microsoft.Extensions.DependencyInjection;


namespace BattleShip.Features.CreateShips;

using Data = GameData.GameData;
public class CreateShipsScene : Scene
{
    private CreateShipsPanel _panel;
    private CreateShipsController _controller;
    private readonly GameSceneManager _sceneManager;
    private readonly IServiceProvider _serviceProvider;

    public CreateShipsScene(GameSceneManager sceneManager, IServiceProvider provider)
    {
        _serviceProvider = provider;
        _sceneManager = sceneManager;


    }
    public override void Initialize()
    {
        base.Initialize();
        ShipSetter.InitializeFromSettings(Data.Settings);
        AiShipSetter.InitializeFromSettings(Data.Settings);
        Data.Session.CurrentPlayer = Data.Session.Player1;
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