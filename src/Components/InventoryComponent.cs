using System.Collections.Generic;

public class InventoryComponent
{
    public Entity[][] InventoryItems;
    public (int, int) activeItemIndices;


    public InventoryComponent()
    {

        InventoryItems = new Entity[9][];
        for (int i = 0; i < 9; i++)
        {
            InventoryItems[i] = new Entity[4];
        }

        activeItemIndices = (1, 0);

    }
}