using System.Collections.Generic;

public class MapEntityStorage
{
    private Dictionary<string, EntityManager> _mapEntities = new();

    public EntityManager GetEntityManager(string mapName)
    {
        if (!_mapEntities.ContainsKey(mapName))
            _mapEntities[mapName] = new EntityManager();
        return _mapEntities[mapName];
    }

    public void SaveEntities(string mapName)
    {
        var entities = _mapEntities[mapName].GetEntities();
        // Serialize and save them to disk if needed
    }

    public void LoadEntities(string mapName)
    {
        if (!_mapEntities.ContainsKey(mapName))
        {
            _mapEntities[mapName] = new EntityManager();
            // Load entities from file if available
        }
    }
}
