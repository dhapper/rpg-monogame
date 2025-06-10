using System;
using Microsoft.Xna.Framework;

public class PlayerController
{
    private Entity _player;
    private InputSystem _inputSystem;
    private CollisionSystem _collisionSystem;
    private MovementSystem _movementSystem;
    private AnimationSystem _animationSystem;
    private MapSystem _mapSystem;
    private bool[] dir = [false, false, false, false];
    private bool facingRight = true;
    private Camera2D _camera;
    private EntityManager _entityManager;
    private InventorySystem _inventorySystem;
    private InteractionSystem _interactionSystem;
    private AssetStore _assets;

    private bool isAnimationLocked = false;

    public PlayerController(Entity player, InputSystem inputSystem, AnimationSystem animationSystem, MapSystem mapSystem, Camera2D camera, EntityManager entityManager, InventorySystem inventorySystem, InteractionSystem interactionSystem, AssetStore assets)
    {
        _player = player;
        _inputSystem = inputSystem;
        // _collisionSystem = new CollisionSystem(_player, _entities);
        _movementSystem = new MovementSystem();
        _animationSystem = animationSystem;
        _mapSystem = mapSystem;
        _camera = camera;
        _entityManager = entityManager;
        _collisionSystem = new CollisionSystem(_player, _entityManager);
        _inventorySystem = inventorySystem;
        _interactionSystem = interactionSystem;
        _assets = assets;
    }

    public void Update()
    {
        ResetDirVars();
        var movement = _player.GetComponent<MovementComponent>();
        movement.IsMoving = false;
        var inputs = _inputSystem.GetInputState();

        _inventorySystem.PickUp(_player);

        UpdateInputActions(inputs);
        UpdateAnimation(inputs, movement);
        UpdateCamera();
    }

    public void GetInteraction()
    {
        // if(Inventory.getCurrentItem == 'wateringCan')
    }

    public void PlantCrop()
    {
        var tile = _interactionSystem.GetTile(_inputSystem.GetMouseLocation(), _camera);
        if (tile == null) { return; }
        if (!tile.HasComponent<TileComponent>()) { return; }

        var tileComp = tile.GetComponent<TileComponent>();
        if (tileComp.Type == Constants.Tile.PathsSheetName && (tileComp.Id == 41 || tileComp.Id == 49))
        {
            var tilePosComp = tile.GetComponent<PositionComponent>();
            var tilePos = (tilePosComp.X, tilePosComp.Y);

            // check if already planted
            foreach (var entity in _entityManager.CropEntities)
            {
                if (entity.GetComponent<CropComponent>().config.TilePosition == tilePos)
                    return;
            }

            CropFactory.CreateCrop(Constants.Crops.Pumpkin, _entityManager, _assets, tilePos);
            _entityManager.RefreshFilteredLists();
        }

    }

    public void GrowPlants()
    {
        foreach (var entity in _entityManager.CropEntities)
        {
            var config = entity.GetComponent<CropComponent>().config;
            Console.WriteLine(config.CurrentStage+" "+config.Stages);
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

    public void UpdateInputActions(InputState inputs)
    {
        // movement
        if (inputs.MoveUp)
            InitMovement(Constants.Directions.Up);
        if (inputs.MoveDown)
            InitMovement(Constants.Directions.Down);
        if (inputs.MoveLeft)
            InitMovement(Constants.Directions.Left);
        if (inputs.MoveRight)
            InitMovement(Constants.Directions.Right);

        // misc
        if (inputs.ToggleHitbox)
            GameInitializer.ShowHitbox = !GameInitializer.ShowHitbox;
        if (inputs.Save)
            SaveManager.SaveData(_player);
        if (inputs.Grow)
            GrowPlants();

        var inv = _player.GetComponent<InventoryComponent>();
        // if (inv.InventoryItems[inputs.Number - 1 ?? 0][0] != null)
        // {
        // inv.activeItem = inputs.IsNumberChanging ? inv.InventoryItems[inputs.Number - 1 ?? 0][0] : inv.activeItem;
        // inv.activeItemIndices = (inputs.Number - 1 ?? 0, 0);
        inv.activeItemIndices = inputs.IsNumberChanging ? (inputs.Number - 1 ?? 0, 0) : inv.activeItemIndices;
        // }

        var activeItemEntity = inv.InventoryItems[inv.activeItemIndices.Item1][inv.activeItemIndices.Item2];
        if (activeItemEntity != null)
        {
            var activeItem = inv.InventoryItems[inv.activeItemIndices.Item1][inv.activeItemIndices.Item2].GetComponent<ItemComponent>().config;
            if (inputs.Interact && activeItem != null)
            {
                switch (activeItem.Name)
                {
                    case "WateringCan":
                        _animationSystem.SetAnimation(_player, facingRight ? Constants.Player.Animations.WateringCanRight : Constants.Player.Animations.WateringCanLeft);
                        isAnimationLocked = true;
                        break;
                    case "Pickaxe":
                        _animationSystem.SetAnimation(_player, facingRight ? Constants.Player.Animations.PickAxeRight : Constants.Player.Animations.PickAxeLeft);
                        isAnimationLocked = true;
                        break;
                    case "Seeds":
                        PlantCrop();
                        break;
                }
            }
        }
    }

    public void UpdateAnimation(InputState inputs, MovementComponent movement)
    {
        if (_player.GetComponent<AnimationComponent>().EndOfOneAnimationCycle)
            isAnimationLocked = false;

        if (isAnimationLocked)
            return;

        Vector2 speedVector = _movementSystem.CalculateSpeed(movement.Speed, dir);
        _collisionSystem.Move(speedVector.X, speedVector.Y, _camera.WorldWidthInPixels, _camera.WorldHeightInPixels);

        if (inputs.MoveLeft)
        {
            _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkLeft);
            facingRight = false;
        }
        if (inputs.MoveRight)
        {
            _animationSystem.SetAnimation(_player, Constants.Player.Animations.WalkRight);
            facingRight = true;
        }

        if (inputs.MoveUp || inputs.MoveDown)
            _animationSystem.SetAnimation(_player, facingRight ? Constants.Player.Animations.WalkRight : Constants.Player.Animations.WalkLeft);

        if (!movement.IsMoving || (speedVector.X == 0 && speedVector.Y == 0))
            _animationSystem.SetAnimation(_player, facingRight ? Constants.Player.Animations.IdleRight : Constants.Player.Animations.IdleLeft);
    }

    public void InitMovement(int direction)
    {
        var movement = _player.GetComponent<MovementComponent>();
        movement.LastDir = direction;
        movement.IsMoving = true;
        dir[direction] = true;
    }

    public void UpdateCamera()
    {
        var pos = _player.GetComponent<PositionComponent>();
        int cameraX = (int)(pos.X + Constants.ScaleFactor * (Constants.Player.XOffset + Constants.Player.HitboxWidth / 2));
        int cameraY = (int)(pos.Y + Constants.ScaleFactor * (Constants.Player.YOffset + Constants.Player.HitboxHeight / 2));
        _camera.Follow(new Vector2(cameraX, cameraY));
    }

    private void ResetDirVars()
    {
        dir[Constants.Directions.Up] = false;
        dir[Constants.Directions.Down] = false;
        dir[Constants.Directions.Left] = false;
        dir[Constants.Directions.Right] = false;
    }

}