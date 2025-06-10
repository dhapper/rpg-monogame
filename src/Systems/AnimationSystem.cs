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
        var anim = entity.GetComponent<AnimationComponent>();

        if (anim.Row != config.Row || anim.IsMirrored != config.IsMirrored)
        {
            anim.Row = config.Row;
            anim.FrameCount = config.FrameCount;
            anim.FrameDuration = config.FrameDuration;
            anim.IsMirrored = config.IsMirrored;
            anim.CurrentFrame = 0;
            anim.Timer = 0f;
            anim.FrameDurations = config.FrameDurations;


            UpdateSourceRect(entity);
        }
    }

    private void UpdateSourceRect(Entity entity)
    {
        var anim = entity.GetComponent<AnimationComponent>();
        var sprite = entity.GetComponent<SpriteComponent>();
        Rectangle rect = sprite.SourceRectangle ?? new Rectangle(0, 0, 0, 0);

        sprite.SourceRectangle = new Rectangle(
            anim.CurrentFrame * rect.Width,
            anim.Row * rect.Height,
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

            animation.EndOfOneAnimationCycle = false;

            if (animation.FrameDurations != null)
            {
                float currentFrameDuration = animation.FrameDurations[animation.CurrentFrame];
                if (animation.Timer >= currentFrameDuration)
                {
                    animation.Timer -= currentFrameDuration;
                    int previousFrame = animation.CurrentFrame;
                    animation.CurrentFrame = (animation.CurrentFrame + 1) % animation.FrameCount;

                    if (animation.CurrentFrame == 0 && previousFrame != 0)
                    {
                        animation.EndOfOneAnimationCycle = true;
                        break;
                    }

                    Rectangle rect = sprite.SourceRectangle ?? new Rectangle(0, 0, 32, 32);
                    sprite.SourceRectangle = new Rectangle(
                        animation.CurrentFrame * rect.Width,
                        animation.Row * rect.Height,
                        rect.Width,
                        rect.Height);
                }
            }
            else
            {
                if (animation.Timer >= animation.FrameDuration)
                {
                    animation.Timer -= animation.FrameDuration;
                    int previousFrame = animation.CurrentFrame;
                    animation.CurrentFrame = (animation.CurrentFrame + 1) % animation.FrameCount;

                    if (animation.CurrentFrame == 0 && previousFrame != 0)
                    {
                        animation.EndOfOneAnimationCycle = true;
                        break;
                    }

                    Rectangle rect = sprite.SourceRectangle ?? new Rectangle(0, 0, 32, 32);
                    sprite.SourceRectangle = new Rectangle(
                        animation.CurrentFrame * rect.Width,
                        animation.Row * rect.Height,
                        rect.Width,
                        rect.Height);
                }
            }

        }
    }

}