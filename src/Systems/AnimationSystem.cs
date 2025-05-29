using System;
using Microsoft.Xna.Framework;

public class AnimationSystem
{
    private EntityManager _entityManager;

    public AnimationSystem(EntityManager entityManager)
    {
        _entityManager = entityManager;
    }

    public void SetAnimation(Entity entity, AnimationConfig config)
    {
        var animation = entity.GetComponent<AnimationComponent>();

        if (animation.Row != config.Row)
        {
            animation.Row = config.Row;
            animation.FrameCount = config.FrameCount;
            animation.FrameDuration = config.FrameDuration;
            animation.CurrentFrame = 0;
            animation.Timer = 0f;

            UpdateSourceRectNow(entity);
        }
    }

    private void UpdateSourceRectNow(Entity entity)
    {
        var animation = entity.GetComponent<AnimationComponent>();
        var sprite = entity.GetComponent<SpriteComponent>();
        Rectangle rect = sprite.SourceRectangle ?? new Rectangle(0, 0, 32, 32); // fallback

        sprite.SourceRectangle = new Rectangle(
            animation.CurrentFrame * rect.Width,
            animation.Row * rect.Height,
            rect.Width,
            rect.Height);
    }


    public void Update(GameTime gameTime)
{
    foreach (var entity in _entityManager.EntitiesWithComponents<AnimationComponent, SpriteComponent>())
    {
        var animation = entity.GetComponent<AnimationComponent>();
        var sprite = entity.GetComponent<SpriteComponent>();

        animation.Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (animation.Timer >= animation.FrameDuration)
        {
            animation.Timer -= animation.FrameDuration;
            animation.CurrentFrame = (animation.CurrentFrame + 1) % animation.FrameCount;

            Rectangle rect = sprite.SourceRectangle ?? new Rectangle(0, 0, 32, 32);

            sprite.SourceRectangle = new Rectangle(
                animation.CurrentFrame * rect.Width,
                animation.Row * rect.Height,
                rect.Width,
                rect.Height);

            Console.WriteLine($"Frame: {animation.CurrentFrame}, Rect: {sprite.SourceRectangle}");
        }
    }
}

}