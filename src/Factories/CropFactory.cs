using System;
using Microsoft.Xna.Framework;

public class CropFactory
{
    public static Entity CreateCrop(CropConfig cropConfig, int row, int col, EntityManager entityManager, (float x, float y) tilePos, int stage = 1)
    {
        var crop = entityManager.CreateEntity();

        crop.AddComponent(new CropComponent
        {
            config = cropConfig.Clone(),
            Row = row,
            Col = col
        } );

        crop.GetComponent<CropComponent>().config.TilePosition = tilePos;

        crop.GetComponent<CropComponent>().config.CurrentStage = stage;
        Rectangle sourceRect = new Rectangle((stage - 1) * Constants.Crops.DefaultSpriteSize, cropConfig.SourceRectangle.Y, cropConfig.SourceRectangle.Width, cropConfig.SourceRectangle.Height);
        crop.AddComponent(new SpriteComponent(AssetStore.CropSprites, sourceRect) {Color = Color.White });
        // crop.AddComponent(new SpriteComponent(AssetStore.CropSprites, cropConfig.SourceRectangle) {Color = Color.White });
        Console.WriteLine(crop.GetComponent<CropComponent>().config.CurrentStage);

        crop.AddComponent(new PositionComponent(tilePos.x - Constants.TileSize, tilePos.y - Constants.TileSize, Constants.Crops.DefaultSpriteSize, Constants.Crops.DefaultSpriteSize));
        // crop.AddComponent(new SpriteComponent(AssetStore.CropSprites, cropConfig.SourceRectangle) { Color = Color.White });
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