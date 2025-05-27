using System;
using System.Collections.Generic;

public class Entity
{
    public int Id { get; private set; } // Unique ID
    public Dictionary<Type, object> Components { get; } = new();

    public Entity(int id)
    {
        Id = id;
    }

    public void AddComponent<T>(T component)
    {
        Components[typeof(T)] = component;
    }

    public T GetComponent<T>()
    {
        return (T)Components[typeof(T)];
    }

    public bool HasComponent<T>()
    {
        return Components.ContainsKey(typeof(T));
    }
}