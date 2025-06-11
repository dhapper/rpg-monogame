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

    public IReadOnlyList<Entity> TileEntities => _tileEntities.AsReadOnly();
    public IReadOnlyList<Entity> SpriteEntities => _spriteEntities.AsReadOnly();
    public IReadOnlyList<Entity> DroppedOverworldItems => _dropperdOverworldItems.AsReadOnly();
    public IReadOnlyList<Entity> CropEntities => _cropEntities.AsReadOnly();


    public void RefreshFilteredLists()
    {
        _tileEntities.Clear();
        _spriteEntities.Clear();
        _dropperdOverworldItems.Clear();
        _cropEntities.Clear();

        foreach (var entity in _entities)
        {
            if (entity.HasComponent<TileComponent>())
                _tileEntities.Add(entity);
            if (entity.HasComponent<PositionComponent>() && entity.HasComponent<SpriteComponent>() && entity.HasComponent<CharacterComponent>())
                _spriteEntities.Add(entity);
            if (entity.HasComponent<ItemComponent>() && entity.GetComponent<ItemComponent>().config.IsInOverworld)
                _dropperdOverworldItems.Add(entity);
            if (entity.HasComponent<CropComponent>())
                _cropEntities.Add(entity);

        }
    }

    public Entity CreateEntity()
    {
        var entity = new Entity(_nextId++);
        _entities.Add(entity);
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
}