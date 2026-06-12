using BattleShip.GameObjects;
namespace BattleShip.GameData;

using Data = GameData;
public class GameShip
{
    public KI Ki;
    public Player Player1;
    public Player Player2;

    public Player CurrentPlayer { get; set; }
    public Player OldPlayer { get; set; }

    //Game Options

    public GameShip()
    {
        int Rows = Data.Settings.Rows;
        int Columns = Data.Settings.Columns;
        //if für entweder player oder ki
        Ki = new KI(Rows, Columns);
        Player1 = new Player(Rows, Columns, "Player1");

        Player2 = new Player(Rows, Columns, "Player2");





    }
}