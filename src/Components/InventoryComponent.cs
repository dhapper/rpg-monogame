using System.Collections.Generic;

public class InventoryComponent
{
    public List<Item> HotbarItems { get; set; }
    public List<Item> InventoryItems { get; set; }
    public Item activeItem;

    public InventoryComponent()
    {
        HotbarItems = new List<Item>();
        InventoryItems = new List<Item>();

        Item item1 = Constants.Items.WateringCan;
        Item item2 = Constants.Items.Pickaxe;
        Item item3 = Constants.Items.Seeds;
        Item empty = Constants.Items.Empty;

        HotbarItems.Add(item1);
        HotbarItems.Add(item2);
        HotbarItems.Add(item3);
        HotbarItems.Add(item1);
        HotbarItems.Add(empty);
        HotbarItems.Add(empty);
        HotbarItems.Add(empty);
        HotbarItems.Add(empty);
        HotbarItems.Add(empty);
        activeItem = HotbarItems[1];

        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(item1);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);

        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(item2);
        InventoryItems.Add(empty);

        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(item3);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
        InventoryItems.Add(empty);
    }
}