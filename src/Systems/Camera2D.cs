using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Camera2D
{
    public Vector2 Position { get; private set; }
    public Matrix Transform { get; private set; }

    private Viewport _viewport;

    public Camera2D(Viewport viewport)
    {
        _viewport = viewport;
        Position = Vector2.Zero;
        UpdateTransform();
    }

    public void Follow(Vector2 target)
    {
        // Center camera on target
        Position = target - new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);
        // Position = target;
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
    }
}
