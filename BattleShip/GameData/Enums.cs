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
    enum AIState
    {
        Searching,
        FoundHit,
        Targeting,
        DoneShip
    }
}