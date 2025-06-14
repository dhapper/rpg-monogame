using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public static class AssetStore
{
    public static SpriteFont GameFont { get; private set; }
    public static Texture2D PlayerTexture { get; private set; }
    public static Texture2D TileSheet;
    public static Texture2D GroundTiles, AnimatedTiles;

    public static Texture2D PlayerBody, PlayerHair, PlayerTools;
    public static Texture2D BackgroundTiles, PathTiles, WaterTiles, CollisionTiles;
    public static Texture2D UIsheet, IconSheet;
    public static Texture2D CropSprites;
    public static Texture2D PlayerSheet;
    private static ContentManager _content;

    // public AssetStore(ContentManager content)
    // {
    //     _content = content;
    // }

    public static void Initialize(ContentManager content)
    {
        _content = content;
        LoadAll();
    }

    public static void LoadAll()
    {
        GameFont = _content.Load<SpriteFont>("gameFont");
        PlayerTexture = _content.Load<Texture2D>("custom_player_sheet");

        // TileSheet = _content.Load<Texture2D>("tilesheet");
        // GroundTiles = _content.Load<Texture2D>("walkable");
        // CollisionTiles = _content.Load<Texture2D>("solids");
        // AnimatedTiles = _content.Load<Texture2D>("animations");
        // BackgroundTiles = _content.Load<Texture2D>("background");

        PlayerBody = _content.Load<Texture2D>("new_player_body");
        PlayerHair = _content.Load<Texture2D>("new_player_hair");
        PlayerTools = _content.Load<Texture2D>("new_player_tool");

        BackgroundTiles = _content.Load<Texture2D>("background");
        CollisionTiles = _content.Load<Texture2D>("collision");
        PathTiles = _content.Load<Texture2D>("paths");
        WaterTiles = _content.Load<Texture2D>("water");

        UIsheet = _content.Load<Texture2D>("ui");
        IconSheet = _content.Load<Texture2D>("icons");

        CropSprites = _content.Load<Texture2D>("farming");

        PlayerSheet = _content.Load<Texture2D>("Master File");

    }

    // public void UpdateSheets(GraphicsDevice graphicsDevice)
    // {
        // Color[][] colourChanges = [
        //     [new Color(0, 0, 0), new Color(200, 200, 200)],
        // ];
        // PathTiles = SpriteProcessor.ChangeColours(colourChanges, PathTiles, graphicsDevice);
        // CollisionTiles = SpriteProcessor.ChangeColours(colourChanges, CollisionTiles, graphicsDevice);
    // }
}
