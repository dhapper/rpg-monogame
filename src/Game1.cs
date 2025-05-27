using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rpg;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private EntityManager _entityManager;
    private AssetStore _assetStore;
    private GameInitializer _gameInitializer;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    TileRenderer tileRenderer;
    TileMap map;
    int[,] mapData = new int[,]
    {
        { 0, 1, 2, 1},
        { 0, 0, 0, 0},
        { 1, 1, 1, 1},
        { 0, 0, 0, 0}
    };

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _assetStore = new AssetStore(Content);
        _assetStore.LoadAll();

        _entityManager = new EntityManager();
        _gameInitializer = new GameInitializer(_entityManager, _spriteBatch, _assetStore);
        _gameInitializer.Initialize();


        int tileSize = 16;
        Texture2D tileset = Content.Load<Texture2D>("tilesheet");
        var tileDefs = TileUtils.LoadTiles(tileSize, tileSize, tileset.Width, tileset.Height);
        map = new TileMap(mapData, tileSize, tileSize);
        tileRenderer = new TileRenderer(_spriteBatch, tileset, tileDefs);

    }

    protected override void Update(GameTime gameTime)
    {
        _gameInitializer.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        tileRenderer.Draw(map);
        _spriteBatch.End();

        _gameInitializer.Draw();
        base.Draw(gameTime);
    }
}
