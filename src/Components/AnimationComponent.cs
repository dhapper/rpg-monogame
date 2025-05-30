using Microsoft.Xna.Framework;

public class AnimationComponent
{
    public int FrameCount { get; set; }
    public int CurrentFrame { get; set; }
    public float FrameDuration { get; set; } // in seconds
    public float Timer { get; set; }
    public int Row { get; set; } = 0;
    public bool IsMirrored;

    public AnimationComponent(AnimationConfig config)
    {
        Row = config.Row;
        FrameCount = config.FrameCount;
        FrameDuration = config.FrameDuration;
        IsMirrored = config.IsMirrored;
        CurrentFrame = 0;
        Timer = 0f;
    }

}