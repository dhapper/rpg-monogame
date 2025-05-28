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
            if (entity.HasComponent<PositionComponent>() && entity.HasComponent<SpriteComponent>())
            {

                if (entity.HasComponent<CollisionComponent>() && entity.GetComponent<CollisionComponent>().isSolid)
                {
                    DrawHitbox(_spriteBatch, entity.GetComponent<CollisionComponent>().Hitbox);
                }

                var position = entity.GetComponent<PositionComponent>();
                var sprite = entity.GetComponent<SpriteComponent>();
                Console.WriteLine($"Drawing at ({position.X},{position.Y}) with rect: {sprite.SourceRectangle}");
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
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Bottom, hitbox.Width, 1), color);
        // Left
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Top, 1, hitbox.Height), color);
        // Right
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Right, hitbox.Top, 1, hitbox.Height), color);

    }

}