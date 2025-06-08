using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class PlayerFactory
{

    public static Entity CreatePlayer(int x, int y, EntityManager entityManager, AssetStore assets, GraphicsDevice graphicsDevice)
    {
        var player = entityManager.CreateEntity();

        Color[][] colourChanges = [
            [Constants.ColourIndex.Hair, new Color(0, 0, 0)],
            [Constants.ColourIndex.HairShine, new Color(40, 40, 40)]
        ];
        Texture2D hairSheet = SpriteProcessor.ChangeColours(colourChanges, assets.PlayerHair, graphicsDevice);
        Texture2D layeredSheet = SpriteProcessor.LayerSheets([assets.PlayerBody, hairSheet, assets.PlayerTools], graphicsDevice);

        player.AddComponent(new PositionComponent(x, y, Constants.Player.SpriteSize, Constants.Player.SpriteSize));
        player.AddComponent(new SpriteComponent(layeredSheet, new Rectangle(0, 0, Constants.Player.SpriteSize, Constants.Player.SpriteSize)) { Color = Color.White });
        player.AddComponent(new AnimationComponent(Constants.Player.Animations.IdleRight));
        player.AddComponent(new CollisionComponent(
            player.GetComponent<PositionComponent>(),
            Constants.Player.XOffset,
            Constants.Player.YOffset,
            Constants.Player.HitboxWidth,
            Constants.Player.HitboxHeight));
        player.AddComponent(new MovementComponent(Constants.Player.Speed));
        player.AddComponent(new InventoryComponent());


        return player;
    }
}
