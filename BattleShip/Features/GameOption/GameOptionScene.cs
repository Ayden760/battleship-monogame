using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameLibrary.Scenes;
using BattleShip.Features.CreateShips;
namespace BattleShip.Features.GameOption;

public class GameOptionScene : Scene
{
    private GameOptionPanel _panel;
    private GameOptionController _controller;
    private readonly GameSceneManager _sceneManager;
    public GameOptionScene(GameSceneManager sceneManager, GameOptionPanel optionPanel, GameOptionController optionController)
    {
        _sceneManager = sceneManager;
        _panel = optionPanel;
        _controller = optionController;

    }
    public override void Initialize()
    {
        base.Initialize();
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