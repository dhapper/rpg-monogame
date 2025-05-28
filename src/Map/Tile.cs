using Microsoft.Xna.Framework;

public class Tile
{
    public Rectangle SourceRect { get; private set; }
    public bool IsSolid { get; private set; }

    public Tile(Rectangle sourceRect, bool isSolid = false)
    {
        SourceRect = sourceRect;
        IsSolid = isSolid;
    }
}
