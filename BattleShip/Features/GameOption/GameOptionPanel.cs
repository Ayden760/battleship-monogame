using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameGum.GueDeriving;
using Gum.Forms.Controls;
using BattleShip.GameData;
using BattleShip.Features.CreateShips;
using Microsoft.Xna.Framework.Graphics;

using BattleShip.Services;
using MonoGameGum;

namespace BattleShip.Features.GameOption;

using System.Diagnostics;
using BattleShip.UiHelper;

public class GameOptionPanel : Panel
{
    private readonly GameOptionController _controller;

    private TextRuntime _twoText;
    private TextRuntime _threeText;
    private TextRuntime _fourText;
    private TextRuntime _fiveText;



    private Button _aiYes, _aiNo;
    private Button _distYes, _distNo;
    private Button _BonusShotYes, _BonusShotNo;

    private TextRuntime _difficultyText;
    private Button _diffMinus;
    private Button _diffPlus;
    private TextRuntime _diffLabel;
    public event Action StartClicked;

    private Button _player1NameButton;

    private Button _player2NameButton;
    private TextRuntime _player2Text;


    public GameOptionPanel(GameOptionController controller)
    {
        _controller = controller;

        CreateOptionPanel();

    }


    public void CreateOptionPanel()
    {
        Dock(Gum.Wireframe.Dock.Fill);
        float x = 20;
        float startY = 35;
        float lineH = 16;
        void AddShipRow(string label, int index, Action add, Action remove, out TextRuntime textRef)
        {
            float y = startY + index * lineH + 10;

            TextRuntime txt = new TextRuntime();
            txt.Text = $"{label}er";
            txt.X = x;
            txt.Y = y;
            txt.FontScale = 0.4f;
            AddChild(txt);

            TextRuntime count = new TextRuntime();
            count.Text = "0";
            count.X = 70;
            count.Y = y;
            count.FontScale = 0.4f;
            AddChild(count);

            textRef = count;

            Button minus = new Button();
            minus.Text = "-";
            minus.X = 95;
            minus.Y = y - 3;
            minus.Width = 12;
            minus.Height = 2;
            UiHelper.SetTextFontScale(minus, 0.3f);
            minus.Click += (_, _) =>
            {
                remove();
                RefreshUi();
            };
            AddChild(minus);

            Button plus = new Button();
            plus.Text = "+";
            plus.X = 113;
            plus.Y = y - 3;
            plus.Width = 12;
            plus.Height = 2;
            UiHelper.SetTextFontScale(plus, 0.3f);
            plus.Click += (_, _) =>
            {
                add();
                RefreshUi();
            };
            AddChild(plus);
        }
        AddShipRow("2", 0,
           add: () => _controller.ChangeShip(ShipType.Two, +1),
           remove: () => _controller.ChangeShip(ShipType.Two, -1),
           out _twoText);

        AddShipRow("3", 1,
            add: () => _controller.ChangeShip(ShipType.Three, +1),
            remove: () => _controller.ChangeShip(ShipType.Three, -1),
            out _threeText);

        AddShipRow("4", 2,
            add: () => _controller.ChangeShip(ShipType.Four, +1),
            remove: () => _controller.ChangeShip(ShipType.Four, -1),
            out _fourText);

        AddShipRow("5", 3,
            add: () => _controller.ChangeShip(ShipType.Five, +1),
            remove: () => _controller.ChangeShip(ShipType.Five, -1),
            out _fiveText);

        //Ai Mode
        float sectionY = startY + 4 * lineH + 15;

        TextRuntime aiText = new TextRuntime();
        aiText.Text = "AI";
        aiText.X = x;
        aiText.Y = sectionY;
        aiText.FontScale = 0.4f;
        AddChild(aiText);

        _aiYes = new Button();
        _aiYes.Text = "Yes";
        _aiYes.X = 95;
        _aiYes.Y = sectionY - 3;
        _aiYes.Width = 16;
        _aiYes.Height = 4;
        _aiYes.Visual.SetProperty("Background.Color", Color.Red);
        UiHelper.SetTextFontScale(_aiYes, 0.3f);
        _aiYes.Click += (_, _) => SetAi(true);

        AddChild(_aiYes);

        _aiNo = new Button();
        _aiNo.Text = "No";
        _aiNo.X = 113;
        _aiNo.Y = sectionY - 3;
        _aiNo.Width = 16;
        _aiNo.Height = 4;
        _aiNo.Visual.SetProperty("Background.Color", Color.Green);
        UiHelper.SetTextFontScale(_aiNo, 0.3f);
        _aiNo.Click += (_, _) => SetAi(false);
        AddChild(_aiNo);


        float difficultyY = sectionY + 20;
        _diffLabel = new TextRuntime();
        _diffLabel = new TextRuntime();
        _diffLabel.Text = "Difficulty";
        _diffLabel.X = x;
        _diffLabel.Y = difficultyY;
        _diffLabel.FontScale = 0.4f;
        AddChild(_diffLabel);

        // Value Text
        _difficultyText = new TextRuntime();
        _difficultyText.X = 70;
        _difficultyText.Text = "0";
        _difficultyText.Y = difficultyY;
        _difficultyText.FontScale = 0.4f;
        AddChild(_difficultyText);

        // Minus Button
        _diffMinus = new Button();
        _diffMinus.Text = "-";
        _diffMinus.X = 95;
        _diffMinus.Y = difficultyY - 2;
        _diffMinus.Width = 12;
        _diffMinus.Height = 2;
        UiHelper.SetTextFontScale(_diffMinus, 0.3f);
        _diffMinus.Click += (_, _) =>
        {
            _controller.ChangeDifficulty(-1);
            RefreshUi();
        }
        ;
        AddChild(_diffMinus);

        // Plus Button
        _diffPlus = new Button();
        _diffPlus.Text = "+";
        _diffPlus.X = 113;
        _diffPlus.Y = difficultyY - 2;
        _diffPlus.Width = 12;
        _diffPlus.Height = 2;
        UiHelper.SetTextFontScale(_diffPlus, 0.3f);
        _diffPlus.Click += (_, _) =>
        {
            _controller.ChangeDifficulty(+1);
            RefreshUi();
        };
        AddChild(_diffPlus);

        //Distance Text and Buttons
        float distY = difficultyY + 20;

        TextRuntime distText = new TextRuntime();
        distText.Text = "Distance Mode";
        distText.X = x;
        distText.Y = distY;
        distText.FontScale = 0.4f;
        AddChild(distText);

        _distYes = new Button();
        _distYes.Text = "Yes";
        _distYes.X = 95;
        _distYes.Y = distY - 3;
        _distYes.Width = 16;
        _distYes.Height = 4;
        _distYes.Visual.SetProperty("Background.Color", Color.Green);
        UiHelper.SetTextFontScale(_distYes, 0.3f);
        _distYes.Click += (_, _) => SetDist(true);
        AddChild(_distYes);

        _distNo = new Button();
        _distNo.Text = "No";
        _distNo.X = 113;
        _distNo.Y = distY - 3;
        _distNo.Width = 16;
        _distNo.Height = 4;
        _distNo.Visual.SetProperty("Background.Color", Color.Red);
        UiHelper.SetTextFontScale(_distNo, 0.3f);
        _distNo.Click += (_, _) => SetDist(false);
        AddChild(_distNo);


        float BonusY = distY + 20;
        TextRuntime BonusShotMode = new TextRuntime();
        BonusShotMode.Text = "Bonus Shot on Hit";
        BonusShotMode.X = x;
        BonusShotMode.Y = BonusY;
        BonusShotMode.FontScale = 0.4f;
        AddChild(BonusShotMode);

        _BonusShotYes = new Button();
        _BonusShotYes.Text = "Yes";
        _BonusShotYes.X = 95;
        _BonusShotYes.Y = BonusY - 3;
        _BonusShotYes.Width = 16;
        _BonusShotYes.Height = 4;
        _BonusShotYes.Visual.SetProperty("Background.Color", Color.Green);
        UiHelper.SetTextFontScale(_BonusShotYes, 0.3f);
        _BonusShotYes.Click += (_, _) => SetBonusShot(true);
        AddChild(_BonusShotYes);

        _BonusShotNo = new Button();
        _BonusShotNo.Text = "No";
        _BonusShotNo.X = 113;
        _BonusShotNo.Y = BonusY - 3;
        _BonusShotNo.Width = 16;
        _BonusShotNo.Height = 4;
        _BonusShotNo.Visual.SetProperty("Background.Color", Color.Red);
        UiHelper.SetTextFontScale(_BonusShotNo, 0.3f);
        _BonusShotNo.Click += (_, _) => SetBonusShot(false);
        AddChild(_BonusShotNo);

        Button startbutton = new Button();
        startbutton.Anchor(Gum.Wireframe.Anchor.Bottom);
        startbutton.Height = 18;
        startbutton.Width = 95;
        startbutton.Y = -4;
        startbutton.Text = "Start";
        UiHelper.SetTextFontScale(startbutton, 0.5f);
        startbutton.Click += (_, _) =>
        {

            IsVisible = false;
            StartClicked?.Invoke();
        };

        AddChild(startbutton);





        //Player Names


        TextRuntime Player1Text = new TextRuntime();
        Player1Text.Anchor(Gum.Wireframe.Anchor.TopRight);
        Player1Text.Text = "Player1:";
        Player1Text.X = -60;
        Player1Text.Y = 40;
        Player1Text.FontScale = 0.4f;
        AddChild(Player1Text);

        _player1NameButton = new Button();
        _player1NameButton.Anchor(Gum.Wireframe.Anchor.TopRight);
        _player1NameButton.Text = _controller.Options.Player1Name;
        _player1NameButton.X = -10;
        _player1NameButton.Y = 50;
        _player1NameButton.Width = 80;
        _player1NameButton.Height = 10;
        UiHelper.SetTextFontScale(_player1NameButton, 0.4f);
        _player1NameButton.Visual.SetProperty("Background.Color", Color.DarkCyan);

        _player1NameButton.Click += (_, _) =>
        {
            _controller.SetEditingName(true, PlayerId.Player1);
        };

        AddChild(_player1NameButton);


        _player2Text = new TextRuntime();
        _player2Text.Anchor(Gum.Wireframe.Anchor.TopRight);
        _player2Text.Text = "Player2:";
        _player2Text.X = -60;
        _player2Text.Y = 80;
        _player2Text.FontScale = 0.4f;
        AddChild(_player2Text);

        _player2NameButton = new Button();
        _player2NameButton.Anchor(Gum.Wireframe.Anchor.TopRight);
        _player2NameButton.Text = _controller.Options.Player2Name;
        _player2NameButton.X = -10;
        _player2NameButton.Y = 90;
        _player2NameButton.Width = 80;
        _player2NameButton.Height = 10;
        UiHelper.SetTextFontScale(_player2NameButton, 0.4f);
        _player2NameButton.Visual.SetProperty("Background.Color", Color.DarkCyan);

        _player2NameButton.Click += (_, _) =>
        {
            _controller.SetEditingName(true, PlayerId.Player2);
        };

        AddChild(_player2NameButton);

        TextRuntime InfoText = new TextRuntime();
        InfoText.Anchor(Gum.Wireframe.Anchor.TopRight);
        InfoText.Text = "CLICK BOX TO EDIT NAME";
        InfoText.X = -5;
        InfoText.Y = 120;
        InfoText.FontScale = 0.4f;
        AddChild(InfoText);
    }
    private void RefreshUi()
    {
        _twoText.Text = _controller.Options.Two_tile.ToString();
        _threeText.Text = _controller.Options.Three_tile.ToString();
        _fourText.Text = _controller.Options.Four_tile.ToString();
        _fiveText.Text = _controller.Options.Five_tile.ToString();

        _difficultyText.Text = _controller.Options.Difficulty.ToString();

        _player1NameButton.Visual.SetProperty("Background.Color", Color.DarkCyan);
        UpdatePlayer1NameState(_controller.IsEditingPlayer1Name);
        UpdatePlayer2NameState(_controller.Options.Ai_Mode, _controller.IsEditingPlayer2Name);
    }
    private void SetDist(bool enabled)
    {

        _distYes.Visual.SetProperty("Background.Color",
            enabled ? Color.Green : Color.Red);

        _distNo.Visual.SetProperty("Background.Color",
            enabled ? Color.Red : Color.Green);

        _controller.SetDistance(enabled);
        RefreshUi();
    }

    private void SetAi(bool enabled)
    {

        _aiYes.Visual.SetProperty("Background.Color",
             enabled ? Color.Green : Color.Red);

        _aiNo.Visual.SetProperty("Background.Color",
             enabled ? Color.Red : Color.Green);
        _controller.SetAi(enabled);
        RefreshUi();
        UpdateDifficultyState(enabled);


    }
    private void SetBonusShot(bool enabled)
    {
        _BonusShotYes.Visual.SetProperty("Background.Color",
                   enabled ? Color.Green : Color.Red);

        _BonusShotNo.Visual.SetProperty("Background.Color",
             enabled ? Color.Red : Color.Green);
        _controller.SetBonusShot(enabled);
        RefreshUi();

    }
    private void UpdateDifficultyState(bool enabled)
    {


        _diffMinus.Visual.SetProperty("Background.Color",
            enabled ? Color.DarkCyan : Color.Gray);

        _diffPlus.Visual.SetProperty("Background.Color",
            enabled ? Color.DarkCyan : Color.Gray);

        _diffLabel.Color = enabled ? Color.White : Color.Gray;
        _difficultyText.Color = enabled ? Color.White : Color.Gray;
    }
    private void UpdatePlayer1NameState(bool IsEditing)
    {
        _player1NameButton.Visual.SetProperty("Background.Color", IsEditing ? new Color(0, 180, 180) : Color.DarkCyan);

    }
    private void UpdatePlayer2NameState(bool enabled, bool IsEditing)
    {
        _player2NameButton.Visual.SetProperty("Background.Color", IsEditing ? new Color(0, 180, 180) :
        enabled ? Color.Gray : Color.DarkCyan);
        _player2Text.Color = enabled ? Color.Gray : Color.White;

    }
    public void Update()
    {
        _controller.CancelNameEditingOnClick();
        SetAi(_controller.Options.Ai_Mode);
        SetDist(_controller.Options.DistanceMode);
        SetBonusShot(_controller.Options.BonusShotOnHit);
    }


    public void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        string text = "OPTIONS";
        Vector2 size = Assets.ArialBig.MeasureString(text);
        Core.SpriteBatch.DrawString(
                Assets.ArialBig,
                text,
                new Vector2(
                    (Core.GraphicsDevice.Viewport.Width - size.X) / 2,
                    10),
                Color.White);

        Core.SpriteBatch.End();
        GumService.Default.Draw();
    }
}