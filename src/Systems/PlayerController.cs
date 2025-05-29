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

    private int _speed = 4;
    private List<Entity> _entities;

    private bool[] dir;
    private int lastDir = -1;

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
        resetDirVars();
        bool isMoving = false;

        var anim = _player.GetComponent<AnimationComponent>();

        if (_inputSystem.IsKeyDown(Keys.Up) || _inputSystem.IsKeyDown(Keys.W))
        {
            _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkUp);
            dir[Constants.Directions.Up] = true;
            lastDir = Constants.Directions.Up;
            isMoving = true;
        }
        if (_inputSystem.IsKeyDown(Keys.Down) || _inputSystem.IsKeyDown(Keys.S))
        {
            _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkDown);
            dir[Constants.Directions.Down] = true;
            lastDir = Constants.Directions.Down;
            isMoving = true;
        }
        if (_inputSystem.IsKeyDown(Keys.Left) || _inputSystem.IsKeyDown(Keys.A))
        {
            _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkLeft);
            dir[Constants.Directions.Left] = true;
            lastDir = Constants.Directions.Left;
            isMoving = true;
        }
        if (_inputSystem.IsKeyDown(Keys.Right) || _inputSystem.IsKeyDown(Keys.D))
        {
            _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkLeft);
            dir[Constants.Directions.Right] = true;
            lastDir = Constants.Directions.Right;
            isMoving = true;
        }

        if (!isMoving && lastDir != -1)
        {
            if (lastDir == Constants.Directions.Up)
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleUp);
            else if (lastDir == Constants.Directions.Left)
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleLeft);
            else if (lastDir == Constants.Directions.Right)
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