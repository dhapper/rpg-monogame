using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteComponent
{
    public Texture2D Texture;
    public Rectangle? SourceRectangle { get; set; }
    public Color Color { get; set; } = Color.White;

    public SpriteComponent(Texture2D texture, Rectangle sourceRectangle)
    {
        Texture = texture;
        SourceRectangle = sourceRectangle;
    }
}