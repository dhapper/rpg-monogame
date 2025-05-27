public struct Position
{
    public float X;
    public float Y;

    public float Scale { get; set; } = 4f; // Default scale

    public Position(float x, float y)
    {
        X = x;
        Y = y;
    }
}