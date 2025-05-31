using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class AssetStore
{
    public SpriteFont GameFont { get; private set; }
    public Texture2D PlayerTexture { get; private set; }
    public Texture2D TileSheet;
    public Texture2D GroundTiles, CollisionTiles, AnimatedTiles, BackgroundTiles;

    private ContentManager _content;

    public AssetStore(ContentManager content)
    {
        _content = content;
    }

    public void LoadAll()
    {
        GameFont = _content.Load<SpriteFont>("gameFont");
        PlayerTexture = _content.Load<Texture2D>("custom_player_sheet");

        TileSheet = _content.Load<Texture2D>("tilesheet");
        GroundTiles = _content.Load<Texture2D>("walkable");
        CollisionTiles = _content.Load<Texture2D>("solids");
        AnimatedTiles = _content.Load<Texture2D>("animations");
        BackgroundTiles = _content.Load<Texture2D>("background");
    }
}
