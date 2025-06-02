public class InteractionSystem
{
    private EntityManager _entityManager;
    private Entity _player;

    public InteractionSystem(EntityManager entityManager, Entity player)
    {
        _entityManager = entityManager;
        _player = player;
    }

    public void CheckInteraction(Entity entity)
    {
        if (entity != null && entity.HasComponent<InteractionComponent>())
        {
            var interactable = entity.GetComponent<InteractionComponent>();
            interactable.Execute(entity);
        }
    }
}