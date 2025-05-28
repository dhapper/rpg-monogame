using Microsoft.Xna.Framework;

public struct Position
{
    public float X, Y;
    public int Width, Height;

    public Position(float x, float y, int width = 16, int height = 16)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    // public Rectangle GetHitbox() => new((int)X, (int)Y, (int) (Width * Constants.ScaleFactor), (int) (Height * Constants.ScaleFactor));
}