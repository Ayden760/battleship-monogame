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



    public MatchState StateMatch { get; private set; }

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
        UpdateState();
        switch (StateMatch)
        {
            case MatchState.SetupPlayer1:
                _session.CurrentPlayer = _session.Player1;
                Check_User_Input();
                break;
            case MatchState.SetupPlayer2:
                _session.CurrentPlayer = _session.Player2;
                Check_User_Input();
                break;
            case MatchState.SetupAI:
                _aiShipSetter.SetAiShips();
                _session.Ai.ShipsSet = true;
                break;
            case MatchState.SetupComplete:
                break;

        }
    }
    public void UpdateState()
    {
        if (!_session.Player1.ShipsSet)
        {
            StateMatch = MatchState.SetupPlayer1;
            return;
        }
        if (_session.Player2 != null && !_session.Player2.ShipsSet)
        {
            StateMatch = MatchState.SetupPlayer2;
            return;
        }
        if (_session.Ai != null && !_session.Ai.ShipsSet)
        {
            StateMatch = MatchState.SetupAI;
            return;
        }
        StateMatch = MatchState.SetupComplete;
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