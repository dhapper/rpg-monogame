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

                if (entity.HasComponent<Collision>() && entity.GetComponent<Collision>().IsSolid)
                {
                    DrawHitbox(_spriteBatch, entity.GetComponent<Collision>().Hitbox);
                }

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
            }
        }
        _spriteBatch.End();
    }

    // public void DrawHitbox(SpriteBatch spriteBatch, Rectangle hitbox)
    // {
    //     // Create a 1x1 white pixel texture
    //     using (Texture2D whitePixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1))
    //     {
    //         whitePixel.SetData(new[] { Color.White });
    //         // Draw the filled rectangle
    //         spriteBatch.Draw(whitePixel, hitbox, Color.Red); // Change Color as needed
    //     }
    // }

    public void DrawHitbox(SpriteBatch spriteBatch, Rectangle hitbox)
    {
        Texture2D whitePixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        whitePixel.SetData(new[] { Color.White });

        Color color = Color.White;

        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Top, hitbox.Width, 1), color);
        // Bottom
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Bottom, hitbox.Width, 1), color);
        // Left
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Top, 1, hitbox.Height), color);
        // Right
        spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Right, hitbox.Top, 1, hitbox.Height), color);

        // color = Color.Red;

        // int width = (int) (hitbox.Width * Constants.ScaleFactor);
        // int height = (int) (hitbox.Height * Constants.ScaleFactor);

        // // Top
        // spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Top, width, 1), color);
        // // Bottom
        // spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Top + height, width, 1), color);
        // // Left
        // spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left, hitbox.Top, 1, height), color);
        // // Right
        // spriteBatch.Draw(whitePixel, new Rectangle(hitbox.Left + width, hitbox.Top, 1, height), color);
    }

}