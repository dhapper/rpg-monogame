public class AnimationConfig
{
    public int Row { get; }
    public int FrameCount { get; }
    public float FrameDuration { get; }
    public bool IsMirrored;

    public AnimationConfig(int row, int frameCount, float frameDuration, bool isMirrored = false)
    {
        Row = row;
        FrameCount = frameCount;
        FrameDuration = frameDuration;
        IsMirrored = isMirrored;
    }

}