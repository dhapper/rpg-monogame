using System;

public class InventorySystem
{

    private EntityManager _entityManager;

    private InventoryComponent _inventory;
    public InventoryComponent Inventory => _inventory;

    public int Coins = 500;

    public InventorySystem(EntityManager entityManager)
    {
        _entityManager = entityManager;
    }

    public void InitInventory(InventoryComponent inventory)
    {
        _inventory = inventory;

        var wateringCan = ItemFactory.CreateItem(Constants.Items.Config.WateringCan, _entityManager);
        var pickaxe = ItemFactory.CreateItem(Constants.Items.Config.Pickaxe, _entityManager);
        var seeds1 = ItemFactory.CreateItem(Constants.Items.Config.PumpkinSeeds, _entityManager);
        var seeds2 = ItemFactory.CreateItem(Constants.Items.Config.PotatoSeeds, _entityManager);
        var seeds3 = ItemFactory.CreateItem(Constants.Items.Config.PotatoSeeds, _entityManager);
        var m1 = ItemFactory.CreateItem(Constants.Items.Config.Juicer, _entityManager);
        var m2 = ItemFactory.CreateItem(Constants.Items.Config.JamJar, _entityManager);
        var m3 = ItemFactory.CreateItem(Constants.Items.Config.PickleJar, _entityManager);
        var m4 = ItemFactory.CreateItem(Constants.Items.Config.Keg, _entityManager);
        var potato1 = ItemFactory.CreateItem(Constants.Items.Config.Potato.Clone(), _entityManager);
        var potato2 = ItemFactory.CreateItem(Constants.Items.Config.Potato.Clone(), _entityManager);
        var potato3 = ItemFactory.CreateItem(Constants.Items.Config.Potato.Clone(), _entityManager);
        var potato4 = ItemFactory.CreateItem(Constants.Items.Config.Potato.Clone(), _entityManager);
        var potato5 = ItemFactory.CreateItem(Constants.Items.Config.Potato.Clone(), _entityManager);
        var potato6 = ItemFactory.CreateItem(Constants.Items.Config.Potato.Clone(), _entityManager);
        var potato7 = ItemFactory.CreateItem(Constants.Items.Config.Potato.Clone(), _entityManager);
        var potato8 = ItemFactory.CreateItem(Constants.Items.Config.Potato.Clone(), _entityManager);
        var potato9 = ItemFactory.CreateItem(Constants.Items.Config.Potato.Clone(), _entityManager);

        potato1.GetComponent<ItemComponent>().Quantity = 8;
        potato2.GetComponent<ItemComponent>().Quantity = 6;

        inventory.InventoryItems[0][2] = wateringCan;
        inventory.InventoryItems[1][2] = pickaxe;
        inventory.InventoryItems[2][2] = seeds1;
        inventory.InventoryItems[7][2] = seeds2;
        inventory.InventoryItems[3][2] = seeds3;

        inventory.InventoryItems[0][0] = m1;
        inventory.InventoryItems[1][0] = m2;
        inventory.InventoryItems[2][0] = m3;
        inventory.InventoryItems[3][0] = m4;

        inventory.InventoryItems[0][1] = potato1;
        inventory.InventoryItems[1][1] = potato2;
        inventory.InventoryItems[2][1] = potato3;
        inventory.InventoryItems[3][1] = potato4;
        inventory.InventoryItems[4][1] = potato5;
        inventory.InventoryItems[5][1] = potato6;
        inventory.InventoryItems[6][1] = potato7;
        inventory.InventoryItems[7][1] = potato8;
        inventory.InventoryItems[8][1] = potato9;
    }

    public (int j, int i)? GetNextEmptySlot()
    {
        for (int i = 0; i < Constants.UI.Inventory.Rows; i++)
        {
            for (int j = 0; j < Constants.UI.Inventory.Cols; j++)
            {
                if (_inventory.InventoryItems[j][i] == null)
                    return (j, i);
            }
        }
        return null;
    }

    public bool PlaceItemInInventory(Entity item)
    {
        if (item.GetComponent<ItemComponent>().Config.Stackable)
        {
            if (PlaceInAvailableStack(item))
            {
                return true;
            }
        }

        var emptySlot = GetNextEmptySlot();
        if (emptySlot != null)
        {
            _inventory.InventoryItems[emptySlot.Value.j][emptySlot.Value.i] = item;
            return true;
        }

        // drop item?
        return false;
    }

    public bool PlaceInAvailableStack(Entity item)
    {
        var itemComp = item.GetComponent<ItemComponent>();
        for (int i = 0; i < Constants.UI.Inventory.Rows; i++)
        {
            for (int j = 0; j < Constants.UI.Inventory.Cols; j++)
            {
                if (_inventory.InventoryItems[j][i] == null) { continue; }
                var slotItem = _inventory.InventoryItems[j][i].GetComponent<ItemComponent>();
                if (slotItem.Config.Name == itemComp.Config.Name)
                {
                    // var additionalQuantity = itemComp.Quantity;
                    if (slotItem.Quantity + itemComp.Quantity <= itemComp.Config.StackLimit)
                    {
                        // Console.WriteLine(itemComp.Quantity);
                        slotItem.Quantity += itemComp.Quantity;
                        return true;
                    }
                    else if (slotItem.Quantity != itemComp.Config.StackLimit)
                    {
                        var addedToStack = itemComp.Config.StackLimit - slotItem.Quantity;
                        itemComp.Quantity -= addedToStack;
                        // Console.WriteLine(itemComp.Config.StackLimit + " | " + slotItem.Quantity);
                        slotItem.Quantity = itemComp.Config.StackLimit;
                        if(itemComp.Quantity == 0) { return true; }
                    }
                    // else
                    // {
                    //     // not sure about this stacking logic
                    //     itemComp.Quantity = itemComp.Config.Capacity - slotItem.Quantity;
                    //     slotItem.Quantity = itemComp.Config.Capacity; 
                    //     return false;
                    // }
                }
            }
        }
        return false;
    }

    public void PlaceInNextEmptySlot(Entity item)
    {
        var slots = GetNextEmptySlot();
        if (slots == null) { return; }
        _inventory.InventoryItems[slots.Value.j][slots.Value.i] = item;
    }

    public void PickUp(Entity entity)
    {
        var hitbox = entity.GetComponent<CollisionComponent>().Hitbox;
        foreach (var item in _entityManager.DroppedOverworldItems)
        {
            var inCollectionBox = item.GetComponent<CollisionComponent>().Hitbox.Intersects(hitbox);
            if (inCollectionBox)
            {
                var itemComp = item.GetComponent<ItemComponent>(); 
                Console.WriteLine("Picking up item | Stackable: "+itemComp.Config.Stackable+" | Quantity: "+itemComp.Quantity);
                // var quantity = item.GetComponent
                PlaceItemInInventory(item);
                itemComp.Config.IsInOverworld = false;
                _entityManager.RefreshFilteredLists();
                return;

                // var slots = GetNextEmptySlot();
                // if (_inventory.InventoryItems[slots.Value.j][slots.Value.i] == null)
                // {
                //     item.GetComponent<ItemComponent>().Config.IsInOverworld = false;
                //     _inventory.InventoryItems[slots.Value.j][slots.Value.i] = item;
                //     _entityManager.RefreshFilteredLists();
                //     return;
                // }
            }
        }
    }

    public void RemoveOneFromSlot(int col, int row)
    {
        var item = _inventory.InventoryItems[col][row];
        var itemComp = item.GetComponent<ItemComponent>();

        if (itemComp.Config.Stackable && itemComp.Quantity > 0)
        {
            itemComp.Quantity--;
        }

        if (!itemComp.Config.Stackable || itemComp.Quantity <= 0)
        {
            Console.WriteLine("deleting item from inv");
            _entityManager.DeleteEntity(_inventory.InventoryItems[col][row]);
            _inventory.InventoryItems[col][row] = null;
        }

        // Console.WriteLine(itemComp.Quantity--);
    }


}