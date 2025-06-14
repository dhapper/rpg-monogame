using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static Constants;
using static Constants.UI;

public class UIRenderSystem
{

    private SpriteBatch _spriteBatch;
    private EntityManager _entityManager;
    private Camera2D _camera;
    // private GameStateManager _gameStateManager;
    private InventoryUI _inventoryUI;

    private GraphicsDevice _graphicsDevice;
    private DialogueSystem _dialogueSystem;
    private ShopSystem _shopSystem;

    public UIRenderSystem(
         SpriteBatch spriteBatch,
         EntityManager entityManager,
         Camera2D camera,
         //  GameStateManager gameStateManager,
         InventoryUI inventoryUI,
         GraphicsDevice graphicsDevice,
         DialogueSystem dialogueSystem,
         ShopSystem shopSystem
         )
    {
        _spriteBatch = spriteBatch;
        _entityManager = entityManager;
        _camera = camera;
        // _gameStateManager = gameStateManager;
        _inventoryUI = inventoryUI;
        _graphicsDevice = graphicsDevice;
        _dialogueSystem = dialogueSystem;
        _shopSystem = shopSystem;
    }

    public void Draw()
    {

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        DrawHotbar();

        if (GameStateManager.CurrentGameState == GameState.Inventory)
        {
            DrawInventory();
            if (_inventoryUI.CurrentlyDragging)
                DrawDraggedItem();
        }

        if (GameStateManager.CurrentGameState == GameState.DialogueBox)
            DrawDialogueBox();

        if (GameStateManager.CurrentGameState == GameState.Shop)
            DrawShopMenu();

        _spriteBatch.End();
    }

    public void DrawShopMenu()
    {
        Texture2D _pixelTexture;
        _pixelTexture = new Texture2D(_graphicsDevice, 1, 1);
        _pixelTexture.SetData([Color.White]);
        // int x = TileSize;
        // int y = TileSize;
        // Vector2 textSize = AssetStore.GameFont.MeasureString(_shopSystem.Line);
        // // int width = (int)textSize.X;
        // // int height = (int)((_shopSystem.Options.Length + 1) * textSize.Y);
        // int width = _shopSystem.ItemBoxes[0].Width + _shopSystem.Spacer * 2;
        // int height = (int)textSize.Y + _shopSystem.Options.Length * (_shopSystem.ItemBoxes[0].Height + _shopSystem.Spacer);

        _spriteBatch.Draw(_pixelTexture, _shopSystem.MenuBox, Color.Black);
        _spriteBatch.DrawString(AssetStore.GameFont, _shopSystem.Line, new Vector2(_shopSystem.MenuBox.X, _shopSystem.MenuBox.Y), Color.White);

        for (int i = 0; i < _shopSystem.Options.Length; i++)
        {
            _spriteBatch.Draw(_pixelTexture, _shopSystem.ItemBoxes[i], Color.DarkBlue);
            var offset = _shopSystem.Spacer / 2;
            _spriteBatch.DrawString(AssetStore.GameFont, _shopSystem.Options[i].Name, new Vector2(_shopSystem.ItemBoxes[i].X + offset, _shopSystem.ItemBoxes[i].Y + offset), Color.White);
        }
    }

    public void DrawDialogueBox()
    {
        Texture2D _pixelTexture;
        _pixelTexture = new Texture2D(_graphicsDevice, 1, 1);
        _pixelTexture.SetData([Color.White]);
        int x = TileSize;
        int y = TileSize;
        Vector2 textSize = AssetStore.GameFont.MeasureString(_dialogueSystem.Line);
        int width = (int)textSize.X;
        int height = (int)((_dialogueSystem.Options.Length + 1) * textSize.Y);
        _spriteBatch.Draw(_pixelTexture, new Rectangle(x, y, width, height), Color.Black);
        _spriteBatch.DrawString(AssetStore.GameFont, _dialogueSystem.Line, new Vector2(x, y), Color.White);
        for (int i = 0; i < _dialogueSystem.Options.Length; i++)
            _spriteBatch.DrawString(AssetStore.GameFont, _dialogueSystem.Options[i], new Vector2(x, y + (int)((1 + i) * textSize.Y)), _dialogueSystem.choice == i ? Color.Yellow : Color.White);
    }

    public void DrawDraggedItem()
    {
        // var (x, y) = _inputSystem.GetMouseLocationRelativeCamera(_camera);
        var (x, y) = InputSystem.GetMouseLocation();
        _spriteBatch.Draw(
            AssetStore.IconSheet,
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
                    AssetStore.UIsheet,
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
                        AssetStore.IconSheet,
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
                AssetStore.UIsheet,
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
                    AssetStore.UIsheet,
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
                    AssetStore.IconSheet,
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