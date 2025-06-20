using System;

public class LimitedUsageSystem
{

    public bool CanUseItem(LimitedUsageComponent comp)
    {
        if (comp.CurrentCapacity > 0) { return true; }
        return false;
    }

    public void UseItem(LimitedUsageComponent comp)
    {
        comp.CurrentCapacity--;
        Console.WriteLine(comp.CurrentCapacity+" uses left");
    }

    public void RefillToCapacity(Entity item)
    {
        var comp = item.GetComponent<LimitedUsageComponent>();
        comp.CurrentCapacity = comp.MaxCapacity;
    }
}