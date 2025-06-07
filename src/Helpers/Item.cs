public class Item
{
    public string Name { get; set; }
    public ItemType Type { get; set; }
}

public enum ItemType { Tool, Plantable, Empty }
