using Microsoft.Xna.Framework;

public class ItemConfig
{
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public Rectangle SourceRectangle;
    public bool BeingDragged = false;
    public bool IsInOverworld = false;
    // public (int x, int y) OverworldPosition;

    public ItemConfig(string name, ItemType type, Rectangle sourceRectangle)
    {
        Name = name;
        Type = type;
        SourceRectangle = sourceRectangle;
    }

    public ItemConfig Clone()
    {
        return new ItemConfig(this.Name, this.Type, this.SourceRectangle);
    }

    public bool IsSameItem(ItemConfig item)
    {
        if (this.Name == item.Name && this.Type == item.Type)
            return true;
        return false;
    }
    

}

public enum ItemType { Tool, Plantable, Empty }
