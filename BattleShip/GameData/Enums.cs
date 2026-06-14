namespace BattleShip.GameData
{
    public enum FieldState
    {

        Ship,
        Hit,
        Miss
    }
    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
    public enum ShipType
    {
        Two,
        Three,
        Four,
        Five
    }
    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }
}