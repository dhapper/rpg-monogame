using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static Constants;
using static Constants.UI;

public class UIRenderSystem
{

    private SpriteBatch _spriteBatch;
    private AssetStore _assetStore;
    private EntityManager _entityManager;
    private InputSystem _inputSystem;
    private Camera2D _camera;
    private GameStateManager _gameStateManager;
    private InventoryUI _inventoryUI;

    public UIRenderSystem(
         SpriteBatch spriteBatch,
         AssetStore assetStore,
         EntityManager entityManager,
         InputSystem inputSystem,
         Camera2D camera,
         GameStateManager gameStateManager,
         InventoryUI inventoryUI)
    {
        _spriteBatch = spriteBatch;
        _assetStore = assetStore;
        _entityManager = entityManager;
        _inputSystem = inputSystem;
        _camera = camera;
        _gameStateManager = gameStateManager;
        _inventoryUI = inventoryUI;
    }

    public void Draw()
    {

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        DrawHotbar();
        if (_gameStateManager.CurrentGameState == GameState.Inventory)
        {
            DrawInventory();

            if (_inventoryUI.CurrentlyDragging)
                DrawDraggedItem();
        }

        _spriteBatch.End();
    }

    public void DrawDraggedItem()
    {
        // var (x, y) = _inputSystem.GetMouseLocationRelativeCamera(_camera);
        var (x, y) = _inputSystem.GetMouseLocation();
        _spriteBatch.Draw(
            _assetStore.IconSheet,
            new Vector2(x, y),
            _inventoryUI.DraggedItem.GetComponent<SpriteComponent>().SourceRectangle,
            Color.White,
            0f,
            Vector2.Zero,
            ScaleFactor,
            SpriteEffects.None,
            0f);

    }

    public void DrawInventory()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                // Draw inventory slots
                _spriteBatch.Draw(
                    _assetStore.UIsheet,
                    _inventoryUI.InventorySlotPositions[i][j],
                    new Rectangle(0, 0, Inventory.DefaultSlotSize, Inventory.DefaultSlotSize),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    ScaleFactor,
                    SpriteEffects.None,
                    0f);

                // Draw inventory items
                var inv = _entityManager.EntitiesWithComponent<InventoryComponent>().First().GetComponent<InventoryComponent>();
                if (inv.InventoryItems[i][j] != null)
                {
                    _spriteBatch.Draw(
                        _assetStore.IconSheet,
                        _inventoryUI.InventoryIconPositions[i][j],
                        inv.InventoryItems[i][j].GetComponent<ItemComponent>().config.SourceRectangle,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        ScaleFactor,
                        SpriteEffects.None,
                        0f);
                }
            }
        }
    }

    public void DrawHotbar()
    {

        for (int i = 0; i < 9; i++)
        {
            // Draw hotbar slots
            _spriteBatch.Draw(
                _assetStore.UIsheet,
                _inventoryUI.InventorySlotPositions[i][0],
                new Rectangle(0, 0, Inventory.DefaultSlotSize, Inventory.DefaultSlotSize),
                Color.White,
                0f,
                Vector2.Zero,
                ScaleFactor,
                SpriteEffects.None,
                0f);
        }

        for (int i = 0; i < 9; i++)
        {
            // Draw active hotbar slot
            var inv = _entityManager.EntitiesWithComponent<InventoryComponent>().First().GetComponent<InventoryComponent>();
            if (i == inv.activeItemIndices.Item1)
            {
                _spriteBatch.Draw(
                    _assetStore.UIsheet,
                    _inventoryUI.InventorySlotPositions[i][0],
                    new Rectangle(32, 0, UI.Inventory.DefaultSlotSize, UI.Inventory.DefaultSlotSize),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    ScaleFactor,
                    SpriteEffects.None,
                    0f);
            }

            // Draw hotbar items
            if (inv.InventoryItems[i][0] != null)
            {
                _spriteBatch.Draw(
                    _assetStore.IconSheet,
                    _inventoryUI.InventoryIconPositions[i][0],
                    inv.InventoryItems[i][0].GetComponent<ItemComponent>().config.SourceRectangle,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    ScaleFactor,
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}