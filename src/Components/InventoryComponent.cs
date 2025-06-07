using System.Collections.Generic;

public class InventoryComponent
{
    public List<Item> Items { get; set; }
    public Item activeItem;

    public InventoryComponent(List<Item> items = null)
    {
        Items = items ?? new List<Item>();

        // Declare two dummy items
        Item item1 = new Item { Name = "WateringCan", Type = ItemType.Tool };
        Item item2 = new Item { Name = "Pickaxe", Type = ItemType.Tool };
        Item item3 = new Item { Name = "seeds", Type = ItemType.Plantable };
        Item empty = new Item { Name = null, Type = ItemType.Empty };

        // Add the items to the list
        Items.Add(empty);

        Items.Add(item1);
        Items.Add(item2);
        Items.Add(item3);

        Items.Add(empty);
        Items.Add(empty);
        Items.Add(empty);

        Items.Add(empty);
        Items.Add(empty);
        Items.Add(empty);

        // Set activeItem safely
        activeItem = Items[1];
    }
}