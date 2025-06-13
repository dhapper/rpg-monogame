public static class GameStateManager
{
    private static GameState _currentGameState = GameState.Playing;

    public static GameState CurrentGameState => _currentGameState;

    public static void SetState(GameState newState)
    {
        if (_currentGameState != newState)
            _currentGameState = newState;
    }

    public static void ToggleBetweenStates(GameState state1, GameState state2)
    {
        _currentGameState = _currentGameState == state1 ? state2 : state1;
    }
}

public enum GameState
{
    Playing,
    Paused,
    Inventory,
    DialogueBox,
}