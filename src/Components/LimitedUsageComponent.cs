public class LimitedUsageComponent
{
    public int MaxCapacity;
    public int CurrentCapacity;

    public LimitedUsageComponent(int maxCapacity, int currentCapacity = -1)
    {
        MaxCapacity = maxCapacity;
        CurrentCapacity = currentCapacity != -1 ? currentCapacity : maxCapacity;
    }
}