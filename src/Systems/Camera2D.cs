using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Camera2D
{
    public Vector2 Position { get; private set; }
    public Matrix Transform { get; private set; }

    private Viewport _viewport;

    private int _worldWidth;
    private int _worldHeight;
    public int WorldWidthInPixels => _worldWidth;
    public int WorldHeightInPixels => _worldHeight;

    public Camera2D(Viewport viewport)
    {
        _viewport = viewport;
        Position = Vector2.Zero;
        UpdateTransform();
    }

    public void SetWorldBounds(int width, int height)
    {
        _worldWidth = width;
        _worldHeight = height;
    }

    public void Follow(Vector2 target)
    {
        bool smallWorldWidth = _worldWidth <= _viewport.Width;
        bool smallWorldHeight = _worldHeight <= _viewport.Height;

        float newX, newY;

        if (smallWorldWidth)
        {
            newX = (_worldWidth - _viewport.Width) / 2f;
        }
        else
        {
            newX = target.X - _viewport.Width / 2f;
            newX = MathHelper.Clamp(newX, 0, _worldWidth - _viewport.Width);
        }

        if (smallWorldHeight)
        {
            newY = (_worldHeight - _viewport.Height) / 2f;
        }
        else
        {
            newY = target.Y - _viewport.Height / 2f;
            newY = MathHelper.Clamp(newY, 0, _worldHeight - _viewport.Height);
        }

        Position = new Vector2(newX, newY);
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        // Round to pixel to avoid jitter
        Vector2 pixelPosition = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
        Transform = Matrix.CreateTranslation(new Vector3(-pixelPosition, 0));
    }

    public Rectangle GetViewRectangle()
    {
        int width = _viewport.Width + Constants.TileSize;
        int height = _viewport.Height + Constants.TileSize;
        int left = (int)Position.X - Constants.TileSize;
        int top = (int)Position.Y - Constants.TileSize;
        return new Rectangle(left, top, width, height);
    }
}