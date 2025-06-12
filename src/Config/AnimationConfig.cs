public class AnimationConfig
{
    public int Row { get; }
    public int FrameCount { get; }
    public float FrameDuration { get; }
    public bool IsMirrored;
    public float[] FrameDurations;

    // public AnimationConfig(int row, int frameCount, float frameDuration, int index = -1, bool isMirrored = false, float[] frameDurations = null)
    // public AnimationConfig(int row, int frameCount, float frameDuration, bool isMirrored = false, float[] frameDurations = null)
    public AnimationConfig(int row, int frameCount, float frameDuration, float[] frameDurations = null)
    {
        Row = row;
        FrameCount = frameCount;
        FrameDuration = frameDuration;
        // IsMirrored = isMirrored;
        FrameDurations = frameDurations;
    }

}