using System;
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
    private MapSystem _mapSystem;
    private InventoryUI _inventoryUI;

    public Entity PlayerEntity { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public RenderSystem RenderSystem { get; private set; }

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
        _inventoryUI = new InventoryUI(_camera, _graphicsDevice.Viewport);

        RenderSystem = new RenderSystem(_spriteBatch, _entityManager, _assets, _camera, _graphicsDevice, _inventoryUI);
        _animationSystem = new AnimationSystem(_entityManager);
        _mapSystem = new MapSystem(_entityManager, _assets, _camera);

        // Create Player
        PlayerEntity = PlayerFactory.CreatePlayer(200, 200, _entityManager, _assets, _graphicsDevice);
        PlayerController = new PlayerController(PlayerEntity, _inputSystem, _animationSystem, _entityManager.Entities, _mapSystem, _camera);
        PlayerEntity.AddComponent(new PlayerComponent());
        var (x, y) = SaveManager.LoadData();
        var position = PlayerEntity.GetComponent<PositionComponent>();
        Console.WriteLine(position.X+" "+position.Y);
        position.X = x;
        position.Y = y;
        Console.WriteLine(position.X+" "+position.Y);
        // Create an Entity
        npc = PlayerFactory.CreatePlayer(50, 300, _entityManager, _assets, _graphicsDevice);
        npc.GetComponent<SpriteComponent>().Color = Color.Red;

    }

    public void Update(GameTime gameTime)
    {
        _inputSystem.Update();
        PlayerController.Update();
        _animationSystem.Update(gameTime);
        _inventoryUI.Update();

    }

    public void Draw()
    {
        RenderSystem.Draw();
    }
}
