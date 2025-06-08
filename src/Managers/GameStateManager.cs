public class GameStateManager
{
    private GameState _currentGameState = GameState.Playing;

    public GameState CurrentGameState => _currentGameState;

    public void SetState(GameState newState)
    {
        if (_currentGameState != newState)
            _currentGameState = newState;
    }

    public void ToggleBetweenStates(GameState state1, GameState state2)
    {
        _currentGameState = _currentGameState == state1 ? state2 : state1;
    }
}

public enum GameState
{
    Playing,
    Paused,
    Inventory
}