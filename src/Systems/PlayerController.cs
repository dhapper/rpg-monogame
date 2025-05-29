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

    // private int _speed = 4;
    private List<Entity> _entities;

    private bool[] dir;
    // private int lastDir = -1;

    public PlayerController(Entity player, InputSystem inputSystem, AnimationSystem animationSystem, List<Entity> entities)
    {
        _player = player;
        _inputSystem = inputSystem;
        _entities = entities;
        _collisionSystem = new CollisionSystem(_player, _entities);
        _movementSystem = new MovementSystem();
        _animationSystem = animationSystem;

        dir = [false, false, false, false];
    }

    public void Update()
    {
        ResetDirVars();

        var anim = _player.GetComponent<AnimationComponent>();
        var movement = _player.GetComponent<MovementComponent>();

        movement.IsMoving = false;

        if (_inputSystem.IsKeyDown(Keys.Up) || _inputSystem.IsKeyDown(Keys.W))
            InitAnimation(Constants.Directions.Up, Constants.Player.Animations.WalkUp);
        if (_inputSystem.IsKeyDown(Keys.Down) || _inputSystem.IsKeyDown(Keys.S))
            InitAnimation(Constants.Directions.Down, Constants.Player.Animations.WalkDown);
        if (_inputSystem.IsKeyDown(Keys.Left) || _inputSystem.IsKeyDown(Keys.A))
            InitAnimation(Constants.Directions.Left, Constants.Player.Animations.WalkLeft);
        if (_inputSystem.IsKeyDown(Keys.Right) || _inputSystem.IsKeyDown(Keys.D))
            InitAnimation(Constants.Directions.Right, Constants.Player.Animations.WalkLeft);

        if (!movement.IsMoving && movement.LastDir != -1)
        {
            if (movement.LastDir == Constants.Directions.Up)
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleUp);
            else if (movement.LastDir == Constants.Directions.Left)
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleLeft);
            else if (movement.LastDir == Constants.Directions.Right)
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleLeft);
            else
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleDown);
        }

        // if (_inputSystem.IsKeyDown(Keys.D1))
        //     _player.GetComponent<AnimationComponent>().UpdateAnimation(Constants.Player.Animations.IdleDown);
        // if (_inputSystem.IsKeyDown(Keys.D2))
        //     _player.GetComponent<AnimationComponent>().UpdateAnimation(Constants.Player.Animations.WalkDown);
        // if (_inputSystem.IsKeyDown(Keys.D3))
        //     _player.GetComponent<AnimationComponent>().UpdateAnimation(Constants.Player.Animations.RunDown);
        // if (_inputSystem.IsKeyDown(Keys.D4))
        //     _player.GetComponent<AnimationComponent>().UpdateAnimation(Constants.Player.Animations.AxeLeft);

        Vector2 speedVector = _movementSystem.CalculateSpeed(movement.Speed, dir);
        _collisionSystem.Move(speedVector.X, speedVector.Y);
    }

    public void InitAnimation(int direction, AnimationConfig config)
    {
        var movement = _player.GetComponent<MovementComponent>();
        _animationSystem.SetAnimation(_player, config);
        dir[direction] = true;
        movement.LastDir = direction;
        movement.IsMoving = true;
    }

    private void ResetDirVars()
    {
        dir[Constants.Directions.Up] = false;
        dir[Constants.Directions.Down] = false;
        dir[Constants.Directions.Left] = false;
        dir[Constants.Directions.Right] = false;
    }

}