using System.Collections.Generic;
using BattleShip.GameData;
using BattleShip.Functions;
using BattleShip.Services;
namespace BattleShip.GameObjects;

using BattleShip.UI;


public class ShipSetter
{

    private List<ShipBase> _shipBases = new List<ShipBase>();

    private bool AllShipsSet = false;

    private int Selectionlocation;
    public bool Ship_Selected { get; set; }



    public int CurrentShip_Length { get; set; }
    public bool Set_Mode { get; set; }

    public int Two_tile { get; private set; }
    public int Three_tile { get; private set; }
    public int Four_tile { get; private set; }
    public int Five_tile { get; private set; }

    private readonly GameSettings _settings;
    private readonly InputHandler _inputHandler;
    private GameSession _session;
    private AiShipSetter _aiShipSetter;
    private GameValidations _validations;
    private readonly ShipPlacer _shipPlacer;

    public ShipSetter(GameSettings settings, InputHandler handler, GameSession session, AiShipSetter aiShipSetter, GameValidations validations, ShipPlacer shipPlacer)
    {
        _settings = settings;
        _inputHandler = handler;
        _session = session;
        _aiShipSetter = aiShipSetter;
        _validations = validations;
        _shipPlacer = shipPlacer;
    }



    public void CheckAllSet()
    {
        bool allset = true;

        if (Two_tile > 0 || Three_tile > 0 || Four_tile > 0 || Five_tile > 0)
        {
            allset = false;
        }

        if (allset)
            AllShipsSet = true;


    }
    public void InitializeFromSettings(GameSettings settings)
    {
        _shipBases.Clear();

        AllShipsSet = false;
        Ship_Selected = false;



        Two_tile = settings.Two_tile;
        Three_tile = settings.Three_tile;
        Four_tile = settings.Four_tile;
        Five_tile = settings.Five_tile;

    }
    public void Reset_And_Set_PlayerField()
    {

        if (_session.CurrentPlayer == _session.Player1)
        {
            _session.Player1.Set_Own_Ships(_shipBases, AllShipsSet);
            InitializeFromSettings(_settings);
        }
        else if (_session.CurrentPlayer == _session.Player2)
        {
            _session.Player2.Set_Own_Ships(_shipBases, AllShipsSet);
        }
    }
    public void Select_CurrentShip(int y, int x)
    {
        if (Set_Mode)
        {

            //set ships new
            Set_Mode = false;
            ShipBase shipBase = new ShipBase(CurrentShip_Length);
            bool Can_Set = _shipPlacer.PlaceShip(y, x, ref shipBase, _shipBases, CurrentShip_Length);
            if (Can_Set)
            {
                bool NotAlreadySet = true;


                if (CurrentShip_Length == 2 && Two_tile > 0)
                {
                    NotAlreadySet = false;
                    Two_tile--;
                }
                else if (CurrentShip_Length == 3 && Three_tile > 0)
                {
                    NotAlreadySet = false;
                    Three_tile--;
                }
                else if (CurrentShip_Length == 4 && Four_tile > 0)
                {
                    NotAlreadySet = false;
                    Four_tile--;
                }
                else if (CurrentShip_Length == 5 && Five_tile > 0)
                {
                    NotAlreadySet = false;
                    Five_tile--;
                }
                if (!NotAlreadySet)
                {
                    shipBase.IsPlaced = true;
                    _shipBases.Add(shipBase);
                }
            }
            else
            {
                //could not place ship, show message?
            }
        }

        else
        {

            var (found, location) = _validations.IsThereShip(_shipBases, x, y);


            if (found)
            {
                Selectionlocation = location;
                Ship_Selected = found;

                MoveShip();
            }


        }
    }

    public void DrawShips()
    {
        FieldRenderer.DrawShips(_shipBases);
    }
    public void MoveShip()
    {

        List<Cell> newCells = new List<Cell>();
        List<ShipBase> newShipBase = new List<ShipBase>(_shipBases);
        newShipBase.RemoveAt(Selectionlocation);

        if (_inputHandler.MoveUp())
        {

            newCells = ShipMover.MoveUp(_shipBases[Selectionlocation].Location);
        }
        if (_inputHandler.MoveDown())
        {
            newCells = ShipMover.MoveDown(_shipBases[Selectionlocation].Location);
        }
        if (_inputHandler.MoveLeft())
        {
            newCells = ShipMover.MoveLeft(_shipBases[Selectionlocation].Location);
        }
        if (_inputHandler.MoveRight())
        {
            newCells = ShipMover.MoveRight(_shipBases[Selectionlocation].Location);
        }
        if (_inputHandler.RotateShip())
        {
            newCells = ShipMover.Rotate(_shipBases[Selectionlocation].Location);
        }
        if (newCells.Count > 0)
        {
            if (_validations.CanPlaceShip(newShipBase, newCells))
            {
                _shipBases[Selectionlocation].Location = newCells;

            }
        }

    }
    public void Check_Confirm()
    {
        if (AllShipsSet)
        {

            Reset_And_Set_PlayerField();
        }
    }

    public void GenerateShipsForCurrentPlayer()
    {
        _shipBases.Clear();
        _shipBases = _aiShipSetter.PlaceAllShipsRandomly();
        Two_tile = 0;
        Three_tile = 0;
        Four_tile = 0;
        Five_tile = 0;

    }





}
