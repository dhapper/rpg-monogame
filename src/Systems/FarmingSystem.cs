using System;
using System.Linq;
using Microsoft.Xna.Framework;

public class FarmingSystem
{
    private EntityManager _entityManager;
    private InventorySystem _inventorySystem;

    public FarmingSystem(EntityManager entityManager, InventorySystem inventorySystem)
    {
        _entityManager = entityManager;
        _inventorySystem = inventorySystem;
    }

    public bool HarvestCrop(InteractionSystem _interactionSystem, InventoryComponent inventory)
    {
        var tile = _interactionSystem.GetTile(InputSystem.GetMouseLocation());
        if (tile == null) { return false; }
        if (!tile.HasComponent<TileComponent>()) { return false; }
        var tileComp = tile.GetComponent<TileComponent>();
        if (!(tileComp.Type == Constants.Tile.PathsSheetName && Constants.Tile.PlantableTiles.Contains(tileComp.Id))) { return false; }

        var tilePosComp = tile.GetComponent<PositionComponent>();
        // var tilePos = (tilePosComp.X, tilePosComp.Y);
        // var tilePos = (tileComp.Col, tileComp.Row);

        // check if there is fully grown planted crop
        foreach (var entity in _entityManager.CropEntities)
        {
            var plantedCropConfig = entity.GetComponent<CropComponent>().config;
            var plantedCropPos = entity.GetComponent<PositionComponent>();
            // if (plantedCropConfig.TilePosition == tilePos)
            if (plantedCropPos.Col == tileComp.Col && plantedCropPos.Row == tileComp.Row )
            {
                // check growth stage
                if (plantedCropConfig.CurrentStage >= plantedCropConfig.Stages)
                {
                    var cropName = plantedCropConfig.Name;
                    Console.WriteLine($"Harvested and deleted entity {entity.Id} at ({plantedCropPos.Col},{plantedCropPos.Row})");
                    _entityManager.DeleteEntity(entity);
                    // var slot = _inventorySystem.GetNextEmptySlot();
                    if (Constants.SeedCropMapping.PlantedCropNameToCrop.TryGetValue(cropName, out var itemConfig))
                    {

                        // if stackable
                        // check for empty unfilled stack

                        // check for empty slot

                        // if not placed
                        // drop into overworld

                        // _inventorySystem.PlaceInNextEmptySlot(ItemFactory.CreateItem(itemConfig, _entityManager));
                        var item = ItemFactory.CreateItem(itemConfig, _entityManager);
                        _inventorySystem.PlaceItemInInventory(item);

                        // inventory.InventoryItems[slot.Value.j][slot.Value.i] = ItemFactory.CreateItem(itemConfig, _entityManager);  //refactor out inventory
                        _entityManager.RefreshFilteredLists();
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    public void PlantCrop(Entity seed, InteractionSystem _interactionSystem)
    {
        var tile = _interactionSystem.GetTile(InputSystem.GetMouseLocation());
        if (tile == null) { return; }
        if (!tile.HasComponent<TileComponent>()) { return; }

        var tileComp = tile.GetComponent<TileComponent>();
        if (!(tileComp.Type == Constants.Tile.PathsSheetName && Constants.Tile.PlantableTiles.Contains(tileComp.Id))) { return; }
        var tilePosComp = tile.GetComponent<PositionComponent>();
        var tilePos = (tilePosComp.X, tilePosComp.Y);

        // check if already planted
        foreach (var entity in _entityManager.CropEntities)
        {
            if (entity.GetComponent<CropComponent>().config.TilePosition == tilePos)
                return;
        }

        var itemName = seed.GetComponent<ItemComponent>().Config.Name;
        if (Constants.SeedCropMapping.SeedNameToCrop.TryGetValue(itemName, out var cropConfig))
        {
            CropFactory.CreateCrop(cropConfig, tileComp.Row, tileComp.Col, _entityManager, tilePos);
            _entityManager.RefreshFilteredLists();

            _entityManager.DeleteEntity(seed);
            var activeItemIndices = _inventorySystem.Inventory.activeItemIndices;
            _inventorySystem.Inventory.InventoryItems[activeItemIndices.Item1][activeItemIndices.Item2] = null;
        }

    }

    public void GrowPlants()
    {
        foreach (var entity in _entityManager.CropEntities)
        {
            bool flag = false;
            var cropComp = entity.GetComponent<CropComponent>();
            foreach (var tile in _entityManager.TileEntities)
            {
                if (flag) { break; }
                var tileComp = tile.GetComponent<TileComponent>();
                var posComp = entity.GetComponent<PositionComponent>();
                if (tileComp.Col == posComp.Col && tileComp.Row == posComp.Row)
                {
                    if (tileComp.Type == Constants.Tile.PathsSheetName && Constants.Tile.WetSoilTiles.Contains(tileComp.Id))
                    {
                        // Console.WriteLine(tileComp.Type + " " + tileComp.Id + " " + cropComp.config.CurrentStage + " " + cropComp.config.Stages);
                        if (cropComp.config.CurrentStage < cropComp.config.Stages)
                        {
                            var spriteComp = entity.GetComponent<SpriteComponent>();
                            Rectangle rect = spriteComp.SourceRectangle;
                            rect.X += Constants.Crops.DefaultSpriteSize;
                            spriteComp.SourceRectangle = rect;
                            cropComp.config.CurrentStage++;
                            flag = true;
                        }
                    }
                }
            }

        }
    }
}