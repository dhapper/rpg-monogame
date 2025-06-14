using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

public static class InputSystem
{
    private static KeyboardState _previousKeyboardState;
    private static MouseState _previousMouseState;

    private static KeyboardState _currentKeyboardState;
    private static MouseState _currentMouseState;

    private static Dictionary<MouseButton, MouseDragState> _dragStates = new();

    private static int currentSelectedNumber = 0;
    private static bool lastKeyBasedswitching = false;
    private static int prevDirection = 0;

    static InputSystem()
    {
        foreach (MouseButton button in Enum.GetValues(typeof(MouseButton)))
        {
            _dragStates[button] = new MouseDragState();
        }
    }

    public static void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _previousMouseState = _currentMouseState;

        _currentKeyboardState = Keyboard.GetState();
        _currentMouseState = Mouse.GetState();

        UpdateDrag();

        var inputState = GetInputState();
        if (inputState.IsNumberChanging && inputState.Number.HasValue)
            currentSelectedNumber = inputState.Number.Value;

    }

    public static void UpdateDrag()
    {
        foreach (var pair in _dragStates)
        {
            var button = pair.Key;
            var dragState = pair.Value;

            bool wasPressed = IsMouseButtonDown(_previousMouseState, button);
            bool isPressed = IsMouseButtonDown(_currentMouseState, button);

            var currentPos = _currentMouseState.Position;

            dragState.DragStarted = false;
            dragState.DragEnded = false;

            if (!wasPressed && isPressed)
            {
                // Drag started
                dragState.IsDragging = true;
                dragState.DragStarted = true;
                dragState.StartPosition = currentPos;
            }
            else if (wasPressed && isPressed && dragState.IsDragging)
            {
                // Drag in progress
                dragState.CurrentPosition = currentPos;
            }
            else if (wasPressed && !isPressed && dragState.IsDragging)
            {
                // Drag ended
                dragState.DragEnded = true;
                dragState.IsDragging = false;
            }
        }
    }

    public static InputState GetInputState()
    {
        var state = new InputState();

        state.MoveUp = IsKeyDown(Keys.Up) || IsKeyDown(Keys.W);
        state.MoveDown = IsKeyDown(Keys.Down) || IsKeyDown(Keys.S);
        state.MoveLeft = IsKeyDown(Keys.Left) || IsKeyDown(Keys.A);
        state.MoveRight = IsKeyDown(Keys.Right) || IsKeyDown(Keys.D);

        state.ToggleHitbox = IsKeyPressed(Keys.H);
        state.ToggleInventory = IsKeyPressed(Keys.I);
        state.Grow = IsKeyPressed(Keys.G);

        state.Save = IsKeyPressed(Keys.O);
        state.Enter = IsKeyPressed(Keys.Enter);
        state.Interact = IsMousePressed(MouseButton.Left);

        HandleHotbarState(state);

        return state;
    }

    public static void HandleHotbarState(InputState state)
    {
        state.IsNumberChanging = false;
        for (int i = 0; i < 9; i++)
        {
            if (IsKeyPressed(Keys.D0 + i))
            {
                state.Number = i - 1;
                state.IsNumberChanging = true;
                lastKeyBasedswitching = true;
                break;
            }
        }

        // Check scroll wheel input
        int scrollDelta = _currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
        if (scrollDelta != 0)
        {
            int direction = Math.Sign(scrollDelta);
            int newNumber = currentSelectedNumber;
            int difference = direction + prevDirection;

            if (!lastKeyBasedswitching)
            {
                if (difference == 0)
                {
                    if (direction == 1) newNumber--;
                    if (direction == -1) newNumber++;
                }

                if (difference == 2) newNumber++;
                else if (difference == -2) newNumber--;
            }

            if (newNumber < 0) newNumber = 0;
            if (newNumber > 8) newNumber = 8;

            state.Number = newNumber;
            state.IsNumberChanging = true;
            lastKeyBasedswitching = false;
            prevDirection = direction;
        }
    }


    // calls continously upon key hold
    public static bool IsKeyDown(Keys key)
    {
        return _currentKeyboardState.IsKeyDown(key);
    }

    // calls once upon initial key press
    public static bool IsKeyPressed(Keys key)
    {
        return _currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
    }

    public static bool IsMousePressed(MouseButton button)
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

    private static bool IsMouseButtonDown(MouseState state, MouseButton button)
    {
        return button switch
        {
            MouseButton.Left => state.LeftButton == ButtonState.Pressed,
            MouseButton.Right => state.RightButton == ButtonState.Pressed,
            MouseButton.Middle => state.MiddleButton == ButtonState.Pressed,
            MouseButton.XButton1 => state.XButton1 == ButtonState.Pressed,
            MouseButton.XButton2 => state.XButton2 == ButtonState.Pressed,
            _ => false,
        };
    }

    public static MouseDragState GetMouseDragState(MouseButton button)
    {
        return _dragStates[button];
    }



    public static (int x, int y) GetMouseLocation()
    {
        return (_currentMouseState.Position.X, _currentMouseState.Position.Y);
    }

    public static (int x, int y) GetMouseLocationRelativeCamera(Camera2D camera)
    {
        int x = (int)(_currentMouseState.Position.X + camera.Position.X);
        int y = (int)(_currentMouseState.Position.Y + camera.Position.Y);
        return (x, y);
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