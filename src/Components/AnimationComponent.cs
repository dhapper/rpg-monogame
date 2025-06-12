using Microsoft.Xna.Framework;

public class AnimationComponent
{
    public int FrameCount { get; set; }
    public int CurrentFrame { get; set; }
    public float FrameDuration { get; set; } // in seconds
    public float Timer { get; set; }
    public int Row { get; set; } = 0;
    public bool IsMirrored;
    public float[] FrameDurations;
    public int AnimationIndex;  // index of current frame
    public bool EndOfOneAnimationCycle = false;

    public int Index;   // index of type, eg sideways, up, and down idle on same row

    public AnimationComponent(AnimationConfig config)
    {
        Row = config.Row;
        FrameCount = config.FrameCount;
        FrameDuration = config.FrameDuration;
        IsMirrored = config.IsMirrored;
        FrameDurations = config.FrameDurations;
        CurrentFrame = 0;
        Timer = 0f;
        AnimationIndex = 0;
        EndOfOneAnimationCycle = false;


        // no index? idk tbh
    }

}