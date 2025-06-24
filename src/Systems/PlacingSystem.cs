using System;
using System.Linq;
using Microsoft.Xna.Framework;

public class PlacingSystem
{
    private EntityManager _entityManager;
    private InteractionSystem _interactionSystem;
    private InventorySystem _inventorySystem;
    private Rectangle range;

    public Entity DefaultPlacingtTile;

    public PlacingSystem(EntityManager entityManager, InteractionSystem interactionSystem, InventorySystem inventorySystem)
    {
        _interactionSystem = interactionSystem;
        _entityManager = entityManager;
        _inventorySystem = inventorySystem;
    }

    public void Update(Entity player)
    {
        var lastDir = player.GetComponent<MovementComponent>().LastDir;
        DefaultPlacingtTile = GetDefaultInteractionTile(player, lastDir);
    }

    public Entity GetDefaultInteractionTile(Entity player, int lastDir)
    {
        var posComp = player.GetComponent<PositionComponent>();
        int col = posComp.Col;
        int row = posComp.Row;
        switch (lastDir)
        {
            case Constants.Directions.Up:
                row--;
                if (posComp.OverStepUp) row--;
                break;
            case Constants.Directions.Down:
                row++;
                if (posComp.OverStepDown) row++;
                break;
            case Constants.Directions.Left:
                col--;
                if (posComp.OverStepLeft) col--;
                break;
            case Constants.Directions.Right:
                col++;
                if (posComp.OverStepRight) col++;
                break;
        }

        foreach (var tile in _entityManager.TileEntities)
        {
            // var tilePos = tile.GetComponent<PositionComponent>();
            var tileComp = tile.GetComponent<TileComponent>();
            if (tileComp.Col == col && tileComp.Row == row)
            {
                // Console.WriteLine($"Default tile position: ({col},{row})");
                return tile;
            }
        }

        return null;
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

    // checks to see if change is required from mouse location to default tile
    public bool VerificationChecks1(Entity tile, Entity player)
    {
        if (tile == null) { return false; }

        var collisionComp = tile.GetComponent<CollisionComponent>();
        var hitbox = player.GetComponent<CollisionComponent>().Hitbox;

        if (!tile.HasComponent<TileComponent>()) { return false; }
        if (!range.Intersects(collisionComp.Hitbox)) { return false; }
        if (hitbox.Intersects(collisionComp.Hitbox)) { return false; }
        return true;
    }

    // checks to see if tile is placeable/not occupied
    public bool VerificationChecks2(Entity tile)
    {
        var tileComp = tile.GetComponent<TileComponent>();
        if (Constants.Tile.SolidTilesets.Contains(tileComp.Type)) { return false; }
        foreach (var entity in _entityManager.PlacedEntities)
        {
            var posComp = entity.GetComponent<PositionComponent>();
            if (posComp.Col == tileComp.Col && posComp.Row == tileComp.Row) { return false; }
        }
        // Console.WriteLine($"passed v2: ({tileComp.Col},{tileComp.Row})");
        return true;
    }

    public void PlaceObject(Entity player, Action<int, int> action)
    {
        var tile = _interactionSystem.GetTile(InputSystem.GetMouseLocation());

        if (!VerificationChecks1(tile, player))
        {
            tile = DefaultPlacingtTile;
            if (!VerificationChecks1(tile, player)) { return; }
        }

        if (!VerificationChecks2(tile)) { return; }

        var tileComp = tile.GetComponent<TileComponent>();

        action(tileComp.Col, tileComp.Row);
        _entityManager.RefreshFilteredLists();
        Console.WriteLine($"Placing obj at: ({tileComp.Col},{tileComp.Row})");

        var activeItemIndices = _inventorySystem.Inventory.activeItemIndices;
        _inventorySystem.RemoveOneFromSlot(activeItemIndices.Item1, activeItemIndices.Item2);
        // _entityManager.DeleteEntity(_inventorySystem.Inventory.InventoryItems[activeItemIndices.Item1][activeItemIndices.Item2]);
        // _inventorySystem.Inventory.InventoryItems[activeItemIndices.Item1][activeItemIndices.Item2] = null;
    }
}