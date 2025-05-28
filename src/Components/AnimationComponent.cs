public class AnimationComponent
{
    public int FrameCount { get; set; }
    public int CurrentFrame { get; set; }
    public float FrameDuration { get; set; } // in seconds
    public float Timer { get; set; }

    public int Row { get; set; } = 0;

    public AnimationComponent(int frameCount, float frameDuration)
    {
        FrameCount = frameCount;
        FrameDuration = frameDuration;
        CurrentFrame = 0;
        Timer = 0f;
    }
}