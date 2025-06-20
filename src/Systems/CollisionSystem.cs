using System;
using Microsoft.Xna.Framework;

public class CollisionSystem
{
    private Entity _entity;
    private EntityManager _entityManager;

    public CollisionSystem(Entity entity, EntityManager entityManager)
    {
        _entity = entity;
        _entityManager = entityManager;
    }

    public bool CheckEntityCollision(Rectangle futureHitbox)    // can refactor to use predefinmiedd list?
    {
        foreach (var entity in _entityManager.Entities)
        {
            if (entity.Equals(_entity)) { continue; }

            var collisionComp = entity.GetComponent<CollisionComponent>();
            if (collisionComp != null && collisionComp.IsSolid)
                if (futureHitbox.Intersects(collisionComp.Hitbox))
                    return true;
        }
        return false;
    }

    public void CheckZones(Rectangle futureHitbox)
    {
        // bool inZoneEndFlag = false;
        foreach (var entity in _entityManager.Zones)
        {
            var zone = entity.GetComponent<ZoneComponent>();
            var collision = entity.GetComponent<CollisionComponent>();
            bool previouslyInZone = zone.InZone;

            if (futureHitbox.Intersects(collision.Hitbox))
            {
                if (!previouslyInZone)
                {
                    zone.InZone = true;
                    zone.ZoneAction();
                    // inZoneEndFlag = true;
                    break;
                }
            }
            else
            {
                zone.InZone = false;
            }
        }


    }

    public void Move(float xSpeed, float ySpeed, int worldWidth, int worldHeight)
    {
        var position = _entity.GetComponent<PositionComponent>();
        var collision = _entity.GetComponent<CollisionComponent>();
        float newX = position.X + xSpeed;
        float newY = position.Y + ySpeed;

        Rectangle newHitboxX = new Rectangle(
            (int)(newX + collision.OffsetX),
            (int)(position.Y + collision.OffsetY),
            (int)collision.HitboxWidth,
            (int)collision.HitboxHeight);

        Rectangle newHitboxY = new Rectangle(
            (int)(position.X + collision.OffsetX),
            (int)(newY + collision.OffsetY),
            (int)collision.HitboxWidth,
            (int)collision.HitboxHeight);

        int hitboxWidth = collision.Hitbox.Width;
        int hitboxHeight = collision.Hitbox.Height;

        if (!(newHitboxX.X > 0 && newHitboxX.X < worldWidth - hitboxWidth - Constants.ScaleFactor))
            newX = position.X;

        if (!(newHitboxY.Y > 0 && newHitboxY.Y < worldHeight - hitboxHeight - Constants.ScaleFactor))
            newY = position.Y;

        if (!CheckEntityCollision(newHitboxX))
            position.X = newX;

        if (!CheckEntityCollision(newHitboxY))
            position.Y = newY;

        CheckZones(collision.Hitbox);

        collision.UpdateHitbox(position);
    }
}