using BattleShip.GameData;
using BattleShip.GameObjects;
using BattleShip.Services;

namespace BattleShip.Features.CreateShips;


public class CreateShipsController
{

    private GameSession _session;
    private readonly InputHandler _inputHandler;
    private readonly ShipSetter _shipSetter;
    private readonly AiShipSetter _aiShipSetter;

    public bool ShouldSwitchToGameScene { get; private set; }
    public CreateShipsController(GameSession session, ShipSetter shipSetter, InputHandler handler, AiShipSetter aiShipSetter)
    {
        _session = session;
        _shipSetter = shipSetter;
        _inputHandler = handler;
        _aiShipSetter = aiShipSetter;

    }
    public void Update()
    {

        _shipSetter.CheckAllSet();

        if (!_session.Player1.ShipsSet)
        {
            _session.CurrentPlayer = _session.Player1;
            Check_User_Input();
        }
        else if (_session.Player2 != null)
        {
            if (!_session.Player2.ShipsSet)
            {
                _session.CurrentPlayer = _session.Player2;
                Check_User_Input();
            }
            else
            {
                ShouldSwitchToGameScene = true;
            }


        }
        else if (_session.Ai != null)
        {
            //create Ai fields
            if (!_session.Ai.ShipsSet)
            {
                _aiShipSetter.SetAiShips();
                _session.Ai.ShipsSet = true;
            }
            else
            {
                ShouldSwitchToGameScene = true;
            }

            // set ships for the AI via a function that randomly places all the ships and uses the already made ShipPlacer class
        }


    }
    public void HandleShipClicked(int type)
    {
        _shipSetter.CurrentShip_Length = type;
        _shipSetter.Set_Mode = true;
        _shipSetter.Ship_Selected = false;
    }
    public void ConfirmClicked()
    {
        _shipSetter.Check_Confirm();
    }
    private void Check_User_Input()
    {
        int y = 0;
        int x = 0;
        if (_inputHandler.CheckFieldClicked(ref y, ref x))
        {
            y -= 1;
            x -= 1;
            _shipSetter.Select_CurrentShip(y, x);
        }
        else if (_shipSetter.Ship_Selected)
        {
            _shipSetter.MoveShip();
        }

    }
    public string GetCurrentPlayerText()
    {
        return $"Player {_session.CurrentPlayer.Name}'s Turn";
    }
    public void GenerateShipsClicked()
    {
        _shipSetter.GenerateShipsForCurrentPlayer();

    }

}