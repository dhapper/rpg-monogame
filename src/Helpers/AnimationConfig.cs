public class AnimationConfig
{
    public int Row { get; }
    public int FrameCount { get; }
    public float FrameDuration { get; }

    public AnimationConfig(int row, int frameCount, float frameDuration)
    {
        Row = row;
        FrameCount = frameCount;
        FrameDuration = frameDuration;
    }

}