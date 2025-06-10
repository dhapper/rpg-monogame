using System;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static Constants.UI.Inventory;
using Vector2 = Microsoft.Xna.Framework.Vector2;

public class InventoryUI
{
    private Camera2D _camera;
    private Viewport _viewport;
    private EntityManager _entityManager;
    private InventoryComponent _inventory;
    private InputSystem _inputSystem;
    private GameStateManager _gameStateManager;

    public Vector2[][] InventorySlotPositions, InventoryIconPositions;
    public Rectangle[][] InventorySlotRectangles;
    public Entity DraggedItem = null;
    public bool CurrentlyDragging = false;

    private int _draggedItemCol, _draggedItemRow;
    private Entity _draggedItem;

    public InventoryUI(Camera2D camera, Viewport viewport, EntityManager entityManager, InputSystem inputSystem, GameStateManager gameStateManager)
    {
        _camera = camera;
        _viewport = viewport;
        _entityManager = entityManager;
        _inputSystem = inputSystem;
        _gameStateManager = gameStateManager;

        InventorySlotPositions = new Vector2[Cols][];
        InventoryIconPositions = new Vector2[Cols][];
        InventorySlotRectangles = new Rectangle[Cols][];
        for (int i = 0; i < Cols; i++)
        {
            InventorySlotPositions[i] = new Vector2[Rows];
            InventoryIconPositions[i] = new Vector2[Rows];
            InventorySlotRectangles[i] = new Rectangle[Rows];
        }

        CalculateLayout();
    }

    public void Update()
    {
        DraggingItemLogic();
    }

    // Delayed initilaization word around
    public void InitializePlayerInventory()
    {
        _inventory = _entityManager.EntitiesWithComponent<InventoryComponent>().FirstOrDefault().GetComponent<InventoryComponent>();
    }

    public void DropItem()
    {
        var item = _inventory.InventoryItems[_draggedItemCol][_draggedItemRow];
        var config = item.GetComponent<ItemComponent>().config;
        var pos = item.GetComponent<PositionComponent>();
        config.IsInOverworld = true;
        var (x, y) = _inputSystem.GetMouseLocationRelativeCamera(_camera);
        pos.X = x;
        pos.Y = y;
        item.GetComponent<CollisionComponent>().Hitbox = new Rectangle(x, y, CollectBoxSize, CollectBoxSize);
        _inventory.InventoryItems[_draggedItemCol][_draggedItemRow] = null;
        _entityManager.RefreshFilteredLists();
    }

    public void DraggingItemLogic()
    {
        if (_gameStateManager.CurrentGameState != GameState.Inventory)
            return;

        var drag = _inputSystem.GetMouseDragState(InputSystem.MouseButton.Left);

        if (drag.DragStarted)
        {
            var draggedItemIndices = IsHoveringSlot();
            if (draggedItemIndices.HasValue)
            {
                _draggedItemCol = draggedItemIndices.Value.Item1;
                _draggedItemRow = draggedItemIndices.Value.Item2;
                _draggedItem = _inventory.InventoryItems[_draggedItemCol][_draggedItemRow];
                if (_draggedItem != null)
                {
                    DraggedItem = _draggedItem;
                    _draggedItem.GetComponent<ItemComponent>().config.BeingDragged = true;
                    CurrentlyDragging = true;
                }
            }
        }

        // if (drag.IsDragging)     // For future use

        if (drag.DragEnded)
        {
            if (_draggedItem != null)
            {
                var hoveredItemIndices = IsHoveringSlot();
                if (hoveredItemIndices.HasValue && hoveredItemIndices != (_draggedItemCol, _draggedItemRow)) // Swap guard
                {
                    var inv = _inventory.InventoryItems;
                    var temp = inv[hoveredItemIndices.Value.Item1][hoveredItemIndices.Value.Item2];
                    inv[hoveredItemIndices.Value.Item1][hoveredItemIndices.Value.Item2] = inv[_draggedItemCol][_draggedItemRow];
                    inv[_draggedItemCol][_draggedItemRow] = temp;

                } else if (!hoveredItemIndices.HasValue) {
                    DropItem();
                }
                DraggedItem.GetComponent<ItemComponent>().config.BeingDragged = false;
            }
            _draggedItem = null;
            DraggedItem = null;
            CurrentlyDragging = false;
        }
    }

    public (int, int)? IsHoveringSlot()
    {
        var (x, y) = _inputSystem.GetMouseLocation();
        for (int i = 0; i < Cols; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                if (InventorySlotRectangles[i][j].Contains(x, y))
                    return (i, j);
            }
        }
        return null;
    }

    public void CalculateLayout()
    {
        int defaultSpacing = 20;
        int slotWidth = (int)(defaultSpacing * Constants.ScaleFactor);
        int x = (int)(_viewport.Width / 2 - slotWidth * 9 / 2 - 1 * Constants.ScaleFactor);
        int yHotbar = (int)(6 * (Constants.DefaultTileSize * Constants.ScaleFactor));
        int yInventory = (int)(0 * (Constants.DefaultTileSize * Constants.ScaleFactor));

        for (int i = 0; i < Cols; i++)
        {
            int xOffset = slotWidth * i;
            InventorySlotPositions[i][0] = new Vector2(x + xOffset, yHotbar);
            InventoryIconPositions[i][0] = new Vector2(InventorySlotPositions[i][0].X + IconOffset, InventorySlotPositions[i][0].Y + IconOffset);
            InventorySlotRectangles[i][0] = new Rectangle(x + xOffset, yHotbar, SlotSize, SlotSize);
            for (int j = FirstInventoryRowIndex; j < Rows; j++)
            {
                int yOffset = slotWidth * j;
                InventorySlotPositions[i][j] = new Vector2(x + xOffset, yInventory + yOffset);
                InventoryIconPositions[i][j] = new Vector2(InventorySlotPositions[i][j].X + IconOffset, InventorySlotPositions[i][j].Y + IconOffset);
                InventorySlotRectangles[i][j] = new Rectangle(x + xOffset, yInventory + yOffset, SlotSize, SlotSize);
            }

        }
    }
}