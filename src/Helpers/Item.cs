using Microsoft.Xna.Framework;

public class Item
{
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public Rectangle SourceRectangle;

    public Item(string name, ItemType type, Rectangle sourceRectangle)
    {
        Name = name;
        Type = type;
        SourceRectangle = sourceRectangle;
    }

}

public enum ItemType { Tool, Plantable, Empty }
