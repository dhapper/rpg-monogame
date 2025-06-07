using Microsoft.Xna.Framework.Input;

public class InputSystem
{
    private KeyboardState _previousKeyboardState;
    private MouseState _previousMouseState;

    private KeyboardState _currentKeyboardState;
    private MouseState _currentMouseState;

    public void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _previousMouseState = _currentMouseState;

        _currentKeyboardState = Keyboard.GetState();
        _currentMouseState = Mouse.GetState();
    }

    public InputState GetInputState()
    {
        var state = new InputState();

        state.MoveUp = IsKeyDown(Keys.Up) || IsKeyDown(Keys.W);
        state.MoveDown = IsKeyDown(Keys.Down) || IsKeyDown(Keys.S);
        state.MoveLeft = IsKeyDown(Keys.Left) || IsKeyDown(Keys.A);
        state.MoveRight = IsKeyDown(Keys.Right) || IsKeyDown(Keys.D);
        state.ToggleHitbox = IsKeyPressed(Keys.H);
        state.Save = IsKeyDown(Keys.O);
        state.Interact = IsMousePressed(MouseButton.Left);

        return state;
    }

    public bool IsKeyDown(Keys key)
    {
        return _currentKeyboardState.IsKeyDown(key);
    }

    public bool IsKeyPressed(Keys key)
    {
        return _currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
    }

    public bool IsMousePressed(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                return _currentMouseState.LeftButton == ButtonState.Pressed &&
                       _previousMouseState.LeftButton == ButtonState.Released;
            case MouseButton.Right:
                return _currentMouseState.RightButton == ButtonState.Pressed &&
                       _previousMouseState.RightButton == ButtonState.Released;
            case MouseButton.Middle:
                return _currentMouseState.MiddleButton == ButtonState.Pressed &&
                       _previousMouseState.MiddleButton == ButtonState.Released;
            case MouseButton.XButton1:
                return _currentMouseState.XButton1 == ButtonState.Pressed &&
                       _previousMouseState.XButton1 == ButtonState.Released;
            case MouseButton.XButton2:
                return _currentMouseState.XButton2 == ButtonState.Pressed &&
                       _previousMouseState.XButton2 == ButtonState.Released;
            default:
                return false;
        }
    }

    public (int x, int y) GetMouseLocation()
    {
        return (_currentMouseState.Position.X, _currentMouseState.Position.Y);
    }

    public enum MouseButton
    {
        Left,
        Right,
        Middle,
        XButton1,
        XButton2
    }
}