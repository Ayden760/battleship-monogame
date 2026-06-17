namespace BattleShip.GameData
{
    public enum FieldState
    {

        Ship,
        Hit,
        Miss
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
    public enum MatchState
    {
        SetupPlayer1,
        SetupPlayer2,
        SetupAI,
        SetupComplete,
        PlayerTurn,
        TurnTransition,
        GameOver
    }
}