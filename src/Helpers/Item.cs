using Microsoft.Xna.Framework;

public class Item
{
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public Rectangle SourceRectangle;
    public bool BeingDragged = false;

    public Item(string name, ItemType type, Rectangle sourceRectangle)
    {
        Name = name;
        Type = type;
        SourceRectangle = sourceRectangle;
    }

    public Item Clone()
    {
        return new Item(this.Name, this.Type, this.SourceRectangle);
    }

    public bool IsSameItem(Item item)
    {
        if (this.Name == item.Name && this.Type == item.Type)
            return true;
        return false;
    }
    

}

public enum ItemType { Tool, Plantable, Empty }
