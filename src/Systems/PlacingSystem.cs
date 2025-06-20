using System;
using System.Linq;
using Microsoft.Xna.Framework;

public class PlacingSystem
{
    private EntityManager _entityManager;
    private InteractionSystem _interactionSystem;
    private InventorySystem _inventorySystem;
    private Rectangle range;

    public PlacingSystem(EntityManager entityManager, InteractionSystem interactionSystem, InventorySystem inventorySystem)
    {
        _interactionSystem = interactionSystem;
        _entityManager = entityManager;
        _inventorySystem = inventorySystem;
    }

    public void UpdateRectangle(Entity player)
    {
        var posComp = player.GetComponent<PositionComponent>();
        var collisionComp = player.GetComponent<CollisionComponent>();
        var midX = posComp.X + collisionComp.OffsetX + collisionComp.HitboxWidth / 2;
        var midY = posComp.Y + collisionComp.OffsetY + collisionComp.HitboxHeight / 2;

        range = new Rectangle(
            (int)(midX - Constants.TileSize - collisionComp.HitboxWidth / 2),
            (int)(midY - Constants.TileSize - collisionComp.HitboxHeight / 2),
            (int)(2 * Constants.TileSize + collisionComp.HitboxWidth),
            (int)(2 * Constants.TileSize + collisionComp.HitboxHeight));
    }

    public void PlaceObjectInRange(Entity player, Action<int, int> action)
    {
        UpdateRectangle(player);
        PlaceObject(player, action);
    }

    public void PlaceObject(Entity player, Action<int, int> action)
    {
        // various checks to ensure target tile is valid
        var tile = _interactionSystem.GetTile(InputSystem.GetMouseLocation());
        var collisionComp = tile.GetComponent<CollisionComponent>();
        var tileComp = tile.GetComponent<TileComponent>();
        var hitbox = player.GetComponent<CollisionComponent>().Hitbox;
        if (tile == null) { return; }
        if (!tile.HasComponent<TileComponent>()) { return; }
        if (Constants.Tile.SolidTilesets.Contains(tileComp.Type)) { return; }
        if (!range.Intersects(collisionComp.Hitbox)) { return; }
        if (hitbox.Intersects(collisionComp.Hitbox)) { return; }
        foreach (var entity in _entityManager.PlacedEntities)
        {
            var posComp = entity.GetComponent<PositionComponent>();
            if (posComp.Col == tileComp.Col && posComp.Row == tileComp.Row) { return; }
        }

        // place object
        action(tileComp.Col, tileComp.Row);
        _entityManager.RefreshFilteredLists();

        // remove from inv
        // _inventorySystem

        var activeItemIndices = _inventorySystem.Inventory.activeItemIndices;
        _entityManager.DeleteEntity(_inventorySystem.Inventory.InventoryItems[activeItemIndices.Item1][activeItemIndices.Item2]);
        _inventorySystem.Inventory.InventoryItems[activeItemIndices.Item1][activeItemIndices.Item2] = null;
    }
}