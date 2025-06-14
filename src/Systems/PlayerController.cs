using System;
using Microsoft.Xna.Framework;

public class PlayerController
{
    private Entity _player;
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
    // private PlantInteractions _plantInteractions;

    private bool isAnimationLocked = false;

    public PlayerController(Entity player, AnimationSystem animationSystem, MapSystem mapSystem, Camera2D camera, EntityManager entityManager, InventorySystem inventorySystem, InteractionSystem interactionSystem)
    {
        _player = player;
        // _collisionSystem = new CollisionSystem(_player, _entities);
        _movementSystem = new MovementSystem();
        _animationSystem = animationSystem;
        _mapSystem = mapSystem;
        _camera = camera;
        _entityManager = entityManager;
        _collisionSystem = new CollisionSystem(_player, _entityManager);
        _interactionSystem = interactionSystem;
        // _plantInteractions = new PlantInteractions(_entityManager);

        _inventorySystem = inventorySystem;
    }

    public void Update()
    {
        ResetDirVars();
        var movement = _player.GetComponent<MovementComponent>();
        movement.IsMoving = false;
        var inputs = InputSystem.GetInputState();

        _inventorySystem.PickUp(_player);

        UpdateInputActions(inputs);
        UpdateAnimation(inputs, movement);
        UpdateCamera();
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

        _interactionSystem.MiscControls(_player, inputs);

        var movement = _player.GetComponent<MovementComponent>();
        _interactionSystem.HandleInteractions(_player, inputs, facingRight, ref isAnimationLocked, movement.LastDir);

        // _interactionSystem.PlantInteractions.HarvestCrop(_interactionSystem, _inputSystem, _camera, _assets, _player.GetComponent<InventoryComponent>());
    }

    public void UpdateAnimation(InputState inputs, MovementComponent movement)
    {
        if (_player.GetComponent<AnimationComponent>().EndOfOneAnimationCycle)
            isAnimationLocked = false;

        if (isAnimationLocked)
            return;

        Vector2 speedVector = _movementSystem.CalculateSpeed(movement.Speed, dir);
        // Vector2 speedVector = _movementSystem.CalculateSpeed(2.66f, dir);
        _collisionSystem.Move(speedVector.X, speedVector.Y, _camera.WorldWidthInPixels, _camera.WorldHeightInPixels);

        if (inputs.MoveLeft)
        {
            _animationSystem.SetAnimation(_player, Constants.Animations.Walk, Constants.Animations.Sideways, true);
            facingRight = false;
        }
        if (inputs.MoveRight)
        {
            _animationSystem.SetAnimation(_player, Constants.Animations.Walk, Constants.Animations.Sideways);
            facingRight = true;
        }

        if (!inputs.MoveLeft && !inputs.MoveRight)
        {
            if (inputs.MoveUp)
                _animationSystem.SetAnimation(_player, Constants.Animations.Walk, Constants.Animations.Up);

            if (inputs.MoveDown)
                _animationSystem.SetAnimation(_player, Constants.Animations.Walk, Constants.Animations.Down);
        }


        // idle check
        if (!movement.IsMoving || (speedVector.X == 0 && speedVector.Y == 0))
        {
            switch (movement.LastDir)
            {
                case Constants.Directions.Left:
                    _animationSystem.SetAnimation(_player, Constants.Animations.Idle, Constants.Animations.Sideways, true);
                    break;
                case Constants.Directions.Right:
                    _animationSystem.SetAnimation(_player, Constants.Animations.Idle, Constants.Animations.Sideways);
                    break;
                case Constants.Directions.Up:
                    _animationSystem.SetAnimation(_player, Constants.Animations.Idle, Constants.Animations.Up);
                    break;
                case Constants.Directions.Down:
                    _animationSystem.SetAnimation(_player, Constants.Animations.Idle, Constants.Animations.Down);
                    break;
            }
        }
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