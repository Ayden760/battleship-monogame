using BattleShip.GameObjects;
namespace BattleShip.GameData;

using Data = GameData;
public class GameSession
{
    public AI Ai;
    public Player Player1;
    public Player Player2;

    public Player CurrentPlayer { get; set; }
    public Player OldPlayer { get; set; }

    //Game Options

    public GameSession()
    {
        if (Data.Settings == null)
        {
            throw new System.Exception("fdfdf");
        }
        int Rows = Data.Settings.Rows;
        int Columns = Data.Settings.Columns;
        //if für entweder player oder ki

        Player1 = new Player(Rows, Columns, "Player1");

        if (!Data.Settings.Ai_Mode)
        {
            Player2 = new Player(Rows, Columns, "Player2");
            Ai = null;
        }
        else
        {
            Ai = new AI(Rows, Columns, "AI_1");
            Player2 = null;
        }






    }
}