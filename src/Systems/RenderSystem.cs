using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class RenderSystem
{
    private SpriteBatch _spriteBatch;
    private EntityManager _entityManager;
    private AssetStore _assetStore;
    private Camera2D _camera;
    private GraphicsDevice _graphicsDevice;
    private InventoryUI _inventoryUI;
    private GameStateManager _gameStatemanager;

    public RenderSystem(SpriteBatch spriteBatch, EntityManager entityManager, AssetStore assetStore, Camera2D camera, GraphicsDevice graphicsDevice, InventoryUI inventoryUI, GameStateManager gameStateManager)
    {
        _spriteBatch = spriteBatch;
        _entityManager = entityManager;
        _assetStore = assetStore;
        _camera = camera;
        _graphicsDevice = graphicsDevice;
        _inventoryUI = inventoryUI;
        _gameStatemanager = gameStateManager;
    }

    public void Draw()
    {
        _spriteBatch.Begin(transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);

        var allEntities = _entityManager.GetEntities();
        var tileEntities = allEntities.Where(e => e.HasComponent<TileComponent>()).ToList();
        var spriteEntities = allEntities.Where(e => e.HasComponent<PositionComponent>() && e.HasComponent<SpriteComponent>() && !e.HasComponent<TileComponent>()).ToList();
        var sortedSprites = spriteEntities
            .OrderBy(e =>
            {
                var position = e.GetComponent<PositionComponent>();
                var sprite = e.GetComponent<SpriteComponent>();
                int sourceHeight = sprite.SourceRectangle?.Height ?? 0;
                return position.Y + sourceHeight;
            }).ToList();

        foreach (var entity in tileEntities)
        {
            if (entity.GetComponent<TileComponent>().Background != default)
                DrawTile(entity, true);

            DrawTile(entity);

            if (GameInitializer.ShowHitbox && entity.HasComponent<CollisionComponent>())
                DrawHitbox(_spriteBatch, entity.GetComponent<CollisionComponent>().Hitbox);
        }

        foreach (var entity in sortedSprites)
        {
            DrawEntity(entity);

            if (GameInitializer.ShowHitbox && entity.HasComponent<CollisionComponent>())
                DrawHitbox(_spriteBatch, entity.GetComponent<CollisionComponent>().Hitbox);
        }

        DrawHotbar();
        if (_gameStatemanager.CurrentGameState == GameState.Inventory)
            DrawInventory();

        _spriteBatch.End();
    }

    public void DrawInventory()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _spriteBatch.Draw(
                    _assetStore.UIsheet,
                    _inventoryUI.InventorySlotPositions[i][j],
                    new Rectangle(0, 0, _inventoryUI.DefaultSlotSize, _inventoryUI.DefaultSlotSize),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    Constants.ScaleFactor,
                    SpriteEffects.None,
                    0f);

                var inv = _entityManager.EntitiesWithComponent<InventoryComponent>().First().GetComponent<InventoryComponent>();
                if (inv.InventoryItems[i][j] != null)
                {
                    _spriteBatch.Draw(
                        _assetStore.IconSheet,
                        _inventoryUI.InventoryIconPositions[i][j],
                        inv.InventoryItems[i][j].SourceRectangle,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        Constants.ScaleFactor,
                        SpriteEffects.None,
                        0f);
                }
            }
        }
    }

    public void DrawHotbar()
    {

        for (int i = 0; i < 9; i++)
        {
            _spriteBatch.Draw(
                _assetStore.UIsheet,
                _inventoryUI.HotbarSlotPositions[i],
                new Rectangle(0, 0, _inventoryUI.DefaultSlotSize, _inventoryUI.DefaultSlotSize),
                Color.White,
                0f,
                Vector2.Zero,
                Constants.ScaleFactor,
                SpriteEffects.None,
                0f);
        }

        for (int i = 0; i < 9; i++)
        {
            var inv = _entityManager.EntitiesWithComponent<InventoryComponent>().First().GetComponent<InventoryComponent>();

            if (inv.HotbarItems[i] == inv.activeItem)
            {
                _spriteBatch.Draw(
                    _assetStore.UIsheet,
                    _inventoryUI.HotbarSlotPositions[i],
                    new Rectangle(32, 0, _inventoryUI.DefaultSlotSize, _inventoryUI.DefaultSlotSize),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    Constants.ScaleFactor,
                    SpriteEffects.None,
                    0f);
            }

            if (inv.HotbarItems[i] != null)
            {
                _spriteBatch.Draw(
                    _assetStore.IconSheet,
                    _inventoryUI.HotbarIconPositions[i],
                    inv.HotbarItems[i].SourceRectangle,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    Constants.ScaleFactor,
                    SpriteEffects.None,
                    0f);
            }
        }
    }

    public void DrawTile(Entity entity, bool hasBackground = false)
    {
        var position = entity.GetComponent<PositionComponent>();
        var sprite = entity.GetComponent<SpriteComponent>();

        _spriteBatch.Draw(
            hasBackground ? _assetStore.BackgroundTiles : sprite.Texture,
            new Vector2(position.X, position.Y),
            hasBackground ? entity.GetComponent<TileComponent>().Background : sprite.SourceRectangle,
            sprite.Color,
            0f,
            Vector2.Zero,
            Constants.ScaleFactor,
            SpriteEffects.None,
            0f);
    }

    public void DrawEntity(Entity entity)
    {
        var position = entity.GetComponent<PositionComponent>();
        var sprite = entity.GetComponent<SpriteComponent>();
        var animation = entity.GetComponent<AnimationComponent>();

        SpriteEffects effects = animation.IsMirrored ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        _spriteBatch.Draw(
            sprite.Texture,
            new Vector2((int)position.X, (int)position.Y),
            sprite.SourceRectangle,
            sprite.Color,
            0f,
            Vector2.Zero,
            Constants.ScaleFactor,
            effects,
            0f);
    }

    public void DrawHitbox(SpriteBatch spriteBatch, Rectangle hitbox)
    {
        Texture2D whitePixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        whitePixel.SetData(new[] { Color.White });
        Color color = Color.White;

        // Top, Bottom, Left, Right
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Top, hitbox.Width, 1), color);
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Bottom - 1, hitbox.Width, 1), color);
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Top, 1, hitbox.Height), color);
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Right - 1, hitbox.Top, 1, hitbox.Height), color);
    }

}