// File: Systems/PlayerController.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class PlayerController
{
    private Entity _player;
    private InputSystem _inputSystem;
    private int _speed;

    public PlayerController(Entity player, InputSystem inputSystem, int speed)
    {
        _player = player;
        _inputSystem = inputSystem;
        _speed = speed;
    }

    public void Update()
    {
        int xSpeed = 0;
        int ySpeed = 0;

        if (_inputSystem.IsKeyDown(Keys.Up))
            ySpeed = -_speed;
        if (_inputSystem.IsKeyDown(Keys.Down))
            ySpeed = _speed;
        if (_inputSystem.IsKeyDown(Keys.Left))
            xSpeed = -_speed;
        if (_inputSystem.IsKeyDown(Keys.Right))
            xSpeed = _speed;
        if (_inputSystem.IsKeyDown(Keys.D0))
        {
            Animation anim = _player.GetComponent<Animation>();
            anim.Row = 0;
            anim.FrameDuration = 0.2f;
        }
        if (_inputSystem.IsKeyDown(Keys.D3))
        {
            Animation anim = _player.GetComponent<Animation>();
            anim.Row = 3;
            anim.FrameDuration = 0.1f;
        }

        var position = _player.GetComponent<Position>();
        position.X += xSpeed;
        position.Y += ySpeed;

        _player.AddComponent(position);
    }
}