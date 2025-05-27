using Microsoft.Xna.Framework;

public class Tile
{
    public Rectangle SourceRect { get; private set; }

    public Tile(Rectangle sourceRect)
    {
        SourceRect = sourceRect;
    }
}
