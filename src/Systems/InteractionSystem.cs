using System;
using System.Linq;

public class InteractionSystem
{
    private EntityManager _entityManager;
    private AnimationSystem _animationSystem;
    private Camera2D _camera;
    private InventorySystem _inventorySystem;

    private PlantInteractions _plantInteractions;
    public PlantInteractions PlantInteractions => _plantInteractions;

    public InteractionSystem(EntityManager entityManager, AnimationSystem animationSystem, Camera2D camera, InventorySystem inventorySystem)
    {
        _entityManager = entityManager;
        _animationSystem = animationSystem;
        _camera = camera;
        _inventorySystem = inventorySystem;

        _plantInteractions = new PlantInteractions(_entityManager, _inventorySystem);
    }

    public void MiscControls(Entity player, InputState inputs)
    {
        // misc
        if (inputs.ToggleHitbox)
            GameInitializer.ShowHitbox = !GameInitializer.ShowHitbox;
        if (inputs.Save)
            SaveManager.SaveData(player);
        if (inputs.Grow)
            _plantInteractions.GrowPlants();
    }

    public void HandleInteractions(Entity player, InputState inputs, bool facingRight, ref bool isAnimationLocked, int lastDir)
    {
        var inv = player.GetComponent<InventoryComponent>();
        var colIndex = inputs.Number  ?? 0;
        inv.activeItemIndices = inputs.IsNumberChanging ? (colIndex, 0) : inv.activeItemIndices;
        


        // colIndex = colIndex < 10 ? colIndex : 9;
        // colIndex = colIndex > 0 ? colIndex : 0;
        // inv.activeItemIndices = inv.activeItemIndices < 9 ? inv.activeItemIndices : 8;


        var activeItemEntity = inv.InventoryItems[inv.activeItemIndices.Item1][inv.activeItemIndices.Item2];
        if (activeItemEntity != null)
        {
            var activeItem = inv.InventoryItems[inv.activeItemIndices.Item1][inv.activeItemIndices.Item2].GetComponent<ItemComponent>().config;
            if (inputs.Interact && activeItem != null)
            {

                if (_plantInteractions.HarvestCrop(this, player.GetComponent<InventoryComponent>()))
                    return;

                if (activeItem.Type == ItemType.Plantable)
                {
                    _plantInteractions.PlantCrop(activeItemEntity, this);
                    return;
                }

                var aniVars = _animationSystem.GetAniInitVars(lastDir);
                switch (activeItem.Name)
                {
                    case "Pickaxe":
                        _animationSystem.SetAnimation(player, Constants.Animations.Pickaxe, aniVars.aniDirIndex, aniVars.mirrored);
                        isAnimationLocked = true;
                        break;
                    case "WateringCan":
                        _animationSystem.SetAnimation(player, Constants.Animations.Watering, aniVars.aniDirIndex, aniVars.mirrored);
                        if (!isAnimationLocked)
                        {
                            var wateredTile = GetTile(InputSystem.GetMouseLocation());
                            if (wateredTile == null) { return; }
                            var wateredTileComp = wateredTile.GetComponent<TileComponent>();

                            // TODO: check if tile is within range?

                            // check if tile is waterable
                            if (wateredTileComp.Type == Constants.Tile.PathsSheetName && Constants.Tile.DrySoilTiles.Contains(wateredTileComp.Id))
                            {
                                _entityManager.ChangeTile(wateredTile, Constants.Tile.PathsSheetName, Constants.Tile.WaterSoilTransform[wateredTileComp.Id]);
                            }
                        }
                        isAnimationLocked = true;
                        break;
                }
            }
        }
    }

    public Entity GetTile((int x, int y) mouse)
    {
        float worldX = mouse.x + _camera.Position.X;
        float worldY = mouse.y + _camera.Position.Y;
        int tileSize = (int)(Constants.DefaultTileSize * Constants.ScaleFactor);
        int col = (int)(worldX / tileSize);
        int row = (int)(worldY / tileSize);
        // foreach (var entity in _entityManager.EntitiesWithComponent<TileComponent>())
        foreach (var entity in _entityManager.TileEntities)
        {
            var position = entity.GetComponent<PositionComponent>();
            if ((int)(position.X / tileSize) == col && (int)(position.Y / tileSize) == row)
            {
                // Console.WriteLine(position.X / tileSize + " " + position.Y / tileSize);
                return entity;
            }
        }
        return null;
    }

}