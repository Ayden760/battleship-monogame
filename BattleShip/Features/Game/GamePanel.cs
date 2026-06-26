using MonoGameGum.GueDeriving;
using Gum.Forms.Controls;
using BattleShip.GameData;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using BattleShip.Services;
using MonoGameLibrary;

namespace BattleShip.Features.Game;


using BattleShip.UiHelper;
using Gum.Forms.DefaultVisuals;

public class GamePanel : Panel
{
    private readonly GameController _controller;
    private GameSession _session;


    private TextRuntime _currentPlayerText;
    private Texture2D _pixel;
    private TextRuntime _ConfigText;
    private TextRuntime _CurrentPlayerStatsText;
    private readonly List<TextRuntime> _highscoreTexts = new();


    public GamePanel(GameController controller, GameSession session)
    {
        _controller = controller;
        _session = session;

        CreateGamePanel();


        _pixel = new Texture2D(Core.GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

    }
    public void CreateGamePanel()
    {



        Dock(Gum.Wireframe.Dock.Fill);


        Button Turn = new Button();
        Turn.Anchor(Gum.Wireframe.Anchor.TopRight);
        Turn.Height = 16;
        Turn.Width = 50;

        Turn.X = -40;
        Turn.Y = 40;
        Turn.Text = "End Turn";
        UiHelper.SetTextFontScale(Turn, 0.5f);
        Turn.Click += (_, _) => _controller.TriggerTurnDelay();
        AddChild(Turn);

        _currentPlayerText = new TextRuntime();
        _currentPlayerText.Anchor(Gum.Wireframe.Anchor.TopRight);
        _currentPlayerText.Y = 20;
        _currentPlayerText.X = -10;
        _currentPlayerText.FontScale = 0.5f;
        _currentPlayerText.Text = "Player 1's Turn";

        AddChild(_currentPlayerText);

        _ConfigText = new TextRuntime();
        _ConfigText.Anchor(Gum.Wireframe.Anchor.TopLeft);
        _ConfigText.Y = 80;
        _ConfigText.X = 190;
        _ConfigText.FontScale = 0.4f;
        _ConfigText.Text = _controller.ConfigText;
        AddChild(_ConfigText);

        _CurrentPlayerStatsText = new TextRuntime();
        _CurrentPlayerStatsText.Anchor(Gum.Wireframe.Anchor.TopLeft);
        _CurrentPlayerStatsText.Y = 130;
        _CurrentPlayerStatsText.X = 190;
        _CurrentPlayerStatsText.FontScale = 0.4f;
        _CurrentPlayerStatsText.Text = _controller.CurrentPlayerStatsText;
        AddChild(_CurrentPlayerStatsText);


        for (int i = 0; i < 5; i++)
        {
            var highscoreText = new TextRuntime();
            highscoreText.Anchor(Gum.Wireframe.Anchor.BottomLeft);

            if (i >= 3)
            {
                highscoreText.X = 180;
                highscoreText.Y = -50 + ((i - 3) * 20);
            }

            else
            {
                highscoreText.X = 60;
                highscoreText.Y = -50 + (i * 20);
            }

            highscoreText.FontScale = 0.35f;
            highscoreText.Text = $"{i + 1}. -";
            _highscoreTexts.Add(highscoreText);
            AddChild(highscoreText);
        }

        UpdateHighscoreTexts();

        TextRuntime HighScores = new TextRuntime();
        HighScores.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        HighScores.X = 5;
        HighScores.Y = -50;
        HighScores.FontScale = 0.4f;
        HighScores.Text = "HighScores";
        AddChild(HighScores);




    }
    public void RefreshUi()
    {
        _currentPlayerText.Text = _controller.GetCurrentPlayerText();
        _CurrentPlayerStatsText.Text = _controller.CurrentPlayerStatsText;
        UpdateHighscoreTexts();
    }

    private void UpdateHighscoreTexts()
    {
        var highscores = _controller.Highscores;

        for (int i = 0; i < _highscoreTexts.Count; i++)
        {
            var entry = (highscores != null && i < highscores.Count && highscores[i] != null)
                ? highscores[i]
                : "-";
            _highscoreTexts[i].Text = $"{i + 1}. {entry}";
        }
    }

    public void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        Assets.Tilemap3x.Draw(Core.SpriteBatch);
        _session.CurrentPlayer.DrawField(_session.OldPlayer.ShipBases);
        if (_controller.State == GameState.GameOver)
        {
            IsVisible = false;
            DrawWinScreen();
        }
        Core.SpriteBatch.End();
        GumService.Default.Draw();
    }
    private void DrawWinScreen()
    {
        string text = $"{_session.CurrentPlayer.Name} has won!";

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