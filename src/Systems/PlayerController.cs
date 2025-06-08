using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class PlayerController
{
    private Entity _player;
    private InputSystem _inputSystem;
    private CollisionSystem _collisionSystem;
    private MovementSystem _movementSystem;
    private AnimationSystem _animationSystem;
    private List<Entity> _entities;
    private MapSystem _mapSystem;
    private bool[] dir = [false, false, false, false];
    private bool facingRight = true;
    private Camera2D _camera;

    private bool isAnimationLocked = false;

    public PlayerController(Entity player, InputSystem inputSystem, AnimationSystem animationSystem, List<Entity> entities, MapSystem mapSystem, Camera2D camera)
    {
        _player = player;
        _inputSystem = inputSystem;
        _entities = entities;
        _collisionSystem = new CollisionSystem(_player, _entities);
        _movementSystem = new MovementSystem();
        _animationSystem = animationSystem;
        _mapSystem = mapSystem;
        _camera = camera;
    }

    public void Update()
    {
        ResetDirVars();
        var movement = _player.GetComponent<MovementComponent>();
        movement.IsMoving = false;
        var inputs = _inputSystem.GetInputState();

        UpdateInputActions(inputs);
        UpdateAnimation(inputs, movement);
        UpdateCamera();
    }

    public void GetInteraction()
    {
        // if(Inventory.getCurrentItem == 'wateringCan')
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

        var inv = _player.GetComponent<InventoryComponent>();
        inv.activeItem = inputs.IsNumberChanging ? inv.HotbarItems[inputs.Number - 1 ?? 0] : inv.activeItem;
        if (inputs.Interact)
        {
            switch (inv.activeItem.Name)
            {
                case "WateringCan":
                    _animationSystem.SetAnimation(_player, facingRight ? Constants.Player.Animations.WateringCanRight : Constants.Player.Animations.WateringCanLeft);
                    isAnimationLocked = true;
                    break;
                case "Pickaxe":
                    _animationSystem.SetAnimation(_player, facingRight ? Constants.Player.Animations.PickAxeRight : Constants.Player.Animations.PickAxeLeft);
                    isAnimationLocked = true;
                    break;
            }

            // _animationSystem.SetAnimation(_player, facingRight ? Constants.Player.Animations.WateringCanRight : Constants.Player.Animations.WateringCanLeft);
            // isAnimationLocked = true;

            // var (x, y) = _inputSystem.GetMouseLocation();
            // Entity tile = _mapSystem.GetTile(x, y);
            // if (tile != null && tile.HasComponent<InteractionComponent>())
            //     tile.GetComponent<InteractionComponent>().InteractAction(tile);
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