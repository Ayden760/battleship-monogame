using Microsoft.Xna.Framework;
using MonoGameGum;
using System;
using MonoGameLibrary.Scenes;
using BattleShip.Features.CreateShips;
using BattleShip.GameData;
using Microsoft.Extensions.DependencyInjection;
using MonoGameLibrary;
namespace BattleShip.Features.GameOption;

public class TitleScene : Scene
{
    private TitlePanel _panel;

    private readonly GameSceneManager _sceneManager;

    private readonly IServiceProvider _serviceProvider;
    public TitleScene(GameSceneManager sceneManager, IServiceProvider provider)
    {
        _serviceProvider = provider;
        _sceneManager = sceneManager;

    }
    public override void Initialize()
    {

        base.Initialize();
        Core.ExitOnEscape = true;
        _panel = _serviceProvider.GetRequiredService<TitlePanel>();

        _panel.StartClicked += OnStartClicked;
        _panel.AddToRoot();

    }
    public override void Update(GameTime gameTime)
    {
        GumService.Default.Update(gameTime);
    }
    public override void Draw(GameTime gameTime)
    {
        _panel.Draw(gameTime);
    }
    private void OnStartClicked()
    {

        _sceneManager.ChangeScene<GameOptionScene>();
    }
}