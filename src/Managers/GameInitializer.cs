using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameInitializer
{
    private EntityManager _currentEntityManager;
    private AnimationSystem _animationSystem;
    private SpriteBatch _spriteBatch;
    private GraphicsDevice _graphicsDevice;
    private Camera2D _camera;
    private MapSystem _mapSystem;
    private InventoryUI _inventoryUI;
    private InventorySystem _inventorySystem;
    private InteractionSystem _interactionSystem;
    private SleepSystem _sleepSystem;
    private DialogueSystem _dialogueSystem;
    private ShopSystem _shopSystem;
    private LocationManager _locationManager;

    public Entity PlayerEntity { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public RenderSystem RenderSystem { get; private set; }

    public Entity npc { get; private set; }

    public static bool ShowHitbox = false;

    // move to manager?
    public int CurrentLocationIndex = 0;

    public GameInitializer(SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
        _graphicsDevice = _spriteBatch.GraphicsDevice;
    }

    // should this be here?
    public void LoadMap(int locationIndex)
    {
        CurrentLocationIndex = locationIndex;
        _currentEntityManager = _locationManager.GetEntityManager(locationIndex);
        _mapSystem = _locationManager.GetMapSystem(locationIndex);

        _mapSystem.SetCameraBounds(_camera);

        _inventoryUI = new InventoryUI(_camera, _graphicsDevice.Viewport, _currentEntityManager);
        _inventorySystem = new InventorySystem(_currentEntityManager);
        _animationSystem = new AnimationSystem(_currentEntityManager);
        _interactionSystem = new InteractionSystem(_currentEntityManager, _animationSystem, _camera, _inventorySystem);
        _sleepSystem = new SleepSystem(_currentEntityManager, _interactionSystem);
        _dialogueSystem = new DialogueSystem(_sleepSystem);
        _shopSystem = new ShopSystem(_currentEntityManager, _inventorySystem);

        RenderSystem = new RenderSystem(_spriteBatch, _currentEntityManager, _camera, _graphicsDevice, _inventoryUI, _dialogueSystem, _shopSystem);

        if (PlayerController != null)
            PlayerController.UpdateEntityManager(_currentEntityManager);

        var inv = PlayerEntity.GetComponent<InventoryComponent>();
        _inventoryUI.InitializePlayerInventory(inv);
        _inventorySystem.InitInventory(inv);

        _currentEntityManager.LoadPlayer(PlayerEntity);
    }

    public void Initialize()
    {

        PlayerEntity = PlayerFactory.CreatePlayer(200, 200);
        _camera = new Camera2D(_graphicsDevice.Viewport);
        _locationManager = new LocationManager();

        LoadMap(Constants.Location.Location1Index);

        PlayerController = new PlayerController(this, PlayerEntity, _animationSystem, _mapSystem, _camera, _currentEntityManager, _inventorySystem, _interactionSystem);
        var (x, y) = SaveManager.LoadData();
        var position = PlayerEntity.GetComponent<PositionComponent>();
        position.X = x;
        position.Y = y;
    }

    public void Update(GameTime gameTime)
    {

        InputSystem.Update();

        switch (GameStateManager.CurrentGameState)
        {
            case GameState.Playing:
                PlayerController.Update();
                _animationSystem.Update(gameTime);
                break;
            case GameState.Inventory:
                _inventoryUI.Update();
                break;
            case GameState.DialogueBox:
                _dialogueSystem.Update();
                break;
            case GameState.Shop:
                _shopSystem.Update();
                break;
        }
    }

    public void Draw()
    {
        RenderSystem.Draw();
    }
}
