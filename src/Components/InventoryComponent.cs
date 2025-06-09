using System.Collections.Generic;

public class InventoryComponent
{
    // public List<Item> HotbarItems { get; set; }
    // public List<Item> InventoryItems { get; set; }

    // public Item[] HotbarItems;
    public Item[][] InventoryItems;
    // public Item activeItem;
    public (int, int) activeItemIndices;

    public InventoryComponent()
    {
        // HotbarItems = new List<Item>();
        // InventoryItems = new List<Item>();

        // HotbarItems = new Item[9];
        InventoryItems = new Item[9][];
        for (int i = 0; i < 9; i++)
        {
            InventoryItems[i] = new Item[4];
        }

        Item item1 = Constants.Items.WateringCan;
        Item item2 = Constants.Items.Pickaxe;
        Item item3 = Constants.Items.Seeds;

        InventoryItems[0][0] = item1.Clone();
        InventoryItems[1][0] = item2.Clone();
        InventoryItems[2][0] = item3.Clone();
        InventoryItems[3][0] = item3.Clone();

        // activeItem = InventoryItems[1][0];
        activeItemIndices = (1, 0);

        InventoryItems[6][1] = item3.Clone();
        InventoryItems[0][2] = item2.Clone();
        InventoryItems[7][3] = item1.Clone();

    }
}