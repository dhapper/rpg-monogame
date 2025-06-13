using Microsoft.Xna.Framework;

public class TileComponent
{
    public int Id { get; set; }
    public AnimationConfig AnimationConfig;
    public Rectangle Hitbox;
    public string Type;
    public Rectangle Background = default;

    // newly added, might be able to refactor more code with this
    public int Col;
    public int Row;

    public TileComponent() { }
}