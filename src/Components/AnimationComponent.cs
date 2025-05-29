using Microsoft.Xna.Framework;

public class AnimationComponent
{
    public int FrameCount { get; set; }
    public int CurrentFrame { get; set; }
    public float FrameDuration { get; set; } // in seconds
    public float Timer { get; set; }
    public int Row { get; set; } = -1;
    public SpriteComponent Sprite;
    public int FrameWidth, FrameHeight;

    public AnimationComponent(AnimationConfig config, SpriteComponent sprite)
    {
        Sprite = sprite;
        UpdateAnimation(config);
    }

    public void UpdateAnimation(AnimationConfig config)
    {
        if (config.Row != Row)
        {
            Row = config.Row;
            FrameCount = config.FrameCount;
            FrameDuration = config.FrameDuration;
            CurrentFrame = 0;
            Timer = 0f;
            UpdateSourceRectNow();
        }
    }

    public void UpdateSourceRectNow()
    {
        Rectangle rect = Sprite.SourceRectangle ?? new Rectangle(0, 0, 0, 0);

        Sprite.SourceRectangle = new Rectangle(
            CurrentFrame * rect.Width,
            Row * rect.Height,
            rect.Width,
            rect.Height);
    }

}