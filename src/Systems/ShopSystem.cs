using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

public class ShopSystem
{
    public string Line;
    // public Entity[] Options;
    public ItemConfig[] Options;
    public int Choice = 0;

    private EntityManager _entityManager;
    private InventorySystem _inventorySystem;
    private InventoryUI _inventoryUI;

    public ShopSystem(EntityManager entityManager, InventorySystem inventorySystem, InventoryUI inventoryUI)
    {
        _entityManager = entityManager;
        _inventorySystem = inventorySystem;
        _inventoryUI = inventoryUI;

        InitShop(
            "Need for seed?",
            [
                Constants.Items.PumpkinSeeds,
                Constants.Items.PotatoSeeds,
                // Constants.Items.WateringCan,
            ]
        );
    }

    public void Update()
    {
        HandleMouseInputs();

        HandleKeyboardInputs();
    }

    private void HandleKeyboardInputs()
    {
        var inputs = InputSystem.GetInputState();
        if (inputs.Escape)
        {
            GameStateManager.SetState(GameState.Playing);
        }
    }

    private void HandleMouseInputs()
    {
        var mouse = InputSystem.GetMouseLocation();

        for (int i = 0; i < Options.Length; i++)
        {
            if (ItemBoxes[i].Contains(mouse.x, mouse.y))
            {
                Choice = i;
                bool mousePressed = InputSystem.IsMousePressed(InputSystem.MouseButton.Left);
                if (mousePressed)
                {
                    var item = ItemFactory.CreateItem(Options[Choice], _entityManager);
                    var itemName = item.GetComponent<ItemComponent>().config.Name;
                    var value = Constants.Value.NameToValue[itemName];
                    if(value > _inventorySystem.Coins) { continue; }
                    _inventorySystem.PlaceInNextEmptySlot(item);
                    _inventorySystem.Coins -= value;
                }
            }
        }

        for (int i = 0; i < Constants.UI.Inventory.Cols; i++) {
            for (int j = 0; j < Constants.UI.Inventory.Rows; j++)
            {
                bool isIn = _inventoryUI.InventorySlotRectangles[i][j].Contains(mouse.x, mouse.y);
                if (!isIn) { continue; }

                var item = _inventorySystem.Inventory.InventoryItems[i][j];
                if (item == null) { continue; }

                bool rightClicked = InputSystem.IsMousePressed(InputSystem.MouseButton.Right);

                if (rightClicked)
                {
                    var itemName = item.GetComponent<ItemComponent>().config.Name;
                    Console.WriteLine(itemName);
                    if (!Constants.Value.NameToValue.ContainsKey(itemName)) { continue; }
                    var value = Constants.Value.NameToValue[itemName];
                    _entityManager.DeleteEntity(item);
                    _inventorySystem.Coins += value;
                    _inventorySystem.Inventory.InventoryItems[i][j] = null;
                }
            }
        }
        
    }

    public void InitShop(string line, ItemConfig[] options)
    {
        Line = line;
        Options = options;

        CreateLayout();
    }

    public Rectangle[] ItemBoxes;
    public Rectangle MenuBox;
    public int Spacer = (int)(2 * Constants.ScaleFactor);

    private void CreateLayout()
    {
        Vector2 textSize = AssetStore.GameFont.MeasureString("abcdefg");
        int yOffset = (int)textSize.Y;
        int width = (int)(100 * Constants.ScaleFactor);
        int height = (int)textSize.Y + Spacer;

        MenuBox = new Rectangle(Constants.TileSize, Constants.TileSize, width + Spacer * 2, yOffset + Options.Length * (height + Spacer));

        ItemBoxes = new Rectangle[Options.Length];
        for (int i = 0; i < Options.Length; i++)
        {
            ItemBoxes[i] = new Rectangle(MenuBox.X + Spacer, MenuBox.Y + yOffset + i * (height + Spacer), width, height);
        }
    }
}