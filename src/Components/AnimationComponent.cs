public class AnimationComponent
{
    public int FrameCount { get; set; }
    public int CurrentFrame { get; set; }
    public float FrameDuration { get; set; }
    public float Timer { get; set; }
    public int Row { get; set; } = -1;
}
