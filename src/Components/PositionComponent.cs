public class PositionComponent
{
    public float X, Y;
    public int Width, Height;
    public int Col, Row;    // mainly for crops and machines currently (placed entities)

    public PositionComponent(float x, float y, int width = 16, int height = 16)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

}