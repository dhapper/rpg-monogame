using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static Constants;

public class RenderSystem
{
    private SpriteBatch _spriteBatch;
    private EntityManager _entityManager;
    private AssetStore _assetStore;
    private Camera2D _camera;
    private GraphicsDevice _graphicsDevice;
    private InventoryUI _inventoryUI;
    private GameStateManager _gameStateManager;
    private InputSystem _inputSystem;

    private UIRenderSystem _uiRenderSystem;

    private Rectangle _cameraView;

    public RenderSystem(SpriteBatch spriteBatch, EntityManager entityManager, AssetStore assetStore, Camera2D camera, GraphicsDevice graphicsDevice, InventoryUI inventoryUI, GameStateManager gameStateManager, InputSystem inputSystem)
    {
        _spriteBatch = spriteBatch;
        _entityManager = entityManager;
        _assetStore = assetStore;
        _camera = camera;
        _graphicsDevice = graphicsDevice;
        _inventoryUI = inventoryUI;
        _gameStateManager = gameStateManager;
        _inputSystem = inputSystem;

        _uiRenderSystem = new UIRenderSystem(_spriteBatch, _assetStore, _entityManager, _inputSystem, _camera, _gameStateManager, _inventoryUI);
    }

    public void Draw()
    {
        _cameraView = _camera.GetViewRectangle();

        _spriteBatch.Begin(transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);  // new spritebatch for screen relative positioning

        var sortedSprites = _entityManager.SpriteEntities
            .OrderBy(e =>
            {
                var position = e.GetComponent<PositionComponent>();
                var sprite = e.GetComponent<SpriteComponent>();
                int sourceHeight = sprite.SourceRectangle?.Height ?? 0;
                return position.Y + sourceHeight;
            }).ToList();

        // Draw tiles
        foreach (var entity in _entityManager.TileEntities)
        {
            if (entity.GetComponent<TileComponent>().Background != default)
                DrawTile(entity, true);

            DrawTile(entity);

            if (GameInitializer.ShowHitbox && entity.HasComponent<CollisionComponent>())
                DrawHitbox(_spriteBatch, entity.GetComponent<CollisionComponent>().Hitbox);
        }

        // Draw entities
        foreach (var entity in sortedSprites)
        {
            DrawEntity(entity);

            if (GameInitializer.ShowHitbox && entity.HasComponent<CollisionComponent>())
                DrawHitbox(_spriteBatch, entity.GetComponent<CollisionComponent>().Hitbox);
        }

        // Draw dropped overworld items
        foreach (var entity in _entityManager.DroppedOverworldItems)
        {
            DrawOverworldItem(entity);

            if (GameInitializer.ShowHitbox && entity.HasComponent<CollisionComponent>())
                DrawHitbox(_spriteBatch, entity.GetComponent<CollisionComponent>().Hitbox);
        }

        _spriteBatch.End();

        _uiRenderSystem.Draw();
    }

    public void DrawOverworldItem(Entity entity)
    {
        if (!RenderHelper.IsEntityInCameraView(entity, _cameraView)) { return; }

        var position = entity.GetComponent<PositionComponent>();
        var sprite = entity.GetComponent<SpriteComponent>();

        _spriteBatch.Draw(
            sprite.Texture,
            new Vector2(position.X, position.Y),
            sprite.SourceRectangle,
            sprite.Color,
            0f,
            Vector2.Zero,
            // ScaleFactor,
            DroppedItemScaleFactor,
            SpriteEffects.None,
            0f);
    }

    public void DrawTile(Entity entity, bool hasBackground = false)
    {
        if (!RenderHelper.IsEntityInCameraView(entity, _cameraView)) { return; }

        var position = entity.GetComponent<PositionComponent>();
        var sprite = entity.GetComponent<SpriteComponent>();

        _spriteBatch.Draw(
            hasBackground ? _assetStore.BackgroundTiles : sprite.Texture,
            new Vector2(position.X, position.Y),
            hasBackground ? entity.GetComponent<TileComponent>().Background : sprite.SourceRectangle,
            sprite.Color,
            0f,
            Vector2.Zero,
            ScaleFactor,
            SpriteEffects.None,
            0f);
    }

    public void DrawEntity(Entity entity)
    {
        if (!RenderHelper.IsEntityInCameraView(entity, _cameraView)) { return; }

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
            ScaleFactor,
            effects,
            0f);
    }

    public void DrawHitbox(SpriteBatch spriteBatch, Rectangle hitbox)
    {
        if (!RenderHelper.IsRectangleInCameraView(hitbox, _cameraView)) { return; }

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