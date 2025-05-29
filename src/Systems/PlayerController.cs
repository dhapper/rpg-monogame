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
    private int lastDir;

    public PlayerController(Entity player, InputSystem inputSystem, AnimationSystem animationSystem, List<Entity> entities)
    {
        _player = player;
        _inputSystem = inputSystem;
        _animationSystem = animationSystem;
        _entities = entities;
        _collisionSystem = new CollisionSystem(_player, _entities);
        _movementSystem = new MovementSystem();

        dir = [false, false, false, false];
    }

    private bool firstFrame;

    public void Update()
    {
        resetDirVars();
        bool isMoving = false;

        var anim = _player.GetComponent<AnimationComponent>();

        if (_inputSystem.IsKeyDown(Keys.Up) || _inputSystem.IsKeyDown(Keys.W))
        {
            // anim.UpdateAnimation(Constants.Player.Animations.WalkUp);
            _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkUp);
            dir[Constants.Directions.Up] = true;
            lastDir = Constants.Directions.Up;
            isMoving = true;
        }
        if (_inputSystem.IsKeyDown(Keys.Down) || _inputSystem.IsKeyDown(Keys.S))
        {
            _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkUp);
            dir[Constants.Directions.Down] = true;
            lastDir = Constants.Directions.Down;
            isMoving = true;
        }
        if (_inputSystem.IsKeyDown(Keys.Left) || _inputSystem.IsKeyDown(Keys.A))
        {
            _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkUp);
            dir[Constants.Directions.Left] = true;
            lastDir = Constants.Directions.Left;
            isMoving = true;
        }
        if (_inputSystem.IsKeyDown(Keys.Right) || _inputSystem.IsKeyDown(Keys.D))
        {
            _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkUp);
            dir[Constants.Directions.Right] = true;
            lastDir = Constants.Directions.Right;
            isMoving = true;
        }

        if (!isMoving)
        {
            _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleDown);
        }

        // if (isMoving)
        // {
        //     if (dir[Constants.Directions.Up])
        //         anim.UpdateAnimation(Constants.Player.Animations.WalkUp);
        //     else if (dir[Constants.Directions.Down])
        //         anim.UpdateAnimation(Constants.Player.Animations.WalkDown);
        //     else if (dir[Constants.Directions.Left])
        //         anim.UpdateAnimation(Constants.Player.Animations.WalkLeft);
        //     else if (dir[Constants.Directions.Right])
        //         anim.UpdateAnimation(Constants.Player.Animations.WalkLeft);
        // }

        // if (!_movementSystem.Moving)
        // {
        //     if (lastDir == Constants.Directions.Up)
        //         _player.GetComponent<AnimationComponent>().UpdateAnimation(Constants.Player.Animations.IdleUp);
        //     else if (lastDir == Constants.Directions.Left)
        //         _player.GetComponent<AnimationComponent>().UpdateAnimation(Constants.Player.Animations.IdleLeft);
        //     else if (lastDir == Constants.Directions.Right)
        //         _player.GetComponent<AnimationComponent>().UpdateAnimation(Constants.Player.Animations.IdleLeft);
        //     else
        //         _player.GetComponent<AnimationComponent>().UpdateAnimation(Constants.Player.Animations.IdleDown);
        // }




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