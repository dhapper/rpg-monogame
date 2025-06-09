using Microsoft.Xna.Framework;

public class ItemFactory
{
    public static Entity CreateItem(ItemConfig itemConfig, EntityManager entityManager, AssetStore assets)
    {
        var item = entityManager.CreateEntity();

        item.AddComponent(new ItemComponent { config = itemConfig.Clone() });
        item.AddComponent(new PositionComponent(-1, -1, 32, 32));
        item.AddComponent(new SpriteComponent(assets.IconSheet, itemConfig.SourceRectangle) { Color = Color.White });
        item.AddComponent(new CollisionComponent(
            item.GetComponent<PositionComponent>(),
            0,
            0,
            16,
            16,
            false
            ));

        return item;
    }
}