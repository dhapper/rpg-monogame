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

    public GameInitializer(EntityManager entityManager, SpriteBatch spriteBatch, AssetStore assets)
    {
        _entityManager = entityManager;
        _assets = assets;
        _spriteBatch = spriteBatch;
    }

    public void Initialize()
    {
        _inputSystem = new InputSystem();

        // Create Player
        PlayerEntity = PlayerFactory.CreatePlayer(_entityManager, _assets.PlayerTexture);

        PlayerController = new PlayerController(PlayerEntity, _inputSystem, 4);
        RenderSystem = new RenderSystem(_spriteBatch, _entityManager);
        AnimationSystem = new AnimationSystem(_entityManager);
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
