using System.Collections.Generic;

public class InventoryComponent
{
    public List<Item> Items { get; set; }
    public Item activeItem;

    public InventoryComponent(List<Item> items = null)
    {
        Items = items ?? new List<Item>();

        // Declare two dummy items
        Item item1 = Constants.Items.WateringCan;
        Item item2 = Constants.Items.Pickaxe;
        Item item3 = Constants.Items.Seeds;
        Item empty = Constants.Items.Empty;

        // Add the items to the list
        Items.Add(item1);
        Items.Add(item2);
        Items.Add(item3);
        Items.Add(item1);

        Items.Add(empty);
        Items.Add(empty);
        Items.Add(empty);
        Items.Add(empty);
        Items.Add(empty);



        // Set activeItem safely
        activeItem = Items[1];
    }
}