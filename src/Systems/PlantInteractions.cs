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

    public void HarvestCrop(InteractionSystem _interactionSystem, InputSystem _inputSystem, Camera2D _camera, AssetStore _assets, InventoryComponent inventory)
    {
        var tile = _interactionSystem.GetTile(_inputSystem.GetMouseLocation(), _camera);
        if (tile == null) { return; }
        if (!tile.HasComponent<TileComponent>()) { return; }
        var tileComp = tile.GetComponent<TileComponent>();
        if (!(tileComp.Type == Constants.Tile.PathsSheetName && Constants.Tile.PlantableTiles.Contains(tileComp.Id))) { return; }

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
                    Console.WriteLine(cropName);
                    if (Constants.SeedCropMapping.PlantedCropNameToCrop.TryGetValue(cropName, out var itemConfig))
                    {
                        Console.WriteLine("in");
                        inventory.InventoryItems[slot.Value.j][slot.Value.i] = ItemFactory.CreateItem(itemConfig, _entityManager, _assets);
                        _entityManager.RefreshFilteredLists();
                    }
                    return;
                }
                else
                {
                    return;
                }
            }
        }
    }

    public void PlantCrop(Entity seed, InteractionSystem _interactionSystem, InputSystem _inputSystem, Camera2D _camera, AssetStore _assets)
    {
        var tile = _interactionSystem.GetTile(_inputSystem.GetMouseLocation(), _camera);
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
            CropFactory.CreateCrop(cropConfig, _entityManager, _assets, tilePos);
            _entityManager.RefreshFilteredLists();
        }

    }

    public void GrowPlants()
    {
        foreach (var entity in _entityManager.CropEntities)
        {
            var config = entity.GetComponent<CropComponent>().config;
            if (config.CurrentStage < config.Stages)
            {
                var spriteComp = entity.GetComponent<SpriteComponent>();
                Rectangle rect = spriteComp.SourceRectangle;
                rect.X += Constants.Crops.DefaultSpriteSize;
                spriteComp.SourceRectangle = rect;
                config.CurrentStage++;
            }
        }
    }
}