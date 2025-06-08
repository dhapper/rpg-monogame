using System.Collections.Generic;

public class InventoryComponent
{
    // public List<Item> HotbarItems { get; set; }
    // public List<Item> InventoryItems { get; set; }

    public Item[] HotbarItems;
    public Item[][] InventoryItems;
    public Item activeItem;

    public InventoryComponent()
    {
        // HotbarItems = new List<Item>();
        // InventoryItems = new List<Item>();

        HotbarItems = new Item[9];
        InventoryItems = new Item[9][];
        for (int i = 0; i < 9; i++)
        {
            InventoryItems[i] = new Item[3];
        }

        Item item1 = Constants.Items.WateringCan;
        Item item2 = Constants.Items.Pickaxe;
        Item item3 = Constants.Items.Seeds;

        HotbarItems[0] = item1;
        HotbarItems[1] = item2;
        HotbarItems[2] = item3;

        activeItem = HotbarItems[0];

        InventoryItems[6][0] = item3;
        InventoryItems[0][1] = item2;
        InventoryItems[7][2] = item1;

    }
}