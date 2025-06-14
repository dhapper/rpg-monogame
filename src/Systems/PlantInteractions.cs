using System;
using System.Linq;
using Microsoft.Xna.Framework;

public class PlantInteractions
{

    private EntityManager _entityManager;
    private InventorySystem _inventorySystem;

    public PlantInteractions(EntityManager entityManager, InventorySystem inventorySystem)
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
        var tilePos = (tilePosComp.X, tilePosComp.Y);

        // check if there is fully grown planted crop
        foreach (var entity in _entityManager.CropEntities)
        {
            var plantedCropConfig = entity.GetComponent<CropComponent>().config;
            if (plantedCropConfig.TilePosition == tilePos)
            {
                // check growth stage
                if (plantedCropConfig.CurrentStage >= plantedCropConfig.Stages)
                {
                    var cropName = plantedCropConfig.Name;
                    _entityManager.DeleteEntity(entity);
                    var slot = _inventorySystem.GetNextEmptySlot(inventory);
                    if (Constants.SeedCropMapping.PlantedCropNameToCrop.TryGetValue(cropName, out var itemConfig))
                    {
                        inventory.InventoryItems[slot.Value.j][slot.Value.i] = ItemFactory.CreateItem(itemConfig, _entityManager);
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

        var itemName = seed.GetComponent<ItemComponent>().config.Name;
        if (Constants.SeedCropMapping.SeedNameToCrop.TryGetValue(itemName, out var cropConfig))
        {
            CropFactory.CreateCrop(cropConfig, tileComp.Row, tileComp.Col, _entityManager, tilePos);
            _entityManager.RefreshFilteredLists();
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
                if (tileComp.Col == cropComp.Col && tileComp.Row == cropComp.Row)
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