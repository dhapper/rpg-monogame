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

    public ShopSystem(EntityManager entityManager, InventorySystem inventorySystem)
    {
        _entityManager = entityManager;
        _inventorySystem = inventorySystem;

        InitShop(
            "Need for seed?",
            [
                Constants.Items.PumpkinSeeds,
                Constants.Items.PotatoSeeds,
                Constants.Items.WateringCan,
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
        // bool mousePressed = InputSystem.IsMousePressed(InputSystem.MouseButton.Left);

        for (int i = 0; i < Options.Length; i++)
        {
            if (ItemBoxes[i].Contains(mouse.x, mouse.y))
            {
                Choice = i;
                bool mousePressed = InputSystem.IsMousePressed(InputSystem.MouseButton.Left);
                if (mousePressed)
                {
                    var item = ItemFactory.CreateItem(Options[Choice], _entityManager);
                    _inventorySystem.PlaceInNextEmptySlot(item);
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