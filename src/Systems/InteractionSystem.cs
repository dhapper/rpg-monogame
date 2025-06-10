public class InteractionSystem
{
    private EntityManager _entityManager;
    private Entity _player;

    public InteractionSystem(EntityManager entityManager)
    {
        _entityManager = entityManager;
    }

    public void CheckInteraction(Entity entity)
    {
        if (entity != null && entity.HasComponent<InteractionComponent>())
        {
            var interactable = entity.GetComponent<InteractionComponent>();
            interactable.Execute(entity);
        }
    }

    public Entity GetTile((int x, int y) mouse, Camera2D camera)
    {
        float worldX = mouse.x + camera.Position.X;
        float worldY = mouse.y + camera.Position.Y;
        int tileSize = (int)(Constants.DefaultTileSize * Constants.ScaleFactor);
        int col = (int)(worldX / tileSize);
        int row = (int)(worldY / tileSize);
        foreach (var entity in _entityManager.EntitiesWithComponent<TileComponent>())
        {
            var position = entity.GetComponent<PositionComponent>();
            if ((int)(position.X / tileSize) == col && (int)(position.Y / tileSize) == row)
            {
                // Console.WriteLine(position.X / tileSize + " " + position.Y / tileSize);
                return entity;
            }
        }
        return null;
    }

}