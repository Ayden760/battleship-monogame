using System;
using Gum.Forms.Controls;
using Gum.DataTypes;
using MonoGameGum.GueDeriving;

namespace BattleShip.Features.GameOption;

using BattleShip.UiHelper;

public class EscPanel : Panel
{

    public event Action ResumeClicked;
    public event Action QuitClicked;
    public EscPanel()
    {

        CreateEscPanel();
    }


    public void CreateEscPanel()
    {
        Anchor(Gum.Wireframe.Anchor.Center);
        WidthUnits = DimensionUnitType.Absolute;
        HeightUnits = DimensionUnitType.Absolute;
        Width = 200;
        Height = 70;
        IsVisible = false;
        ColoredRectangleRuntime background = new ColoredRectangleRuntime();
        background.WidthUnits = DimensionUnitType.RelativeToParent;
        background.HeightUnits = DimensionUnitType.RelativeToParent;
        background.Width = 0;
        background.Height = 0;
        background.Red = 40;
        background.Green = 40;
        background.Blue = 40;
        background.Alpha = 220;
        AddChild(background);

        TextRuntime text = new TextRuntime();
        text.Text = "PAUSED";
        text.FontScale = 0.5f;
        text.X = 10;
        text.Y = 10;
        AddChild(text);

        Button resumebutton = new Button();
        resumebutton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        resumebutton.Y = -9;
        resumebutton.X = 9;
        resumebutton.Width = 60;
        resumebutton.Text = "Resume";
        UiHelper.SetTextFontScale(resumebutton, 0.5f);
        resumebutton.Click += (_, _) => OnResumeButtonClicked();


        AddChild(resumebutton);

        Button quitButton = new Button();
        quitButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        quitButton.X = -9;
        quitButton.Y = -9;
        quitButton.Width = 60;
        quitButton.Text = "Quit";
        UiHelper.SetTextFontScale(quitButton, 0.5f);
        quitButton.Click += (_, _) => OnQuitButtonClicked();

        AddChild(quitButton);

    }
    private void OnResumeButtonClicked()
    {
        IsVisible = false;
        ResumeClicked?.Invoke();
    }
    private void OnQuitButtonClicked()
    {
        IsVisible = false;
        QuitClicked?.Invoke();
    }

}