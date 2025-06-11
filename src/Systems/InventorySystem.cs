public class InventorySystem
{

    private EntityManager _entityManager;
    private AssetStore _assets;

    public InventorySystem(EntityManager entityManager, AssetStore assets)
    {
        _entityManager = entityManager;
        _assets = assets;
    }

    public void InitInventory(InventoryComponent inventory)
    {
        var wateringCan = ItemFactory.CreateItem(Constants.Items.WateringCan, _entityManager, _assets);
        var pickaxe = ItemFactory.CreateItem(Constants.Items.Pickaxe, _entityManager, _assets);
        var seeds1 = ItemFactory.CreateItem(Constants.Items.PumpkinSeeds, _entityManager, _assets);
        var seeds2 = ItemFactory.CreateItem(Constants.Items.PotatoSeeds, _entityManager, _assets);

        inventory.InventoryItems[0][0] = wateringCan;
        inventory.InventoryItems[1][0] = pickaxe;
        inventory.InventoryItems[2][0] = seeds1;
        inventory.InventoryItems[7][2] = seeds2;
        inventory.InventoryItems[3][0] = seeds2;
        inventory.InventoryItems[4][0] = seeds2;
    }

    public (int j, int i)? GetNextEmptySlot(InventoryComponent inventory)
    {
        for (int i = 0; i < Constants.UI.Inventory.Rows; i++)
        {
            for (int j = 0; j < Constants.UI.Inventory.Cols; j++)
            {
                if (inventory.InventoryItems[j][i] == null)
                {
                    return (j, i);
                }
            }
        }
        return null;
    }

    public void PickUp(Entity entity)
    {
        var hitbox = entity.GetComponent<CollisionComponent>().Hitbox;
        foreach (var item in _entityManager.DroppedOverworldItems)
        {
            var inCollectionBox = item.GetComponent<CollisionComponent>().Hitbox.Intersects(hitbox);
            if (inCollectionBox)
            {
                for (int i = 0; i < Constants.UI.Inventory.Rows; i++)
                {
                    for (int j = 0; j < Constants.UI.Inventory.Cols; j++)
                    {
                        var inv = entity.GetComponent<InventoryComponent>();
                        if (inv.InventoryItems[j][i] == null)
                        {
                            item.GetComponent<ItemComponent>().config.IsInOverworld = false;
                            inv.InventoryItems[j][i] = item;
                            _entityManager.RefreshFilteredLists();
                            return;
                        }
                    }
                }
            }
        }
    }


}