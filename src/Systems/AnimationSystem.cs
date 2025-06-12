using Microsoft.Xna.Framework;

public class AnimationSystem
{
    private EntityManager _entityManager;

    public AnimationSystem(EntityManager entityManager)
    {
        _entityManager = entityManager;
    }



    public (int aniDirIndex, bool mirrored) GetAniInitVars(int lastDir)
    {
        switch (lastDir)
        {
            case Constants.Directions.Right:
                return (Constants.Animations.Sideways, false);
            case Constants.Directions.Left:
                return (Constants.Animations.Sideways, true);
            case Constants.Directions.Up:
                return (Constants.Animations.Up, false);
            case Constants.Directions.Down:
                return (Constants.Animations.Down, false);
            default:
                return (-1, true);
        }
    }

    public void SetAnimation(Entity entity, AnimationConfig config, int directionIndex, bool mirrored = false)
    {
        var anim = entity.GetComponent<AnimationComponent>();

        if (anim.Row != config.Row || anim.Index != directionIndex || anim.IsMirrored != mirrored)
        {
            anim.Row = config.Row;
            anim.FrameCount = config.FrameCount;
            anim.FrameDuration = config.FrameDuration;
            // anim.IsMirrored = config.IsMirrored;
            anim.CurrentFrame = 0;
            anim.Timer = 0f;
            anim.FrameDurations = config.FrameDurations;

            anim.Index = directionIndex;
            anim.IsMirrored = mirrored;

            InitSourceRect(entity);
        }
    }

    private void InitSourceRect(Entity entity)
    {
        var anim = entity.GetComponent<AnimationComponent>();
        var sprite = entity.GetComponent<SpriteComponent>();
        Rectangle rect = sprite.SourceRectangle;

        sprite.SourceRectangle = new Rectangle(
            // anim.CurrentFrame * rect.Width,
            anim.Index == Constants.Animations.DefaultIndex ? 0 : rect.Width * anim.Index,
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

                    if (animation.Index == Constants.Animations.DefaultIndex)
                    {
                        Rectangle rect = sprite.SourceRectangle;
                        sprite.SourceRectangle = new Rectangle(
                            animation.CurrentFrame * rect.Width,
                            animation.Row * rect.Height,
                            rect.Width,
                            rect.Height);
                    }
                    else
                    {
                        Rectangle rect = sprite.SourceRectangle;
                        sprite.SourceRectangle = new Rectangle(
                            animation.CurrentFrame * rect.Width * 3 + animation.Index * rect.Width,
                            animation.Row * rect.Height,
                            rect.Width,
                            rect.Height);
                    }
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

                    // Rectangle rect = sprite.SourceRectangle;
                    // sprite.SourceRectangle = new Rectangle(
                    //     animation.CurrentFrame * rect.Width,
                    //     animation.Row * rect.Height,
                    //     rect.Width,
                    //     rect.Height);

                    if (animation.Index == Constants.Animations.DefaultIndex)
                    {
                        Rectangle rect = sprite.SourceRectangle;
                        sprite.SourceRectangle = new Rectangle(
                            animation.CurrentFrame * rect.Width,
                            animation.Row * rect.Height,
                            rect.Width,
                            rect.Height);
                    }
                    else
                    {
                        Rectangle rect = sprite.SourceRectangle;
                        sprite.SourceRectangle = new Rectangle(
                            animation.CurrentFrame * rect.Width * 3 + animation.Index * rect.Width,
                            animation.Row * rect.Height,
                            rect.Width,
                            rect.Height);
                    }
                }
            }

        }
    }

}