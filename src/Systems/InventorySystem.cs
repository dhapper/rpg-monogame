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
        var seeds1 = ItemFactory.CreateItem(Constants.Items.Seeds, _entityManager, _assets);
        var seeds2 = ItemFactory.CreateItem(Constants.Items.Seeds, _entityManager, _assets);

        inventory.InventoryItems[0][0] = wateringCan;
        inventory.InventoryItems[1][0] = pickaxe;
        inventory.InventoryItems[2][0] = seeds1;
        inventory.InventoryItems[7][2] = seeds2;
    }
    

}