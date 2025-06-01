using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TileComponent
{
    public int Id { get; set; }
    public AnimationConfig AnimationConfig;
    public Rectangle Hitbox;
    public string Type;
    public Rectangle Background = default;

    public TileComponent() { }
}