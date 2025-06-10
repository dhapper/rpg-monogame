using System.ComponentModel;
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

    private int border = 3 * (int)(Constants.DefaultTileSize * Constants.ScaleFactor);

    private float _zoom { get; set; } = 1f;

    public Camera2D(Viewport viewport)
    {
        _viewport = viewport;
        Position = Vector2.Zero;
        UpdateTransform();
    }

    public void Follow(Vector2 target)
    {
        float leftBound = Position.X + border;
        float rightBound = Position.X + _viewport.Width - border;
        float topBound = Position.Y + border;
        float bottomBound = Position.Y + _viewport.Height - border;

        float newX = Position.X;
        float newY = Position.Y;

        // Horizontal camera adjustment
        if (target.X < leftBound)
            newX = target.X - border;
        else if (target.X > rightBound)
            newX = target.X - _viewport.Width + border;

        // Vertical camera adjustment
        if (target.Y < topBound)
            newY = target.Y - border;
        else if (target.Y > bottomBound)
            newY = target.Y - _viewport.Height + border;

        // Clamp to world bounds
        newX = MathHelper.Clamp(newX, 0, _worldWidth - _viewport.Width);
        newY = MathHelper.Clamp(newY, 0, _worldHeight - _viewport.Height);

        Position = new Vector2(newX, newY);
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
    }

    public void SetWorldBounds(int width, int height)
    {
        _worldWidth = width;
        _worldHeight = height;
    }

    public float Zoom { get; set; } = 1f;

    public Rectangle GetViewRectangle()
    {
        int width = (int)(_viewport.Width / Zoom) + Constants.TileSize;
        int height = (int)(_viewport.Height / Zoom) + Constants.TileSize;
        int left = (int)Position.X - Constants.TileSize;
        int top = (int)Position.Y - Constants.TileSize;
        return new Rectangle(left, top, width, height);
    }
}
