using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class RenderSystem
{
    private SpriteBatch _spriteBatch;
    private EntityManager _entityManager;
    private AssetStore _assetStore;

    public RenderSystem(SpriteBatch spriteBatch, EntityManager entityManager, AssetStore assetStore)
    {
        _spriteBatch = spriteBatch;
        _entityManager = entityManager;
        _assetStore = assetStore;
    }

    public void Draw()
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        foreach (var entity in _entityManager.GetEntities())
        {
            // tiles
            if (entity.HasComponent<TileComponent>())
            {
                // bg of tile for transparent tiles
                if (!entity.GetComponent<TileComponent>().Background.IsEmpty)
                {
                    var position = entity.GetComponent<PositionComponent>();
                    var sprite = entity.GetComponent<SpriteComponent>();
                    var tile = entity.GetComponent<TileComponent>();
                    _spriteBatch.Draw(
                        _assetStore.BackgroundTiles,
                        new Vector2(position.X, position.Y),
                        tile.Background,
                        sprite.Color,
                        0f,
                        Vector2.Zero,
                        Constants.ScaleFactor,
                        SpriteEffects.None,
                        0f);
                }
                DrawTile(entity);
            }

            // entities
            // temp params
            if (entity.HasComponent<PositionComponent>() && entity.HasComponent<SpriteComponent>() && !entity.HasComponent<TileComponent>())
                DrawEntity(entity);

            // hitboxes
            if (entity.HasComponent<CollisionComponent>())
                DrawHitbox(_spriteBatch, entity.GetComponent<CollisionComponent>().Hitbox);
        }
        _spriteBatch.End();
    }

    public void DrawTile(Entity entity)
    {
        var position = entity.GetComponent<PositionComponent>();
        var sprite = entity.GetComponent<SpriteComponent>();

        _spriteBatch.Draw(
            sprite.Texture,
            new Vector2(position.X, position.Y),
            sprite.SourceRectangle,
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
            new Vector2(position.X, position.Y),
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

        // Top
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Top, hitbox.Width, 1), color);
        // Bottom
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Bottom - 1, hitbox.Width, 1), color);
        // Left
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Top, 1, hitbox.Height), color);
        // Right
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Right - 1, hitbox.Top, 1, hitbox.Height), color);
    }

}