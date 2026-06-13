
using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameGum.GueDeriving;
using Gum.Forms.Controls;
using BattleShip.GameObjects;

using BattleShip.GameData;
namespace BattleShip.Features.CreateShips;

using Data = GameData.GameData;
using BattleShip.UiHelper;

public class CreateShipsPanel : Panel
{
    private readonly CreateShipsController _controller;

    private TextRuntime _ship2Text;
    private TextRuntime _ship3Text;
    private TextRuntime _ship4Text;
    private TextRuntime _ship5Text;

    TextRuntime _currentPlayerText;


    public CreateShipsPanel(CreateShipsController controller)
    {
        _controller = controller;

        CreateShipPanel();

    }
    public void CreateShipPanel()
    {


        Dock(Gum.Wireframe.Dock.Fill);


        TextRuntime shipsText = new TextRuntime();
        shipsText.Anchor(Gum.Wireframe.Anchor.TopRight);
        shipsText.Y = 40;
        shipsText.X = -20;
        shipsText.Text = "Ships Available:";
        shipsText.FontScale = 0.5f;

        AddChild(shipsText);

        Button ship_2 = new Button();
        ship_2.Anchor(Gum.Wireframe.Anchor.TopRight);
        ship_2.Y = 60;
        ship_2.X = -80;
        ship_2.Width = 15;
        ship_2.Height = 5;
        ship_2.Text = "2er";
        UiHelper.SetTextFontScale(ship_2, 0.25f);
        ship_2.Click += (_, _) => _controller.HandleShipClicked(2);
        AddChild(ship_2);

        Button ship_3 = new Button();
        ship_3.Anchor(Gum.Wireframe.Anchor.TopRight);
        ship_3.Y = 80;
        ship_3.X = -80;
        ship_3.Width = 15;
        ship_3.Height = 5;
        ship_3.Text = "3er";
        UiHelper.SetTextFontScale(ship_3, 0.25f);
        ship_3.Click += (_, _) => _controller.HandleShipClicked(3);
        AddChild(ship_3);

        Button ship_4 = new Button();
        ship_4.Anchor(Gum.Wireframe.Anchor.TopRight);
        ship_4.Y = 100;
        ship_4.X = -80;
        ship_4.Width = 15;
        ship_4.Height = 5;
        ship_4.Text = "4er";
        UiHelper.SetTextFontScale(ship_4, 0.25f);
        ship_4.Click += (_, _) => _controller.HandleShipClicked(4);
        AddChild(ship_4);

        Button ship_5 = new Button();
        ship_5.Anchor(Gum.Wireframe.Anchor.TopRight);
        ship_5.Y = 120;
        ship_5.X = -80;
        ship_5.Width = 15;
        ship_5.Height = 5;
        ship_5.Text = "5er";
        UiHelper.SetTextFontScale(ship_5, 0.25f);
        ship_5.Click += (_, _) => _controller.HandleShipClicked(5);
        AddChild(ship_5);


        //Text

        _ship2Text = new TextRuntime();
        _ship2Text.Anchor(Gum.Wireframe.Anchor.TopRight);
        _ship2Text.FontScale = 0.25f;
        _ship2Text.X = -55;
        _ship2Text.Y = 60;
        _ship2Text.Text = "0";
        AddChild(_ship2Text);

        _ship3Text = new TextRuntime();
        _ship3Text.Anchor(Gum.Wireframe.Anchor.TopRight);
        _ship3Text.FontScale = 0.25f;
        _ship3Text.X = -55;
        _ship3Text.Y = 80;
        _ship3Text.Text = "0";
        AddChild(_ship3Text);

        _ship4Text = new TextRuntime();
        _ship4Text.Anchor(Gum.Wireframe.Anchor.TopRight);
        _ship4Text.FontScale = 0.25f;
        _ship4Text.X = -55;
        _ship4Text.Y = 100;
        _ship4Text.Text = "0";
        AddChild(_ship4Text);

        _ship5Text = new TextRuntime();
        _ship5Text.Anchor(Gum.Wireframe.Anchor.TopRight);
        _ship5Text.FontScale = 0.25f;
        _ship5Text.X = -55;
        _ship5Text.Y = 120;
        _ship5Text.Text = "0";
        AddChild(_ship5Text);

        _currentPlayerText = new TextRuntime();
        _currentPlayerText.Anchor(Gum.Wireframe.Anchor.TopRight);
        _currentPlayerText.Y = 20;
        _currentPlayerText.X = -10;
        _currentPlayerText.FontScale = 0.5f;
        _currentPlayerText.Text = "Player 1's Turn";

        AddChild(_currentPlayerText);

        Button Confirm_Placement = new Button();
        Confirm_Placement.Anchor(Gum.Wireframe.Anchor.BottomRight);
        Confirm_Placement.Y = -40;
        Confirm_Placement.X = -10;
        Confirm_Placement.Width = 80;
        Confirm_Placement.Height = 20;
        Confirm_Placement.Text = "Confirm";
        Confirm_Placement.Click += (_, _) => _controller.ConfirmClicked();
        AddChild(Confirm_Placement);
        RefreshUi();
    }



    public void RefreshUi()
    {

        _ship2Text.Text = ShipSetter.Two_tile.ToString();
        _ship3Text.Text = ShipSetter.Three_tile.ToString();
        _ship4Text.Text = ShipSetter.Four_tile.ToString();
        _ship5Text.Text = ShipSetter.Five_tile.ToString();
        _currentPlayerText.Text = _controller.GetCurrentPlayerText();
    }




}