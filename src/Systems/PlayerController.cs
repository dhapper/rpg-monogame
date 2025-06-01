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
    private bool facingRight = true;
    private Camera2D _camera;

    public PlayerController(Entity player, InputSystem inputSystem, AnimationSystem animationSystem, List<Entity> entities, Camera2D camera)
    {
        _player = player;
        _inputSystem = inputSystem;
        _entities = entities;
        _collisionSystem = new CollisionSystem(_player, _entities);
        _movementSystem = new MovementSystem();
        _animationSystem = animationSystem;
        _camera = camera;
    }

    public void Update()
    {
        ResetDirVars();
        var movement = _player.GetComponent<MovementComponent>();
        movement.IsMoving = false;

        bool UpKeyPressed = _inputSystem.IsKeyDown(Keys.Up) || _inputSystem.IsKeyDown(Keys.W);
        bool DownKeyPressed = _inputSystem.IsKeyDown(Keys.Down) || _inputSystem.IsKeyDown(Keys.S);
        bool LeftKeyPressed = _inputSystem.IsKeyDown(Keys.Left) || _inputSystem.IsKeyDown(Keys.A);
        bool RightKeyPressed = _inputSystem.IsKeyDown(Keys.Right) || _inputSystem.IsKeyDown(Keys.D);

        // movement
        if (UpKeyPressed && !DownKeyPressed)
            InitMovement(Constants.Directions.Up);
        if (DownKeyPressed && !UpKeyPressed)
            InitMovement(Constants.Directions.Down);
        if (LeftKeyPressed && !RightKeyPressed)
            InitMovement(Constants.Directions.Left);
        if (RightKeyPressed && !LeftKeyPressed)
            InitMovement(Constants.Directions.Right);

        if (_inputSystem.IsKeyDown(Keys.H))
            GameInitializer.ShowHitbox = !GameInitializer.ShowHitbox;

        Vector2 speedVector = _movementSystem.CalculateSpeed(movement.Speed, dir);
        _collisionSystem.Move(speedVector.X, speedVector.Y, _camera.WorldWidthInPixels, _camera.WorldHeightInPixels);
        // int worldWidth, int worldHeight

        // walking animation
        // if (!LeftKeyPressed && !RightKeyPressed)
        // {
        //     if (UpKeyPressed)
        //         _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkUp);
        //     if (DownKeyPressed)
        //         _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkDown);
        // }
        // if (LeftKeyPressed)
        //     _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkLeft);
        // if (RightKeyPressed)
        //     _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkRight);

        // if (!LeftKeyPressed && !RightKeyPressed)
        // {
            if (LeftKeyPressed)
            {
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkLeft);
                facingRight = false;
            }
            if (RightKeyPressed)
            {
                _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkRight);
                facingRight = true;
            }
        // }
        // if (!UpKeyPressed && !DownKeyPressed)
        // {
            if (UpKeyPressed || DownKeyPressed) {
                _animationSystem.SetAnimation(_player, facingRight ? Constants.Player.Animations.WalkRight : Constants.Player.Animations.WalkLeft);
            }
        // }

        // default idle
        // if ((!movement.IsMoving && movement.LastDir != -1) || (speedVector.X == 0 && speedVector.Y == 0))
        // {
        //     if (movement.LastDir == Constants.Directions.Up)
        //         _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleUp);
        //     else if (movement.LastDir == Constants.Directions.Left)
        //         _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleLeft);
        //     else if (movement.LastDir == Constants.Directions.Right)
        //         _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleRight);
        //     else
        //         _animationSystem.SetAnimation(_player, Constants.Player.Animations.IdleDown);
        // }

        if (!movement.IsMoving || (speedVector.X == 0 && speedVector.Y == 0))
        {
            _animationSystem.SetAnimation(_player, facingRight ? Constants.Player.Animations.IdleRight : Constants.Player.Animations.IdleLeft);
        }


        var pos = _player.GetComponent<PositionComponent>();
        // int cameraX = (int)pos.X - (Constants.Player.XOffset + Constants.Player.HitboxWidth/2);
        // int cameraY = (int)pos.Y - (Constants.Player.YOffset + Constants.Player.HitboxHeight/2);
        // int cameraX = (int)pos.X + 100;
        // int cameraY = (int)pos.Y + 100;
        int cameraX = (int)(pos.X + Constants.ScaleFactor * (Constants.Player.XOffset + Constants.Player.HitboxWidth / 2));
        int cameraY = (int)(pos.Y + Constants.ScaleFactor * (Constants.Player.YOffset + Constants.Player.HitboxHeight / 2));
        _camera.Follow(new Vector2(cameraX, cameraY));



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