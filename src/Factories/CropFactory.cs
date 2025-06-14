using Microsoft.Xna.Framework;

public class CropFactory
{
    public static Entity CreateCrop(CropConfig cropConfig, int row, int col, EntityManager entityManager, (float x, float y) tilePos)
    {
        var crop = entityManager.CreateEntity();

        crop.AddComponent(new CropComponent
        {
            config = cropConfig.Clone(),
            Row = row,
            Col = col
        } );
        crop.GetComponent<CropComponent>().config.TilePosition = tilePos;
        crop.AddComponent(new PositionComponent(tilePos.x - Constants.TileSize, tilePos.y - Constants.TileSize, Constants.Crops.DefaultSpriteSize, Constants.Crops.DefaultSpriteSize));
        crop.AddComponent(new SpriteComponent(AssetStore.CropSprites, cropConfig.SourceRectangle) { Color = Color.White });
        crop.AddComponent(new CollisionComponent(
            crop.GetComponent<PositionComponent>(),
            0,
            0,
            16,
            16,
            false
            ));

        return crop;
    }
}