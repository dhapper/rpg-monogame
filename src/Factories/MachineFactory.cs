public class MachineFactory
{
    public static Entity CreateMachine(MachineConfig config, EntityManager entityManager, int col, int row)
    {
        var entity = entityManager.CreateEntity();

        entity.AddComponent(new MachineComponent()
        {
            Config = config
        });

        entity.AddComponent(new SpriteComponent(AssetStore.MachineSprites, config.SourceRect));
        var pos = new PositionComponent(col * Constants.TileSize, row * Constants.TileSize)
        {
            Col = col,
            Row = row
        };
        entity.AddComponent(pos);
        entity.AddComponent(new CollisionComponent(pos, 0, 0, Constants.DefaultTileSize, Constants.DefaultTileSize));

        return entity;
    }
}