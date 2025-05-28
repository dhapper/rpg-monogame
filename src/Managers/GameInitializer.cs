using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameInitializer
{
    private EntityManager _entityManager;
    private AssetStore _assets;
    private InputSystem _inputSystem;
    private SpriteBatch _spriteBatch;

    public Entity PlayerEntity { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public RenderSystem RenderSystem { get; private set; }
    public AnimationSystem AnimationSystem { get; private set; }

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

        // Create Player
        PlayerEntity = PlayerFactory.CreatePlayer(100, 100, _entityManager, _assets.PlayerTexture);

        PlayerController = new PlayerController(PlayerEntity, _inputSystem, Constants.Player.Speed, _entityManager.Entities);
        RenderSystem = new RenderSystem(_spriteBatch, _entityManager);
        AnimationSystem = new AnimationSystem(_entityManager);

        // Create an Entity
        npc = PlayerFactory.CreatePlayer(200, 200, _entityManager, _assets.PlayerTexture);
        npc.GetComponent<Sprite>().Color = Color.Red;

    }

    public void Update(GameTime gameTime)
    {
        _inputSystem.Update();
        PlayerController.Update();
        AnimationSystem.Update(gameTime);
    }

    public void Draw()
    {
        RenderSystem.Draw();
    }
}
