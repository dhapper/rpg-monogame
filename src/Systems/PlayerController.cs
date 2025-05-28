// File: Systems/PlayerController.cs
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class PlayerController
{
    private Entity _player;
    private InputSystem _inputSystem;
    private int _speed;

    // private TileMap _map;
    // private Tile[] _tiles;
    private List<Entity> _entities;

    public PlayerController(Entity player, InputSystem inputSystem, int speed, List<Entity> entities)
    {
        _player = player;
        _inputSystem = inputSystem;
        _speed = speed;
        _entities = entities;

        // _map = map;
        // _tiles = tiles;
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
        float newX = position.X + xSpeed;
        float newY = position.Y + ySpeed;
        Rectangle newHitbox = new Rectangle(
            (int)newX,
            (int)newY,
            (int)(position.Width * Constants.ScaleFactor),
            (int)(position.Height * Constants.ScaleFactor));
        if (!IsSolid(newHitbox))
        {
            position.X = newX;
            position.Y = newY;
            var collision = _player.GetComponent<Collision>();
            collision.UpdateHitbox(position);
        }

        _player.AddComponent(position);
    }

    private bool IsSolid(Rectangle hitbox)
    {
        foreach (var entity in _entities)
        {
            if(entity.Equals(_player)) { continue; }

            if (entity.HasComponent<Collision>() && entity.GetComponent<Collision>().IsSolid)
                if (hitbox.Intersects(entity.GetComponent<Collision>().Hitbox))
                {
                    return true;
                }
        }
        return false;
    }
}