public class InventorySystem
{

    private EntityManager _entityManager;
    private InventoryComponent _inventory;

    public InventorySystem(EntityManager entityManager)
    {
        _entityManager = entityManager;
    }

    public void InitInventory(InventoryComponent inventory)
    {
        _inventory = inventory;

        var wateringCan = ItemFactory.CreateItem(Constants.Items.WateringCan, _entityManager);
        var pickaxe = ItemFactory.CreateItem(Constants.Items.Pickaxe, _entityManager);
        var seeds1 = ItemFactory.CreateItem(Constants.Items.PumpkinSeeds, _entityManager);
        var seeds2 = ItemFactory.CreateItem(Constants.Items.PotatoSeeds, _entityManager);
        var seeds3 = ItemFactory.CreateItem(Constants.Items.PotatoSeeds, _entityManager);

        inventory.InventoryItems[0][0] = wateringCan;
        inventory.InventoryItems[1][0] = pickaxe;
        inventory.InventoryItems[2][0] = seeds1;
        inventory.InventoryItems[7][2] = seeds2;
        inventory.InventoryItems[3][0] = seeds3;
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

    public void PlaceInNextEmptySlot(Entity item)
    {
        var slots = GetNextEmptySlot();
        if(slots == null) { return; }
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
                var slots = GetNextEmptySlot();

                if (_inventory.InventoryItems[slots.Value.j][slots.Value.i] == null)
                {
                    item.GetComponent<ItemComponent>().config.IsInOverworld = false;
                    _inventory.InventoryItems[slots.Value.j][slots.Value.i] = item;
                    _entityManager.RefreshFilteredLists();
                    return;
                }
            }
        }
    }


}