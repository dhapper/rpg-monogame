using System;
using Microsoft.Xna.Framework;

public class ArtisanSystem
{

    private InteractionSystem _interactionSystem;
    private EntityManager _entityManager;
    private InventorySystem _inventorySystem;

    public ArtisanSystem(EntityManager entityManager, InteractionSystem interactionSystem, InventorySystem inventorySystem)
    {
        _interactionSystem = interactionSystem;
        _entityManager = entityManager;
        _inventorySystem = inventorySystem;
    }

    public void ProcessCrop(Entity crop)
    {
        var machine = GetMachine();
        if (machine == null) { return; }
        var machineComp = machine.GetComponent<MachineComponent>();
        if (machineComp.Active) { return; }

        // check if crop is valid for machine

        // set machineComp vars
        machineComp.DaysToProcess = machineComp.Config.DaysToProcess;
        machineComp.CurrentDay = 0;
        machineComp.CurrentItem = crop;
        machineComp.Active = true;

        // update sprite
        var sprite = machine.GetComponent<SpriteComponent>();
        sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.X + Constants.DefaultTileSize, sprite.SourceRectangle.Y, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);

        // remove Item from inv
        var activeItemIndices = _inventorySystem.Inventory.activeItemIndices;
        // _entityManager.DeleteEntity(_inventorySystem.Inventory.InventoryItems[activeItemIndices.Item1][activeItemIndices.Item2]);
        // _inventorySystem.Inventory.InventoryItems[activeItemIndices.Item1][activeItemIndices.Item2] = null;

        _inventorySystem.RemoveOneFromSlot(activeItemIndices.Item1, activeItemIndices.Item2);
    }

    public bool GetProcessedCrop()
    {
        var machine = GetMachine();
        if (machine == null) { return false; }
        Console.WriteLine("null");
        var machineComp = machine.GetComponent<MachineComponent>();
        // if (!machineComp.Active) { return false; }
        // Console.WriteLine("active");
        if (!machineComp.ProcessComplete) { return false; }
        Console.WriteLine("complete");

        machineComp.ProcessComplete = false;

        // update machine sprite
        var sprite = machine.GetComponent<SpriteComponent>();
        sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.X - 2 * Constants.DefaultTileSize, sprite.SourceRectangle.Y, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);

        // put in inv
        var processedCrop = machineComp.CurrentItem;    // for crop specific good, idk how to implement that though
        var itemConfig = Constants.Machines.MachineToArtisanGood[machineComp.Config.Name];
        var item = ItemFactory.CreateItem(itemConfig, _entityManager);
        _inventorySystem.PlaceItemInInventory(item);
        _entityManager.RefreshFilteredLists();
        return true;

    }

    public Entity GetMachine()
    {
        var tile = _interactionSystem.GetTile(InputSystem.GetMouseLocation());
        if(tile == null) { return null; }
        var tileComp = tile.GetComponent<TileComponent>();
        foreach (var machine in _entityManager.MachineEntities)
        {
            var pos = machine.GetComponent<PositionComponent>();
            if (tileComp.Col == pos.Col && tileComp.Row == pos.Row)
                return machine;
        }
        return null;
    }

    public void UpdateMachineProgress()
    {
        foreach (var machine in _entityManager.MachineEntities)
        {
            var machineComp = machine.GetComponent<MachineComponent>();
            if (machineComp.Active)
            {
                machineComp.CurrentDay++;
                if (machineComp.CurrentDay >= machineComp.Config.DaysToProcess)
                {
                    machineComp.Active = false;
                    var sprite = machine.GetComponent<SpriteComponent>();
                    sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.X + Constants.DefaultTileSize, sprite.SourceRectangle.Y, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                    machineComp.ProcessComplete = true;
                }
            }
        }
    }
}