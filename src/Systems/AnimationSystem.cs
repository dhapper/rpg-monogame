using System;
using Microsoft.Xna.Framework;

public class AnimationSystem
{
    private EntityManager _entityManager;

    public AnimationSystem(EntityManager entityManager)
    {
        _entityManager = entityManager;
    }

    public void Update(GameTime gameTime)
    {
        foreach (var entity in _entityManager.EntitiesWithComponents<Animation, Sprite>())
        {
            var animation = entity.GetComponent<Animation>();
            var sprite = entity.GetComponent<Sprite>();

            animation.Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animation.Timer >= animation.FrameDuration)
            {
                animation.Timer -= animation.FrameDuration;
                animation.CurrentFrame = (animation.CurrentFrame + 1) % animation.FrameCount;

                // Update the sprite's source rectangle to show the current frame

                Rectangle rect = sprite.SourceRectangle ?? new Rectangle(0, 0, 10, 10);

                int frameWidth = rect.Width;
                int frameHeight = rect.Height;

                sprite.SourceRectangle = new Rectangle(
                    animation.CurrentFrame * frameWidth,
                    animation.Row * frameHeight,
                    frameWidth,
                    frameHeight);

                // Inside AnimationSystem.Update()
                Console.WriteLine($"Frame: {animation.CurrentFrame}, Rect: {sprite.SourceRectangle}");

            }
        }

    }
}