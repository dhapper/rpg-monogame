using System;

public class ZoneComponent
{
    public bool InZone = false;
    public ZoneType Type;   // is this necessary if i just invoke ZoneAction?
    public Action ZoneAction;

    public ZoneComponent(ZoneType type, Action zoneAction)
    {
        Type = type;
        ZoneAction = zoneAction;
    }
}