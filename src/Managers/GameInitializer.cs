using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameInitializer
{
    private EntityManager _entityManager;
    private AssetStore _assets;
    private InputSystem _inputSystem;
    private AnimationSystem _animationSystem;
    private SpriteBatch _spriteBatch;
    private GraphicsDevice _graphicsDevice;
    private Camera2D _camera;

    public Entity PlayerEntity { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public RenderSystem RenderSystem { get; private set; }
    public MapSystem MapSystem;

    public Entity npc { get; private set; }

    public static bool ShowHitbox = false;

    public GameInitializer(EntityManager entityManager, SpriteBatch spriteBatch, AssetStore assets)
    {
        _entityManager = entityManager;
        _assets = assets;
        _spriteBatch = spriteBatch;
        _graphicsDevice = _spriteBatch.GraphicsDevice;
    }

    public void Initialize()
    {
        _inputSystem = new InputSystem();
        _camera = new Camera2D(_graphicsDevice.Viewport);

        RenderSystem = new RenderSystem(_spriteBatch, _entityManager, _assets, _camera);
        _animationSystem = new AnimationSystem(_entityManager);

        MapSystem = new MapSystem(_entityManager, _assets, _camera);

        // Create Player
        // Color[][] colourChanges = new Color[][]{
        //     new Color[] { new Color(255, 0, 0), new Color(85, 60, 45) },
        //     new Color[] { new Color(0, 0, 255), new Color(100, 70, 55) }
        // };
        Color[][] colourChanges = [
            [new Color(255, 0, 0), new Color(0, 0, 0)],
            [new Color(0, 0, 255), new Color(40, 40, 40)]
        ];
        Texture2D hairSheet = SpriteProcessor.ChangeColours(colourChanges, _assets.PlayerHair, _graphicsDevice);
        Texture2D layeredSheet = SpriteProcessor.LayerSheets([_assets.PlayerBody, hairSheet, _assets.PlayerTools], _graphicsDevice);
        PlayerEntity = PlayerFactory.CreatePlayer(200, 200, _entityManager, layeredSheet);

        PlayerController = new PlayerController(PlayerEntity, _inputSystem, _animationSystem, _entityManager.Entities, _camera);

        // Create an Entity
        npc = PlayerFactory.CreatePlayer(50, 300, _entityManager, _assets.PlayerBody);
        npc.GetComponent<SpriteComponent>().Color = Color.Red;

    }

    public void Update(GameTime gameTime)
    {
        _inputSystem.Update();
        PlayerController.Update();
        _animationSystem.Update(gameTime);
    }

    public void Draw()
    {
        RenderSystem.Draw();
    }
}
