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
                    Constants.ScaleFactor,
                    SpriteEffects.None,
                    0f);
                DrawHitbox(_spriteBatch, entity.GetComponent<Position>());
            }
        }
        _spriteBatch.End();
    }

    public void DrawHitbox(SpriteBatch spriteBatch, Position position)
    {
        Texture2D whitePixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        whitePixel.SetData(new[] { Color.White });

        Rectangle rect = position.GetHitbox();
        Color color = Color.Red;

        // Top
        spriteBatch.Draw(whitePixel, new Rectangle(rect.Left, rect.Top, rect.Width, 1), color);
        // Bottom
        spriteBatch.Draw(whitePixel, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), color);
        // Left
        spriteBatch.Draw(whitePixel, new Rectangle(rect.Left, rect.Top, 1, rect.Height), color);
        // Right
        spriteBatch.Draw(whitePixel, new Rectangle(rect.Right, rect.Top, 1, rect.Height), color);
    }

}