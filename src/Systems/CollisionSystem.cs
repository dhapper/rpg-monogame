using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class CollisionSystem
{
    private List<Entity> _entities;
    private Entity _entity;

    public CollisionSystem(Entity entity, List<Entity> entities)
    {
        _entities = entities;
        _entity = entity;
    }

    public bool CheckEntityCollision(Rectangle futureHitbox)
    {
        foreach (var entity in _entities)
        {
            if (entity.Equals(_entity)) { continue; }

            if (entity.HasComponent<CollisionComponent>() && entity.GetComponent<CollisionComponent>().isSolid)
                if (futureHitbox.Intersects(entity.GetComponent<CollisionComponent>().Hitbox))
                {
                    return true;
                }
        }
        return false;
    }

    public void Move(float xSpeed, float ySpeed)
    {
        var position = _entity.GetComponent<PositionComponent>();
        float newX = position.X + xSpeed;
        float newY = position.Y + ySpeed;

        var collision = _entity.GetComponent<CollisionComponent>();

        Rectangle newHitboxX = new Rectangle(
            (int)(newX + collision.offsetX * Constants.ScaleFactor),
            (int)(position.Y + collision.offsetY * Constants.ScaleFactor),
            (int)(collision.hitboxWidth * Constants.ScaleFactor),
            (int)(collision.hitboxHeight * Constants.ScaleFactor));

        Rectangle newHitboxY = new Rectangle(
            (int)(position.X + collision.offsetX * Constants.ScaleFactor),
            (int)(newY + collision.offsetY * Constants.ScaleFactor),
            (int)(collision.hitboxWidth * Constants.ScaleFactor),
            (int)(collision.hitboxHeight * Constants.ScaleFactor));


        if (!CheckEntityCollision(newHitboxX))
            position.X = newX;

        if (!CheckEntityCollision(newHitboxY))
            position.Y = newY;
        
        collision.UpdateHitbox(position);
        _entity.AddComponent(position);
    }
}