using System;
using Microsoft.Xna.Framework;

public class ItemConfig
{
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public Rectangle SourceRectangle;
    public bool BeingDragged = false;
    public bool IsInOverworld = false;
    // public (int x, int y) OverworldPosition;

    public bool Stackable = false;
    public int StackLimit;
    public int Capacity;

    public ItemConfig(string name, ItemType type, Rectangle sourceRectangle, int stackLimit = -1, int capacity = -1)
    {
        Name = name;
        Type = type;
        SourceRectangle = sourceRectangle;

        StackLimit = stackLimit;
        Capacity = capacity;

        if (stackLimit != -1)
        {
            Stackable = true;
            StackLimit = stackLimit;
        }

        if (capacity != -1)
        {
            Capacity = capacity;
        }
    }

    public ItemConfig Clone()
    {
        return new ItemConfig(this.Name, this.Type, this.SourceRectangle, this.StackLimit, this.Capacity);
    }

    public bool IsSameItem(ItemConfig item)
    {
        if (this.Name == item.Name && this.Type == item.Type)
            return true;
        return false;
    }
    

}

public enum ItemType
{
    Tool,
    Plantable,
    Crop,
    Artisan,
    Machine
}
