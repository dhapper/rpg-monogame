using System;
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
    public InventoryComponent Inventory => _inventory;
    // private GameStateManager _gameStateManager;

    public Vector2[][] InventorySlotPositions, InventoryIconPositions;
    public Rectangle[][] InventorySlotRectangles;
    public Entity DraggedItem = null, DraggedItem2 = null;
    public bool CurrentlyDragging = false;

    private int _draggedItemCol, _draggedItemRow;
    private Entity _draggedItem;
    private (int, int)? _originalSlotPos;
    private bool _consectutiveSwap = false;

    public InventoryUI(Camera2D camera, Viewport viewport, EntityManager entityManager)
    {
        _camera = camera;
        _viewport = viewport;
        _entityManager = entityManager;
        // _gameStateManager = gameStateManager;

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
        // DraggingItemLogic();
        DraggingItemLogic2();
        HandleInputs();
    }

    private void HandleInputs()
    {
        var inputs = InputSystem.GetInputState();
        if (inputs.ToggleInventory)
        {
            GameStateManager.SetState(GameState.Playing);
        }
    }

    // Delayed initilaization word around
    public void InitializePlayerInventory(InventoryComponent inventory)
    {
        _inventory = inventory;
    }

    public void DropItem()
    {
        var item = _inventory.InventoryItems[_draggedItemCol][_draggedItemRow];
        var config = item.GetComponent<ItemComponent>().Config;
        var pos = item.GetComponent<PositionComponent>();
        config.IsInOverworld = true;
        var (x, y) = InputSystem.GetMouseLocationRelativeCamera(_camera);
        pos.X = x;
        pos.Y = y;
        item.GetComponent<CollisionComponent>().Hitbox = new Rectangle(x, y, CollectBoxSize, CollectBoxSize);
        _inventory.InventoryItems[_draggedItemCol][_draggedItemRow] = null;
        _entityManager.RefreshFilteredLists();
    }

    // public void DraggingItemLogic3()
    // {
    //     // checks
    //     if (GameStateManager.CurrentGameState != GameState.Inventory)   // idk if this is necessary
    //         return;
    //     var mouseClicked = InputSystem.IsMousePressed(InputSystem.MouseButton.Left);
    //     if (!mouseClicked) { return; }
    //     var slotPos = IsHoveringSlot();
    //     if (slotPos == null) { return; }

    //     // local vars
    //     // var targetItem = _inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2];
    //     // var targetItemComp = targetItem.GetComponent<ItemComponent>();
    //     // var draggedItemComp = DraggedItem2.GetComponent<ItemComponent>();

    //     // if already dragging item ***************************************************************
    //     if (CurrentlyDragging)
    //     {
    //         //  // if empty, place ********************************************************************
    //         if (_inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2] == null)
    //         {
    //             _inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2] = DraggedItem2;
    //             CurrentlyDragging = false;
    //             DraggedItem2 = null;
    //             Console.WriteLine("Placed in emtpy slot");
    //             if (!_consectutiveSwap)
    //                 _inventory.InventoryItems[_originalSlotPos.Value.Item1][_originalSlotPos.Value.Item2] = null;
    //             _consectutiveSwap = false;
    //         }
    //         //  // else if same item + stackable ******************************************************
    //         else if (draggedItemComp.Config.Stackable && draggedItemComp.Config.Name == targetItemComp.Config.Name)
    //         {
    //             //  //  // if less than stack *************************************************************
    //             //  //  // if more then stack *************************************************************
    //         }
    //         //  // else if unstackable/different item, swap *******************************************
    //         else
    //         {
    //             var temp = _inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2];
    //             _inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2] = DraggedItem2;
    //             DraggedItem2 = temp;
    //             Console.WriteLine("Swapped");
    //             if (!_consectutiveSwap)
    //                 _inventory.InventoryItems[_originalSlotPos.Value.Item1][_originalSlotPos.Value.Item2] = null;
    //             _originalSlotPos = slotPos;
    //             _consectutiveSwap = true;
    //         }
    //     }
    //     // else not already dragging item *********************************************************
    //     else
    //     {
    //         DraggedItem2 = _inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2];
    //         if (DraggedItem2 != null)
    //         {
    //             CurrentlyDragging = true;
    //             _originalSlotPos = slotPos;
    //             // _inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2] = null;
    //             Console.WriteLine(DraggedItem2.GetComponent<ItemComponent>().Config.Name);
    //         }
    //         else
    //         {
    //             CurrentlyDragging = false;
    //         }
    //     }

    // }

    // public void DraggingItemLogic2()
    // {
    //     // idk if this is necessary
    //     if (GameStateManager.CurrentGameState != GameState.Inventory)
    //         return;


    //     var mouseClicked = InputSystem.IsMousePressed(InputSystem.MouseButton.Left);
    //     if (!mouseClicked) { return; }
    //     var slotPos = IsHoveringSlot();

    //     // check to see if item is already dragged
    //     // swap, place or drop

    //     if (slotPos == null) { return; }

    //     if (CurrentlyDragging)
    //     {
    //         // empty slot
    //         if (_inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2] == null)
    //         {
    //             _inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2] = DraggedItem2;
    //             CurrentlyDragging = false;
    //             DraggedItem2 = null;
    //             Console.WriteLine("Placed in emtpy slot");
    //             if (!_consectutiveSwap)
    //                 _inventory.InventoryItems[_originalSlotPos.Value.Item1][_originalSlotPos.Value.Item2] = null;
    //             _consectutiveSwap = false;
    //         }
    //         // occupied slot
    //         else if (_inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2] != null)
    //         {
    //             // var draggedItem =
    //             var targetItem = _inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2];
    //             var targetItemComp = targetItem.GetComponent<ItemComponent>();
    //             var draggedItemComp = DraggedItem2.GetComponent<ItemComponent>();
    //             if (draggedItemComp.Config.Stackable && draggedItemComp.Config.Name == targetItemComp.Config.Name)
    //             {
    //                 // if quantity + quantity < stackLimit
    //                 var lessThanStack = draggedItemComp.Quantity + targetItemComp.Quantity <= draggedItemComp.Config.StackLimit;

    //                 if (lessThanStack)
    //                 {
    //                     targetItemComp.Quantity += draggedItemComp.Quantity;
    //                     // DraggedItem2 = null;
    //                     // _inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2] = DraggedItem2;
    //                     CurrentlyDragging = false;
    //                     DraggedItem2 = null;

    //                     if (!_consectutiveSwap)
    //                         _inventory.InventoryItems[_originalSlotPos.Value.Item1][_originalSlotPos.Value.Item2] = null;
    //                     _consectutiveSwap = false;
    //                 }
    //                 else
    //                 {

    //                     var difference = draggedItemComp.Config.StackLimit - targetItemComp.Quantity;
    //                     targetItemComp.Quantity = draggedItemComp.Config.StackLimit;
    //                     draggedItemComp.Quantity -= difference;

    //                     if (!_consectutiveSwap)
    //                         _inventory.InventoryItems[_originalSlotPos.Value.Item1][_originalSlotPos.Value.Item2] = null;
    //                     _consectutiveSwap = false;
    //                 }
    //                 _consectutiveSwap = true;

    //             }
    //             else
    //             {
    //                 var temp = _inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2];
    //                 _inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2] = DraggedItem2;
    //                 DraggedItem2 = temp;
    //                 Console.WriteLine("Swapped");
    //                 if (!_consectutiveSwap)
    //                     _inventory.InventoryItems[_originalSlotPos.Value.Item1][_originalSlotPos.Value.Item2] = null;
    //                 _originalSlotPos = slotPos;
    //                 _consectutiveSwap = true;
    //             }
    //         }
    //     }
    //     // check to see if item is clicked?
    //     // set to draggedItem?
    //     else
    //     {
    //         DraggedItem2 = _inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2];
    //         if (DraggedItem2 != null)
    //         {
    //             CurrentlyDragging = true;
    //             _originalSlotPos = slotPos;
    //             // _inventory.InventoryItems[slotPos.Value.Item1][slotPos.Value.Item2] = null;
    //             Console.WriteLine(DraggedItem2.GetComponent<ItemComponent>().Config.Name);
    //         }
    //         else
    //         {
    //             CurrentlyDragging = false;
    //         }
    //     }
    // }

    public void DraggingItemLogic2()
    {
        // Only allow dragging when in the Inventory state
        if (GameStateManager.CurrentGameState != GameState.Inventory)
            return;

        // Only run logic when left mouse is pressed
        if (!InputSystem.IsMousePressed(InputSystem.MouseButton.Left))
            return;

        // Get the currently hovered slot (row, column)
        var slotPos = IsHoveringSlot();
        if (slotPos == null)
            return;

        // Extract slot coordinates
        int row = slotPos.Value.Item1;
        int col = slotPos.Value.Item2;
        var slotItem = _inventory.InventoryItems[row][col];

        // CASE 1: Already dragging an item
        if (CurrentlyDragging && DraggedItem2 != null)
        {
            var draggedComp = DraggedItem2.GetComponent<ItemComponent>();

            // CASE 1a: Empty slot → place the item
            if (slotItem == null)
            {
                Console.WriteLine("Placed in empty slot");
                _inventory.InventoryItems[row][col] = DraggedItem2;
                EndDrag();
                return;
            }

            // CASE 1b: Occupied slot → check stacking or swap
            var targetComp = slotItem.GetComponent<ItemComponent>();

            // CASE 1b-i: Stackable and same item
            if (draggedComp.Config.Stackable && draggedComp.Config.Name == targetComp.Config.Name)
            {
                int totalQuantity = draggedComp.Quantity + targetComp.Quantity;

                // CASE: All items fit into target stack
                if (totalQuantity <= draggedComp.Config.StackLimit)
                {
                    Console.WriteLine("Stacked items fully");
                    targetComp.Quantity = totalQuantity;
                    EndDrag();
                }
                // CASE: Only partial stack possible
                else
                {
                    Console.WriteLine("Stacked partially, overflow remains");
                    int spaceLeft = draggedComp.Config.StackLimit - targetComp.Quantity;
                    targetComp.Quantity = draggedComp.Config.StackLimit;
                    draggedComp.Quantity -= spaceLeft;

                    // Do not end drag if there's still remainder
                    if (!_consectutiveSwap)
                        _inventory.InventoryItems[_originalSlotPos.Value.Item1][_originalSlotPos.Value.Item2] = null;

                    _consectutiveSwap = false;
                }

                _consectutiveSwap = true;
            }
            else
            {
                // CASE 1b-ii: Not stackable or different item → swap
                Console.WriteLine("Swapped items");
                _inventory.InventoryItems[row][col] = DraggedItem2;
                DraggedItem2 = slotItem;

                if (!_consectutiveSwap)
                    _inventory.InventoryItems[_originalSlotPos.Value.Item1][_originalSlotPos.Value.Item2] = null;

                _originalSlotPos = slotPos;
                _consectutiveSwap = true;
            }
        }
        // CASE 2: Not dragging → Start dragging the clicked item
        else
        {
            DraggedItem2 = slotItem;
            if (DraggedItem2 != null)
            {
                CurrentlyDragging = true;
                _originalSlotPos = slotPos;
                Console.WriteLine($"Started dragging: {DraggedItem2.GetComponent<ItemComponent>().Config.Name}");
            }
            else
            {
                CurrentlyDragging = false;
            }
        }
    }

    private void EndDrag()
    {
        CurrentlyDragging = false;
        DraggedItem2 = null;

        if (!_consectutiveSwap && _originalSlotPos.HasValue)
        {
            _inventory.InventoryItems[_originalSlotPos.Value.Item1][_originalSlotPos.Value.Item2] = null;
        }

        _consectutiveSwap = false;
    }


    public void DraggingItemLogic()
    {
        if (GameStateManager.CurrentGameState != GameState.Inventory)
            return;

        var drag = InputSystem.GetMouseDragState(InputSystem.MouseButton.Left);

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
                    _draggedItem.GetComponent<ItemComponent>().Config.BeingDragged = true;
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

                    // add to stack?


                    var inv = _inventory.InventoryItems;
                    var temp = inv[hoveredItemIndices.Value.Item1][hoveredItemIndices.Value.Item2];
                    inv[hoveredItemIndices.Value.Item1][hoveredItemIndices.Value.Item2] = inv[_draggedItemCol][_draggedItemRow];
                    inv[_draggedItemCol][_draggedItemRow] = temp;

                }
                else if (!hoveredItemIndices.HasValue)
                {
                    DropItem();
                }
                DraggedItem.GetComponent<ItemComponent>().Config.BeingDragged = false;
            }
            _draggedItem = null;
            DraggedItem = null;
            CurrentlyDragging = false;
        }
    }

    public (int, int)? IsHoveringSlot()
    {
        var (x, y) = InputSystem.GetMouseLocation();
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
        yHotbar = _viewport.Height - 2 * Constants.TileSize;
        int yInventory = (int)(0 * (Constants.DefaultTileSize * Constants.ScaleFactor));
        yInventory = _viewport.Height - 8 * Constants.TileSize;

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