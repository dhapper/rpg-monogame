using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameInitializer
{
    private EntityManager _entityManager;
    private AssetStore _assets;
    private InputSystem _inputSystem;
    private AnimationSystem _animationSystem;
    private SpriteBatch _spriteBatch;

    public Entity PlayerEntity { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public RenderSystem RenderSystem { get; private set; }
    public MapSystem MapSystem;

    public Entity npc { get; private set; }

    // public TileMap _map;
    // public Tile[] _tiles;

    public GameInitializer(EntityManager entityManager, SpriteBatch spriteBatch, AssetStore assets)
    {
        _entityManager = entityManager;
        _assets = assets;
        _spriteBatch = spriteBatch;
        // _map = map;
        // _tiles = tiles;
    }

    public void Initialize()
    {
        _inputSystem = new InputSystem();

        RenderSystem = new RenderSystem(_spriteBatch, _entityManager);
        _animationSystem = new AnimationSystem(_entityManager);

        MapSystem = new MapSystem(_entityManager, _assets);

        // Create Player
        PlayerEntity = PlayerFactory.CreatePlayer(100, 100, _entityManager, _assets.PlayerTexture);

        PlayerController = new PlayerController(PlayerEntity, _inputSystem, _animationSystem, _entityManager.Entities);

        // Create an Entity
        npc = PlayerFactory.CreatePlayer(50, 300, _entityManager, _assets.PlayerTexture);
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
