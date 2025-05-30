using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class RenderSystem
{
    private SpriteBatch _spriteBatch;
    private EntityManager _entityManager;

    public RenderSystem(SpriteBatch spriteBatch, EntityManager entityManager)
    {
        _spriteBatch = spriteBatch;
        _entityManager = entityManager;
    }

    public void Draw()
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        foreach (var entity in _entityManager.GetEntities())
        {

            // var position = entity.GetComponent<PositionComponent>();
            // var sprite = entity.GetComponent<SpriteComponent>();
            // var animation = entity.GetComponent<AnimationComponent>();

            if (entity.HasComponent<TileComponent>())
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

            // temp fix
            if (entity.HasComponent<PositionComponent>() && entity.HasComponent<SpriteComponent>() && !entity.HasComponent<TileComponent>())
            {
                var position = entity.GetComponent<PositionComponent>();
                var sprite = entity.GetComponent<SpriteComponent>();
                var animation = entity.GetComponent<AnimationComponent>();

                // Console.WriteLine($"Drawing at ({position.X},{position.Y}) with rect: {sprite.SourceRectangle}");

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

            // hitboxes
            if (entity.HasComponent<CollisionComponent>()) {
                DrawHitbox(_spriteBatch, entity.GetComponent<CollisionComponent>().Hitbox);
            }
        }
        _spriteBatch.End();
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