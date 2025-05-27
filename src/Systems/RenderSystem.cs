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
            if (entity.HasComponent<Position>() && entity.HasComponent<Sprite>())
            {
                var position = entity.GetComponent<Position>();
                var sprite = entity.GetComponent<Sprite>();
                Console.WriteLine($"Drawing at ({position.X},{position.Y}) with rect: {sprite.SourceRectangle}");
                _spriteBatch.Draw(
                    sprite.Texture,
                    new Vector2(position.X, position.Y),
                    sprite.SourceRectangle,
                    sprite.Color,
                    0f,
                    Vector2.Zero,
                    position.Scale,
                    SpriteEffects.None,
                    0f);
            }
        }
        _spriteBatch.End();
    }
}