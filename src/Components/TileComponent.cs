using Microsoft.Xna.Framework;

public class TileComponent
{
    public int Id { get; set; }
    public AnimationConfig AnimationConfig;
    public Rectangle Hitbox;
    public string Type;
    public int Background;

    // public TileComponent(int id, string type = "ground", Rectangle hitbox = default, AnimationConfig animationConfig = null, int background = -1)
    // {
    //     Id = id;
    //     Type = type;
    //     Hitbox = hitbox;
    //     AnimationConfig = animationConfig;
    //     Background = background;
    // }

    // public TileComponent(int id, string spriteType = "ground", Rectangle hitbox = default, int background = -1)
    // {
    //     Id = id;
    //     SpriteType = spriteType;
    //     Hitbox = hitbox;
    //     Background = background;
    // }


    public TileComponent() { }


    // public class TileData
    // {
    //     public int Type { get; set; }
    //     public int Id { get; set; }

    //     public TileData(int type, int id)
    //     {
    //         Type = type;
    //         Id = id;
    //     }
    // }
}