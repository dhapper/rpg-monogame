public class Animation
{
    public int FrameCount { get; set; }
    public int CurrentFrame { get; set; }
    public float FrameDuration { get; set; } // in seconds
    public float Timer { get; set; }

    public Animation(int frameCount, float frameDuration)
    {
        FrameCount = frameCount;
        FrameDuration = frameDuration;
        CurrentFrame = 0;
        Timer = 0f;
    }
}