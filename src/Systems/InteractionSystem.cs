using System;
using System.Linq;

using static Constants.Items.Name;

public class InteractionSystem
{
    private EntityManager _entityManager;
    private AnimationSystem _animationSystem;
    private Camera2D _camera;
    private InventorySystem _inventorySystem;

    private FarmingSystem _farmingSystem;
    public FarmingSystem FarmingSystem => _farmingSystem;

    private PlacingSystem _placingSystem;
    public PlacingSystem PlacingSystem => _placingSystem;

    public InteractionSystem(EntityManager entityManager, AnimationSystem animationSystem, Camera2D camera, InventorySystem inventorySystem)
    {
        _entityManager = entityManager;
        _animationSystem = animationSystem;
        _camera = camera;
        _inventorySystem = inventorySystem;

        _farmingSystem = new FarmingSystem(_entityManager, _inventorySystem);
        _placingSystem = new PlacingSystem(_entityManager, this, _inventorySystem);
    }

    public void MiscControls(Entity player, InputState inputs)
    {
        // misc
        if (inputs.ToggleHitbox)
            GameInitializer.ShowHitbox = !GameInitializer.ShowHitbox;
        // if (inputs.Save)
        //     SaveManager.SaveData(player);
        if (inputs.Grow)
            _farmingSystem.GrowPlants();
        if (inputs.ToggleInventory)
            GameStateManager.SetState(GameState.Inventory);
    }

    public void HandleInteractions(Entity player, InputState inputs, bool facingRight, ref bool isAnimationLocked, int lastDir)
    {
        var inv = player.GetComponent<InventoryComponent>();
        var colIndex = inputs.Number ?? 0;
        inv.activeItemIndices = inputs.IsNumberChanging ? (colIndex, 0) : inv.activeItemIndices;

        var activeItemEntity = inv.InventoryItems[inv.activeItemIndices.Item1][inv.activeItemIndices.Item2];



        if (activeItemEntity != null)
        {
            var activeItemConfig = inv.InventoryItems[inv.activeItemIndices.Item1][inv.activeItemIndices.Item2].GetComponent<ItemComponent>().Config;
            if (inputs.Interact && activeItemConfig != null)
            {
                if (_farmingSystem.HarvestCrop(this, player.GetComponent<InventoryComponent>()))
                    return;

                if (activeItemConfig.Type == ItemType.Plantable)
                {
                    _farmingSystem.PlantCrop(activeItemEntity, this);
                    return;
                }

                var aniVars = _animationSystem.GetAniInitVars(lastDir);
                switch (activeItemConfig.Name)
                {
                    case Pickaxe:
                        _animationSystem.SetAnimation(player, Constants.Animations.Pickaxe, aniVars.aniDirIndex, aniVars.mirrored);
                        isAnimationLocked = true;
                        break;
                    case WateringCan:
                        if (!isAnimationLocked)
                        {
                            var wateredTile = GetTile(InputSystem.GetMouseLocation());
                            if (wateredTile == null) { return; }
                            var wateredTileComp = wateredTile.GetComponent<TileComponent>();

                            // TODO: check if tile is within range?

                            // check if tile is waterable
                            if (wateredTileComp.Type == Constants.Tile.PathsSheetName && Constants.Tile.DrySoilTiles.Contains(wateredTileComp.Id))
                            {
                                var comp = activeItemEntity.GetComponent<LimitedUsageComponent>();

                                if (new LimitedUsageSystem().CanUseItem(comp))
                                {
                                    isAnimationLocked = true;
                                    _animationSystem.SetAnimation(player, Constants.Animations.Watering, aniVars.aniDirIndex, aniVars.mirrored);
                                    _entityManager.ChangeTile(wateredTile, Constants.Tile.PathsSheetName, Constants.Tile.WaterSoilTransform[wateredTileComp.Id]);
                                    new LimitedUsageSystem().UseItem(comp);
                                }
                            }
                        }
                        break;
                    case Juicer:
                    case JamJar:
                    case PickleJar:
                    case Keg:
                        var itemName = activeItemEntity.GetComponent<ItemComponent>().Config.Name;
                        var config = Constants.Machines.NameToConfig[itemName];
                        Action<int, int> placingAction = (x, y) => MachineFactory.CreateMachine(config, _entityManager, x, y);
                        _placingSystem.PlaceObjectInRange(player, placingAction);
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
        foreach (var entity in _entityManager.TileEntities)
        {
            var position = entity.GetComponent<PositionComponent>();
            if ((int)(position.X / tileSize) == col && (int)(position.Y / tileSize) == row)
            {
                return entity;
            }
        }
        return null;
    }

}