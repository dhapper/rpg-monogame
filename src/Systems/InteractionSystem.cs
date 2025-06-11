using System;

public class InteractionSystem
{
    private EntityManager _entityManager;
    private InputSystem _inputSystem;
    private AssetStore _assets;
    private AnimationSystem _animationSystem;
    private Camera2D _camera;

    private PlantInteractions _plantInteractions;
    public PlantInteractions PlantInteractions => _plantInteractions;
    private InventorySystem _inventorySystem;

    public InteractionSystem(EntityManager entityManager, InputSystem inputSystem, AssetStore assets, AnimationSystem animationSystem, Camera2D camera, InventorySystem inventorySystem)
    {
        _entityManager = entityManager;
        _inputSystem = inputSystem;
        _assets = assets;
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

    public void HandleInteractions(Entity player, InputState inputs, bool facingRight, ref bool isAnimationLocked)
    {
        var inv = player.GetComponent<InventoryComponent>();
        inv.activeItemIndices = inputs.IsNumberChanging ? (inputs.Number - 1 ?? 0, 0) : inv.activeItemIndices;

        var activeItemEntity = inv.InventoryItems[inv.activeItemIndices.Item1][inv.activeItemIndices.Item2];
        if (activeItemEntity != null)
        {
            var activeItem = inv.InventoryItems[inv.activeItemIndices.Item1][inv.activeItemIndices.Item2].GetComponent<ItemComponent>().config;
            if (inputs.Interact && activeItem != null)
            {

                _plantInteractions.HarvestCrop(this, _inputSystem, _camera, _assets, player.GetComponent<InventoryComponent>());

                if (activeItem.Type == ItemType.Plantable)
                {
                    _plantInteractions.PlantCrop(activeItemEntity, this, _inputSystem, _camera, _assets);
                    return;
                }

                switch (activeItem.Name)
                {
                    case "WateringCan":
                        _animationSystem.SetAnimation(player, facingRight ? Constants.Player.Animations.WateringCanRight : Constants.Player.Animations.WateringCanLeft);
                        isAnimationLocked = true;
                        break;
                    case "Pickaxe":
                        _animationSystem.SetAnimation(player, facingRight ? Constants.Player.Animations.PickAxeRight : Constants.Player.Animations.PickAxeLeft);
                        isAnimationLocked = true;
                        break;
                }
            }
        }
    }

    public Entity GetTile((int x, int y) mouse, Camera2D camera)
    {
        float worldX = mouse.x + camera.Position.X;
        float worldY = mouse.y + camera.Position.Y;
        int tileSize = (int)(Constants.DefaultTileSize * Constants.ScaleFactor);
        int col = (int)(worldX / tileSize);
        int row = (int)(worldY / tileSize);
        foreach (var entity in _entityManager.EntitiesWithComponent<TileComponent>())
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