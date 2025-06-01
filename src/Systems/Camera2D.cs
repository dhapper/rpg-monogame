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

    public void Follow(Vector2 target)
    {
        float halfViewportWidth = _viewport.Width / 2f;
        float halfViewportHeight = _viewport.Height / 2f;
        float clampedX = MathHelper.Clamp(target.X - halfViewportWidth, 0, _worldWidth - _viewport.Width);
        float clampedY = MathHelper.Clamp(target.Y - halfViewportHeight, 0, _worldHeight - _viewport.Height);
        Position = new Vector2(clampedX, clampedY);
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
}
