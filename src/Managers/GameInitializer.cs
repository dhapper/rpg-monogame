using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameInitializer
{
    private EntityManager _entityManager;
    // private AssetStore _assets;
    // private InputSystem _inputSystem;
    private AnimationSystem _animationSystem;
    private SpriteBatch _spriteBatch;
    private GraphicsDevice _graphicsDevice;
    private Camera2D _camera;
    private MapSystem _mapSystem;
    private InventoryUI _inventoryUI;
    // private GameStateManager _gameStateManager;
    private InventorySystem _inventorySystem;
    private InteractionSystem _interactionSystem;
    private SleepSystem _sleepSystem;
    private DialogueSystem _dialogueSystem;
    private ShopSystem _shopSystem;

    public Entity PlayerEntity { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public RenderSystem RenderSystem { get; private set; }

    public Entity npc { get; private set; }

    public static bool ShowHitbox = false;


    // private GameState currentGameState = GameState.Playing;



    public GameInitializer(EntityManager entityManager, SpriteBatch spriteBatch)
    {
        _entityManager = entityManager;
        _spriteBatch = spriteBatch;
        _graphicsDevice = _spriteBatch.GraphicsDevice;
    }

    public void Initialize()
    {
        // _inputSystem = new InputSystem();
        // _gameStateManager = new GameStateManager();
        _camera = new Camera2D(_graphicsDevice.Viewport);
        _inventoryUI = new InventoryUI(_camera, _graphicsDevice.Viewport, _entityManager);
        _inventorySystem = new InventorySystem(_entityManager);
        _animationSystem = new AnimationSystem(_entityManager);
        _interactionSystem = new InteractionSystem(_entityManager, _animationSystem, _camera, _inventorySystem);
        _sleepSystem = new SleepSystem(_entityManager, _interactionSystem);
        _dialogueSystem = new DialogueSystem(_sleepSystem);
        _shopSystem = new ShopSystem();

        RenderSystem = new RenderSystem(_spriteBatch, _entityManager, _camera, _graphicsDevice, _inventoryUI, _dialogueSystem, _shopSystem);
        _mapSystem = new MapSystem(_entityManager, _camera, _sleepSystem);

        // Create Player
        PlayerEntity = PlayerFactory.CreatePlayer(200, 200, _entityManager, _graphicsDevice, _inventorySystem);
        PlayerController = new PlayerController(PlayerEntity, _animationSystem, _mapSystem, _camera, _entityManager, _inventorySystem, _interactionSystem);
        PlayerEntity.AddComponent(new PlayerComponent());   // should move to factory?
        var (x, y) = SaveManager.LoadData();
        var position = PlayerEntity.GetComponent<PositionComponent>();
        position.X = x;
        position.Y = y;
        _inventoryUI.InitializePlayerInventory();

        // Create an Entity
        // npc = PlayerFactory.CreatePlayer(50, 300, _entityManager, _assets, _graphicsDevice);
        // npc.GetComponent<SpriteComponent>().Color = Color.Red;

    }

    public void Update(GameTime gameTime)
    {

        InputSystem.Update();
        _inventoryUI.Update();

        var inputs = InputSystem.GetInputState();
        if (inputs.ToggleInventory)
            GameStateManager.ToggleBetweenStates(GameState.Playing, GameState.Inventory);

        switch (GameStateManager.CurrentGameState)
        {
            case GameState.Playing:
                // _inputSystem.Update();
                PlayerController.Update();
                _animationSystem.Update(gameTime);
                // _inventoryUI.Update();
                break;
            case GameState.Inventory:
                break;
            case GameState.DialogueBox:
                _dialogueSystem.Update();
                break;
        }
    }

    public void Draw()
    {
        RenderSystem.Draw();
    }
}
