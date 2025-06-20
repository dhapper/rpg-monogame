using System;
using System.Collections.Generic;

public class EntityManager
{
    private int _nextId = 0;
    private List<Entity> _entities = new();
    public List<Entity> Entities => _entities;

    private List<Entity> _tileEntities = new List<Entity>();
    private List<Entity> _spriteEntities = new List<Entity>();
    private List<Entity> _dropperdOverworldItems = new List<Entity>();
    private List<Entity> _cropEntities = new List<Entity>();
    private List<Entity> _machineEntities = new List<Entity>();
    private List<Entity> _zones = new List<Entity>();
    private List<Entity> _placedEntities = new List<Entity>();

    public IReadOnlyList<Entity> TileEntities => _tileEntities.AsReadOnly();
    public IReadOnlyList<Entity> SpriteEntities => _spriteEntities.AsReadOnly();
    public IReadOnlyList<Entity> DroppedOverworldItems => _dropperdOverworldItems.AsReadOnly();
    public IReadOnlyList<Entity> CropEntities => _cropEntities.AsReadOnly();
    public IReadOnlyList<Entity> MachineEntities => _machineEntities.AsReadOnly();
    public IReadOnlyList<Entity> Zones => _zones.AsReadOnly();
    public IReadOnlyList<Entity> PlacedEntities => _placedEntities.AsReadOnly();
    
    public void RefreshFilteredLists()  // not zones
    {
        _tileEntities.Clear();
        _spriteEntities.Clear();
        _dropperdOverworldItems.Clear();
        _cropEntities.Clear();
        _machineEntities.Clear();
        _placedEntities.Clear();

        foreach (var entity in _entities)
        {
            if (entity.HasComponent<TileComponent>())
                _tileEntities.Add(entity);
            if (entity.HasComponent<PositionComponent>() && entity.HasComponent<SpriteComponent>() && entity.HasComponent<CharacterComponent>())
                _spriteEntities.Add(entity);
            if (entity.HasComponent<ItemComponent>() && entity.GetComponent<ItemComponent>().Config.IsInOverworld)
                _dropperdOverworldItems.Add(entity);
            if (entity.HasComponent<CropComponent>())
                _cropEntities.Add(entity);
            if (entity.HasComponent<MachineComponent>())
                _machineEntities.Add(entity);
            if (entity.HasComponent<MachineComponent>() || entity.HasComponent<CropComponent>())
                _placedEntities.Add(entity);
            // if (entity.HasComponent<ZoneComponent>())
            //     _zones.Add(entity);

        }
    }

    public void ClearTileEntities()
    {
        foreach (var tile in _tileEntities)
        {
            _entities.Remove(tile);
        }
        RefreshFilteredLists();
    }

    public void ClearZones()
    {
        foreach (var zone in _zones)
        {
            _entities.Remove(zone);
        }
        RefreshZones();
    }

    public void RefreshZones()
    {
        _zones.Clear();
        foreach (var entity in _entities)
        {
            if (entity.HasComponent<ZoneComponent>())
                _zones.Add(entity);
        }
    }

    // maybe move this to child class?
    public void ChangeTiles(string oldSheet, int oldId, string newSheet, int newId)
    {
        for (int i = 0; i < _tileEntities.Count; i++)
        {
            var tile = _tileEntities[i];
            var tileComp = tile.GetComponent<TileComponent>();
            if (tileComp.Type == oldSheet && tileComp.Id == oldId)
            {
                var col = tileComp.Col;
                var row = tileComp.Row;

                _entities.Remove(tile);

                var newTile = TileFactory.CreateTile(newId, newSheet, null, (int)row, (int)col, this);
                _tileEntities[i] = newTile;
            }
        }
        RefreshFilteredLists();
    }

    public void ChangeTile(Entity selectedTile, string newSheet, int newId)
    {
        for (int i = 0; i < _tileEntities.Count; i++)
        {
            var tile = _tileEntities[i];
            if (tile == selectedTile)
            {
                var tileComp = tile.GetComponent<TileComponent>();
                var col = tileComp.Col;
                var row = tileComp.Row;

                _entities.Remove(tile);
                _tileEntities.RemoveAt(i);

                var newTile = TileFactory.CreateTile(newId, newSheet, null, (int)row, (int)col, this);
                _tileEntities.Insert(i, newTile);
                _entities.Add(newTile);
                RefreshFilteredLists();
                return;
            }
        }
    }


    public Entity CreateEntity(bool refresh = true)
    {
        var entity = new Entity(_nextId++);
        _entities.Add(entity);
        if (refresh)
            RefreshFilteredLists(); // guard check?
        return entity;
    }

    public void DeleteEntity(Entity entity)
    {
        _entities.Remove(entity);
        RefreshFilteredLists();
    }

    public IEnumerable<Entity> GetEntities()
    {
        return _entities;
    }

    internal IEnumerable<Entity> EntitiesWithComponent<T>()
    {
        foreach (var entity in _entities)
        {
            if (entity.HasComponent<T>())
            {
                yield return entity;
            }
        }
    }

    internal IEnumerable<Entity> EntitiesWithComponents<T1, T2>()
    {
        foreach (var entity in _entities)
        {
            if (entity.HasComponent<T1>() && entity.HasComponent<T2>())
            {
                yield return entity;
            }
        }
    }

    public void LoadPlayer(Entity player)
    {
        if (!_entities.Contains(player))
        {
            _entities.Add(player);
            RefreshFilteredLists();
        }
    }
}