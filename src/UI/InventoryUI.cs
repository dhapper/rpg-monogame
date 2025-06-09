using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

using static Constants.UI.Inventory;

public class InventoryUI
{
    private Camera2D _camera;
    private Viewport _viewport;
    private EntityManager _entityManager;
    private InventoryComponent _inventory;
    private InputSystem _inputSystem;
    private GameStateManager _gameStateManager;

    public Vector2[][] InventorySlotPositions, InventoryIconPositions;
    public Item DraggedItem = null;
    public bool CurrentlyDragging = false;

    private int _draggedItemCol, _draggedItemRow;
    private Item _draggedItem;

    public InventoryUI(Camera2D camera, Viewport viewport, EntityManager entityManager, InputSystem inputSystem, GameStateManager gameStateManager)
    {
        _camera = camera;
        _viewport = viewport;
        _entityManager = entityManager;
        _inputSystem = inputSystem;
        _gameStateManager = gameStateManager;

        InventorySlotPositions = new Vector2[Cols][];
        InventoryIconPositions = new Vector2[Cols][];
        for (int i = 0; i < Cols; i++)
        {
            InventorySlotPositions[i] = new Vector2[Rows];
            InventoryIconPositions[i] = new Vector2[Rows];
        }
    }

    public void Update()
    {
        CalculateLayout();
        DraggingItemLogic();
    }

    // Delayed initilaization word around
    public void InitializePlayerInventory()
    {
        _inventory = _entityManager.EntitiesWithComponent<InventoryComponent>().FirstOrDefault().GetComponent<InventoryComponent>();
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
                    _draggedItem.BeingDragged = true;
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
                    Item temp = inv[hoveredItemIndices.Value.Item1][hoveredItemIndices.Value.Item2];
                    inv[hoveredItemIndices.Value.Item1][hoveredItemIndices.Value.Item2] = inv[_draggedItemCol][_draggedItemRow];
                    inv[_draggedItemCol][_draggedItemRow] = temp;

                }
                DraggedItem.BeingDragged = false;
            }
            _draggedItem = null;
            DraggedItem = null;
            CurrentlyDragging = false;
        }
    }

    public (int, int)? IsHoveringSlot()
    {
        var (x, y) = _inputSystem.GetMouseLocationRelativeCamera(_camera);

        for (int i = 0; i < Cols; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                var slotPos = InventorySlotPositions[i][j];
                Rectangle slotRect = new Rectangle((int)slotPos.X, (int)slotPos.Y, SlotSize, SlotSize);
                if (slotRect.Contains(x, y))
                {
                    return (i, j);
                }
            }
        }
        return null;
    }


    public void CalculateLayout()
    {
        int defaultSpacing = 20;
        int slotWidth = (int)(defaultSpacing * Constants.ScaleFactor);
        int x = (int)(_camera.Position.X + _viewport.Width / 2 - slotWidth * 9 / 2 - 1 * Constants.ScaleFactor);
        int yHotbar = (int)(_camera.Position.Y + 6 * (Constants.TileSize * Constants.ScaleFactor));
        int yInventory = (int)(_camera.Position.Y + 0 * (Constants.TileSize * Constants.ScaleFactor));

        for (int i = 0; i < Cols; i++)
        {
            int xOffset = slotWidth * i;
            InventorySlotPositions[i][0] = new Vector2(x + xOffset, yHotbar);
            InventoryIconPositions[i][0] = new Vector2(InventorySlotPositions[i][0].X + IconOffset, InventorySlotPositions[i][0].Y + IconOffset);
            for (int j = FirstInventoryRowIndex; j < Rows; j++)
            {
                int yOffset = slotWidth * j;
                InventorySlotPositions[i][j] = new Vector2(x + xOffset, yInventory + yOffset);
                InventoryIconPositions[i][j] = new Vector2(InventorySlotPositions[i][j].X + IconOffset, InventorySlotPositions[i][j].Y + IconOffset);
            }

        }
    }
}