using Microsoft.Xna.Framework.Input;

public class InputSystem
{
    private KeyboardState _previousKeyboardState;

    public void Update()
    {
        _previousKeyboardState = Keyboard.GetState();
    }

    public bool IsKeyDown(Keys key)
    {
        return Keyboard.GetState().IsKeyDown(key);
    }

    public bool IsKeyPressed(Keys key)
    {
        var currentState = Keyboard.GetState();
        return currentState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
    }
}