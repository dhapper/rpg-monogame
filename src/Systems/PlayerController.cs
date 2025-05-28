using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class PlayerController
{
    private Entity _player;

    private InputSystem _inputSystem;
    private CollisionSystem _collisionSystem;
    private MovementSystem _movementSystem;

    private int _speed = 4;
    private List<Entity> _entities;

    private bool[] dir;

    public PlayerController(Entity player, InputSystem inputSystem, List<Entity> entities)
    {
        _player = player;
        _inputSystem = inputSystem;
        _entities = entities;
        _collisionSystem = new CollisionSystem(_player, _entities);
        _movementSystem = new MovementSystem();

        dir = [false, false, false, false];
    }

    public void Update()
    {
        resetDirVars();

        if (_inputSystem.IsKeyDown(Keys.Up) || _inputSystem.IsKeyDown(Keys.W))
            dir[Constants.Directions.Up] = true;

        if (_inputSystem.IsKeyDown(Keys.Down) || _inputSystem.IsKeyDown(Keys.S))
            dir[Constants.Directions.Down] = true;

        if (_inputSystem.IsKeyDown(Keys.Left) || _inputSystem.IsKeyDown(Keys.A))
            dir[Constants.Directions.Left] = true;

        if (_inputSystem.IsKeyDown(Keys.Right) || _inputSystem.IsKeyDown(Keys.D))
            dir[Constants.Directions.Right] = true;

        if (_inputSystem.IsKeyDown(Keys.D0))
        {
            AnimationComponent anim = _player.GetComponent<AnimationComponent>();
            anim.Row = 0;
            anim.FrameDuration = 0.2f;
        }
        if (_inputSystem.IsKeyDown(Keys.D3))
        {
            AnimationComponent anim = _player.GetComponent<AnimationComponent>();
            anim.Row = 3;
            anim.FrameDuration = 0.1f;
        }

        Vector2 speedVector = _movementSystem.CalculateSpeed(_speed, dir);
        _collisionSystem.Move(speedVector.X, speedVector.Y);
    }

    private void resetDirVars()
    {
        dir[Constants.Directions.Up] = false;
        dir[Constants.Directions.Down] = false;
        dir[Constants.Directions.Left] = false;
        dir[Constants.Directions.Right] = false;
    }

}