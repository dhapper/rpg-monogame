using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

public class CollisionSystem
{
    private List<Entity> _entities; // factor out?
    private Entity _entity;
    private EntityManager _entityManager;

    public CollisionSystem(Entity entity, EntityManager entityManager, List<Entity> entities)
    {
        _entities = entities;
        _entity = entity;
        _entityManager = entityManager;
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

    public void PickUp()
    {
        var hitbox = _entity.GetComponent<CollisionComponent>().Hitbox;
        // can be opitimized;
        var allEntities = _entityManager.GetEntities();
        var overworldItems = allEntities.Where(e => e.HasComponent<ItemComponent>() && e.GetComponent<ItemComponent>().config.IsInOverworld).ToList();

        foreach (var item in overworldItems)
        {
            var inCollectionBox = item.GetComponent<CollisionComponent>().Hitbox.Intersects(hitbox);
            if (inCollectionBox)
            {
                // Console.WriteLine(item.GetComponent<ItemComponent>().config.Name);
                //maybe remove this code from here
                for (int i = 0; i < Constants.UI.Inventory.Rows; i++)
                {
                    for (int j = 0; j < Constants.UI.Inventory.Cols; j++)
                    {
                        var inv = _entity.GetComponent<InventoryComponent>();
                        if (inv.InventoryItems[j][i] == null)
                        {
                            item.GetComponent<ItemComponent>().config.IsInOverworld = false;
                            inv.InventoryItems[j][i] = item;
                            return;
                        }
                    }
                }
            }
        }
    }

    public void Move(float xSpeed, float ySpeed, int worldWidth, int worldHeight)
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

        int hitboxWidth = _entity.GetComponent<CollisionComponent>().Hitbox.Width;
        int hitboxHeight = _entity.GetComponent<CollisionComponent>().Hitbox.Height;

        if (!(newHitboxX.X > 0 && newHitboxX.X < worldWidth - hitboxWidth - Constants.ScaleFactor))
            newX = position.X;

        if (!(newHitboxY.Y > 0 && newHitboxY.Y < worldHeight - hitboxHeight - Constants.ScaleFactor))
            newY = position.Y;

        if (!CheckEntityCollision(newHitboxX))
            position.X = newX;

        if (!CheckEntityCollision(newHitboxY))
            position.Y = newY;

        collision.UpdateHitbox(position);
        _entity.AddComponent(position);
    }
}