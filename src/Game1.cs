using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rpg;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private EntityManager _entityManager;
    // private AssetStore _assetStore;
    private GameInitializer _gameInitializer;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = 1800 - 600;  // Set width
        _graphics.PreferredBackBufferHeight = 970 - 300;  // Set height
        _graphics.ApplyChanges(); // Apply the size change

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // _assetStore = new AssetStore(Content);
        // _assetStore.LoadAll();
        AssetStore.Initialize(Content);


        _entityManager = new EntityManager();
        _gameInitializer = new GameInitializer(_entityManager, _spriteBatch);
        _gameInitializer.Initialize();

    }

    protected override void Update(GameTime gameTime)
    {
        _gameInitializer.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // GraphicsDevice.Clear(Color.CornflowerBlue);
        GraphicsDevice.Clear(Color.LightPink);

        _gameInitializer.Draw();
        base.Draw(gameTime);
    }
}
