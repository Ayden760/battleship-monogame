using BattleShip.GameData;
namespace BattleShip.Features;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;
using Gum.Forms.Controls;
using MonoGameGum.GueDeriving;
using BattleShip.UiHelper;

using Data = GameData.GameData;
using CsvHelper.Configuration.Attributes;

public class GameScene : Scene
{
    private enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    private Panel _GameScreenButtonsPanel;
    private TextRuntime _currentPlayerText;
    private GameState _state;
    private Func<string, string> _player_turn = (x) => $"Player {x}'s Turn";

    private Texture2D _pixel;

    public override void Initialize()
    {
        // TODO: Add your initialization logic here
        CreateInfoPanel();
        base.Initialize();
        Data.Ship.CurrentPlayer = Data.Ship.Player1;
        if (Data.Settings.Ai_Mode)
        {
            Data.Ship.OldPlayer = Data.Ship.Ki;
        }
        else
        {
            Data.Ship.OldPlayer = Data.Ship.Player2;
        }
        _state = GameState.Playing;
    }

    public override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
        _pixel = new Texture2D(Core.GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });




    }
    public override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        GumService.Default.Update(gameTime);
        if (_state == GameState.GameOver)
        {
            return;
        }
        _currentPlayerText.Text = _player_turn(Data.Ship.CurrentPlayer.Name);
        CheckWin();

        Data.Ship.CurrentPlayer.Update(Data.Ship.OldPlayer.ShipBases);

    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        Assets.Tilemap3x.Draw(Core.SpriteBatch);
        Data.Ship.CurrentPlayer.DrawField(Data.Ship.OldPlayer.ShipBases);

        if (_state == GameState.GameOver)
        {
            DrawWinScreen();
        }
        Core.SpriteBatch.End();
        GumService.Default.Draw();

    }
    private void CreateInfoPanel()
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);
        _GameScreenButtonsPanel = new Panel();
        _GameScreenButtonsPanel.Dock(Gum.Wireframe.Dock.Fill);
        _GameScreenButtonsPanel.AddToRoot();

        Button Turn = new Button();
        Turn.Anchor(Gum.Wireframe.Anchor.TopRight);
        Turn.Height = 16;
        Turn.Width = 50;

        Turn.X = -40;
        Turn.Y = 150;
        Turn.Text = "End Turn";
        UiHelper.SetTextFontScale(Turn, 0.5f);
        Turn.Click += HandleTurn;
        _GameScreenButtonsPanel.AddChild(Turn);

        _currentPlayerText = new TextRuntime();
        _currentPlayerText.Anchor(Gum.Wireframe.Anchor.TopRight);
        _currentPlayerText.Y = 20;
        _currentPlayerText.X = -10;
        _currentPlayerText.FontScale = 0.5f;
        _currentPlayerText.Text = "Player 1's Turn";

        _GameScreenButtonsPanel.AddChild(_currentPlayerText);


    }
    private void HandleTurn(object sender, EventArgs e)
    {

        if (Data.Ship.CurrentPlayer.MadeMove)
        {
            Data.Ship.CurrentPlayer.MadeMove = false;
            if (Data.Ship.CurrentPlayer.MadeHit)
            {
                return;
            }
            Data.Ship.OldPlayer = Data.Ship.CurrentPlayer;

            if (Data.Ship.CurrentPlayer == Data.Ship.Player1)
            {



                if (!Data.Settings.Ai_Mode)
                {
                    Data.Ship.CurrentPlayer = Data.Ship.Player2;
                }
                else
                {
                    Data.Ship.CurrentPlayer = Data.Ship.Ki;
                    //auf KI setzen
                }


            }
            else if (Data.Ship.CurrentPlayer == Data.Ship.Player2)
            {
                Data.Ship.CurrentPlayer = Data.Ship.Player1;
            }
            else
            {
                //Ai's turn

            }
        }
    }
    public void CheckWin()
    {
        if (Data.Ship.CurrentPlayer.HasWon(Data.Ship.OldPlayer.ShipBases))
        {

            _state = GameState.GameOver;

            _GameScreenButtonsPanel.IsVisible = false;


        }
    }
    private void DrawWinScreen()
    {
        string text = $"{Data.Ship.CurrentPlayer.Name} has won!";

        Vector2 textSize = Assets.Arial.MeasureString(text);

        Vector2 screenCenter = new Vector2(
            Core.GraphicsDevice.Viewport.Width / 2,
            Core.GraphicsDevice.Viewport.Height / 2
        );

        Rectangle bg = new Rectangle(
            (int)(screenCenter.X - textSize.X / 2 - 30),
            (int)(screenCenter.Y - textSize.Y / 2 - 20),
            (int)textSize.X + 60,
            (int)textSize.Y + 40
        );

        // dark overlay over the whole game
        Core.SpriteBatch.Draw(
            _pixel,
            new Rectangle(0, 0,
                Core.GraphicsDevice.Viewport.Width,
                Core.GraphicsDevice.Viewport.Height),
            Color.Black * 0.6f
        );

        // lightblue Background fpr the Winning Text
        Core.SpriteBatch.Draw(
            _pixel,
            bg,
            Color.LightSkyBlue
        );

        // Text centered
        Vector2 textPos = new Vector2(
            bg.Center.X - textSize.X / 2,
            bg.Center.Y - textSize.Y / 2
        );

        Core.SpriteBatch.DrawString(
            Assets.Arial,
            text,
            textPos,
            Color.White
        );
    }
}