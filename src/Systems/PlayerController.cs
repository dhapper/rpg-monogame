// File: Systems/PlayerController.cs
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class PlayerController
{
    private Entity _player;
    private InputSystem _inputSystem;
    private int _speed;

    // private TileMap _map;
    // private Tile[] _tiles;

    public PlayerController(Entity player, InputSystem inputSystem, int speed)
    {
        _player = player;
        _inputSystem = inputSystem;
        _speed = speed;

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
        Rectangle newHitbox = new Rectangle((int)newX, (int)newY, position.Width, position.Height);
        // if (!IsSolid(newHitbox))
        // {
            position.X = newX;
            position.Y = newY;
        // }

        _player.AddComponent(position);
    }

    // private bool IsSolid(Rectangle hitbox)
    // {
    //     int leftTile = hitbox.Left / _map.TileWidth;
    //     int rightTile = hitbox.Right / _map.TileWidth;
    //     int topTile = hitbox.Top / _map.TileHeight;
    //     int bottomTile = hitbox.Bottom / _map.TileHeight;

    //     for (int y = topTile; y <= bottomTile; y++)
    //     {
    //         for (int x = leftTile; x <= rightTile; x++)
    //         {
    //             if (x < 0 || y < 0 || x >= _map.Width || y >= _map.Height) continue;

    //             int tileId = _map.Tiles[x, y];
    //             if (tileId < 0 || tileId >= _tiles.Length) continue;

    //             if (_tiles[tileId].IsSolid)
    //                 return true;
    //         }
    //     }

    //     return false;
    // }
}