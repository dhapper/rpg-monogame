using System;

public class InteractionComponent
{
    public Action<Entity> InteractAction;

    public InteractionComponent(Action<Entity> action)
    {
        InteractAction = action;
    }

    public void Execute(Entity entity)
    {
        InteractAction?.Invoke(entity);
    }
}