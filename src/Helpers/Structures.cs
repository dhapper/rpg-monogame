using System.Collections.Generic;

public class CropData
{
    public string Name { get; set; }
    public int Stage { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
}

public class EntitySaveData
{
    public string EntityType { get; set; } // "crop"
    public CropData EntityInfo { get; set; }
}

public class EntitySaveWrapper
{
    public int LocationIndex { get; set; }
    public List<EntitySaveData> Entities { get; set; }
}
