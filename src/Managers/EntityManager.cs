using System;
using System.Collections.Generic;

public class EntityManager
{
    private int _nextId = 0;
    private List<Entity> _entities = new();
    public List<Entity> Entities => _entities;

    public Entity CreateEntity()
    {
        var entity = new Entity(_nextId++);
        _entities.Add(entity);
        return entity;
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