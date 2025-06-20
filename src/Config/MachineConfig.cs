using Microsoft.Xna.Framework;

public class MachineConfig
{
    public string Name;
    public Rectangle SourceRect;
    public int DaysToProcess;

    public MachineConfig(string name, Rectangle sourceRect, int daysToProcess)
    {
        Name = name;
        SourceRect = sourceRect;
        DaysToProcess = daysToProcess;
    }

    public MachineConfig Clone()
    {
        return new MachineConfig(Name, SourceRect, DaysToProcess);
    }
}