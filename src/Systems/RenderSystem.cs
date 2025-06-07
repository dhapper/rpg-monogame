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

    public RenderSystem(SpriteBatch spriteBatch, EntityManager entityManager, AssetStore assetStore, Camera2D camera, GraphicsDevice graphicsDevice)
    {
        _spriteBatch = spriteBatch;
        _entityManager = entityManager;
        _assetStore = assetStore;
        _camera = camera;
        _graphicsDevice = graphicsDevice;
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

        _spriteBatch.End();
    }

    public void DrawHotbar()
    {

        for (int i = 0; i < 9; i++)
        {
            int defaultSlotSize = 22;
            int defaultSpacing = 20;
            int slotWidth = (int)(defaultSpacing * Constants.ScaleFactor);
            int hotbarWidth = (int)(slotWidth * 9);
            // int x = (int)(_camera.Position.X + _graphicsDevice.Viewport.Width - hotbarWidth/2);
            int x = (int)(_camera.Position.X + 50);
            int y = (int)(_camera.Position.Y + 6 * (Constants.TileSize * Constants.ScaleFactor));

            _spriteBatch.Draw(
                _assetStore.UIsheet,
                new Vector2(x + slotWidth * i, y),
                new Rectangle(0, 0, defaultSlotSize, defaultSlotSize),
                Color.White,
                0f,
                Vector2.Zero,
                Constants.ScaleFactor,
                SpriteEffects.None,
                0f);
        }

        for (int i = 0; i < 9; i++)
        {

            int defaultSlotSize = 22;
            int defaultSpacing = 20;
            int slotWidth = (int)(defaultSpacing * Constants.ScaleFactor);
            int hotbarWidth = (int)(slotWidth * 9);
            // int x = (int)(_camera.Position.X + _graphicsDevice.Viewport.Width - hotbarWidth/2);
            int x = (int)(_camera.Position.X + 50);
            int y = (int)(_camera.Position.Y + 6 * (Constants.TileSize * Constants.ScaleFactor));

            var inv = _entityManager.EntitiesWithComponent<InventoryComponent>().First().GetComponent<InventoryComponent>();

            if (inv.Items[i] == inv.activeItem)
            {
                _spriteBatch.Draw(
                    _assetStore.UIsheet,
                    new Vector2(x + slotWidth * i, y),
                    new Rectangle(32, 0, defaultSlotSize, defaultSlotSize),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    Constants.ScaleFactor,
                    SpriteEffects.None,
                    0f);
            }

            int iconOffset = (int)(3 * Constants.ScaleFactor);

            if (inv.Items[i].Type != ItemType.Empty)
            {
                _spriteBatch.Draw(
                    _assetStore.IconSheet,
                    new Vector2(x + slotWidth * i + iconOffset, y + iconOffset),
                    inv.Items[i].SourceRectangle,
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