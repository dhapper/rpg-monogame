using Microsoft.Xna.Framework;

public class CropConfig
{
    public string Name { get; set; }
    public Rectangle SourceRectangle;
    public int Stages;
    public int CurrentStage = 1;
    public (float x, float y) TilePosition { get; set; }

    public CropConfig(string name, int stages, Rectangle sourceRectangle)
    {
        Name = name;
        Stages = stages;
        SourceRectangle = sourceRectangle;
    }

    public CropConfig Clone()
    {
        return new CropConfig(this.Name, this.Stages, this.SourceRectangle);
    }
}