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
            if (collisionComp != null && collisionComp.isSolid)
                if (futureHitbox.Intersects(collisionComp.Hitbox))
                    return true;
        }
        return false;
    }

    public void CheckZones(Rectangle futureHitbox)
    {
        foreach (var entity in _entityManager.Zones)
        {
            var zone = entity.GetComponent<ZoneComponent>();
            var collision = entity.GetComponent<CollisionComponent>();
            bool previouslyInZone = zone.InZone;

            if (futureHitbox.Intersects(collision.Hitbox))
            {
                if (!previouslyInZone)
                {
                    Console.WriteLine("sleepin");
                    zone.InZone = true;

                    // zone specific code invoked only on initial entry
                }
            }
            else
            {
                zone.InZone = false;
            }
        }
    }

    // public void PickUp()    // move to invsys
    // {
    //     var hitbox = _entity.GetComponent<CollisionComponent>().Hitbox;
    //     foreach (var item in _entityManager.DroppedOverworldItems)
    //     {
    //         var inCollectionBox = item.GetComponent<CollisionComponent>().Hitbox.Intersects(hitbox);
    //         if (inCollectionBox)
    //         {
    //             for (int i = 0; i < Constants.UI.Inventory.Rows; i++)
    //             {
    //                 for (int j = 0; j < Constants.UI.Inventory.Cols; j++)
    //                 {
    //                     var inv = _entity.GetComponent<InventoryComponent>();
    //                     if (inv.InventoryItems[j][i] == null)
    //                     {
    //                         item.GetComponent<ItemComponent>().config.IsInOverworld = false;
    //                         inv.InventoryItems[j][i] = item;
    //                         _entityManager.RefreshFilteredLists();
    //                         return;
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }

    public void Move(float xSpeed, float ySpeed, int worldWidth, int worldHeight)
    {
        var position = _entity.GetComponent<PositionComponent>();
        var collision = _entity.GetComponent<CollisionComponent>();
        float newX = position.X + xSpeed;
        float newY = position.Y + ySpeed;

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