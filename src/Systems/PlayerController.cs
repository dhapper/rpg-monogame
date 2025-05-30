using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class PlayerController
{
    private Entity _player;
    private InputSystem _inputSystem;
    private CollisionSystem _collisionSystem;
    private MovementSystem _movementSystem;
    private AnimationSystem _animationSystem;
    private List<Entity> _entities;
    private bool[] dir = [false, false, false, false];

    public PlayerController(Entity player, InputSystem inputSystem, AnimationSystem animationSystem, List<Entity> entities)
    {
        _player = player;
        _inputSystem = inputSystem;
        _entities = entities;
        _collisionSystem = new CollisionSystem(_player, _entities);
        _movementSystem = new MovementSystem();
        _animationSystem = animationSystem;
    }

    public void Update()
    {
        ResetDirVars();

        var anim = _player.GetComponent<AnimationComponent>();
        var movement = _player.GetComponent<MovementComponent>();

        movement.IsMoving = false;

        bool UpKeyPressed = _inputSystem.IsKeyDown(Keys.Up) || _inputSystem.IsKeyDown(Keys.W);
        bool DownKeyPressed = _inputSystem.IsKeyDown(Keys.Down) || _inputSystem.IsKeyDown(Keys.S);
        bool LeftKeyPressed = _inputSystem.IsKeyDown(Keys.Left) || _inputSystem.IsKeyDown(Keys.A);
        bool RightKeyPressed = _inputSystem.IsKeyDown(Keys.Right) || _inputSystem.IsKeyDown(Keys.D);

        // animation
        if (!LeftKeyPressed && !RightKeyPressed)
        {
            if (UpKeyPressed)
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkUp);
            if (DownKeyPressed)
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkDown);
        }
        if (LeftKeyPressed)
            _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkLeft);
        if (RightKeyPressed)
            _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkRight);

        // movement
        if (UpKeyPressed)
            InitMovement(Constants.Directions.Up);
        if (DownKeyPressed)
            InitMovement(Constants.Directions.Down);
        if (LeftKeyPressed)
            InitMovement(Constants.Directions.Left);
        if (RightKeyPressed)
            InitMovement(Constants.Directions.Right);


        // default idle
        if (!movement.IsMoving && movement.LastDir != -1)
        {
            if (movement.LastDir == Constants.Directions.Up)
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleUp);
            else if (movement.LastDir == Constants.Directions.Left)
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleLeft);
            else if (movement.LastDir == Constants.Directions.Right)
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleRight);
            else
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleDown);
        }

        Vector2 speedVector = _movementSystem.CalculateSpeed(movement.Speed, dir);
        _collisionSystem.Move(speedVector.X, speedVector.Y);
    }

    public void InitMovement(int direction)
    {
        var movement = _player.GetComponent<MovementComponent>();
        movement.LastDir = direction;
        movement.IsMoving = true;
        dir[direction] = true;
    }

    private void ResetDirVars()
    {
        dir[Constants.Directions.Up] = false;
        dir[Constants.Directions.Down] = false;
        dir[Constants.Directions.Left] = false;
        dir[Constants.Directions.Right] = false;
    }

}