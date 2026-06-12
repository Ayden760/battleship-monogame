using BattleShip.Scenes;
using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using BattleShip.GameData;
using Microsoft.Xna.Framework.Graphics;
using BattleShip.GameObjects;

namespace BattleShip.Features.CreateShips;

using Data = GameData.GameData;
public class CreateShipsScene : Scene
{
    private CreateShipsPanel _panel;
    private CreateShipsController _controller;

    public override void Initialize()
    {
        base.Initialize();
        ShipSetter.InitializeFromSettings(Data.Settings);
        _controller = new CreateShipsController();
        Data.Ship.CurrentPlayer = Data.Ship.Player1;

        _panel = new CreateShipsPanel(_controller);
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
            Core.ChangeScene(new GameScene());
        }
    }
    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        Assets.Tilemap3x.Draw(Core.SpriteBatch);
        ShipSetter.DrawShips();

        Core.SpriteBatch.End();
        GumService.Default.Draw();
    }
}