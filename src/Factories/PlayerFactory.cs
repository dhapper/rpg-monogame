using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class PlayerFactory
{

    public static Entity CreatePlayer(int x, int y, EntityManager entityManager, GraphicsDevice graphicsDevice, InventorySystem inventorySystem)
    {
        var player = entityManager.CreateEntity();

        // Color[][] colourChanges = [
        //     [Constants.ColourIndex.Hair, new Color(0, 0, 0)],
        //     [Constants.ColourIndex.HairShine, new Color(40, 40, 40)]
        // ];
        // Texture2D hairSheet = SpriteProcessor.ChangeColours(colourChanges, assets.PlayerHair, graphicsDevice);
        // Texture2D layeredSheet = SpriteProcessor.LayerSheets([assets.PlayerBody, hairSheet, assets.PlayerTools], graphicsDevice);

        player.AddComponent(new CharacterComponent());
        player.AddComponent(new PositionComponent(x, y, Constants.Player.SpriteSize, Constants.Player.SpriteSize));
        player.AddComponent(new SpriteComponent(AssetStore.PlayerSheet, new Rectangle(0, 0, Constants.Player.SpriteSize, Constants.Player.SpriteSize)) { Color = Color.White });
        player.AddComponent(new AnimationComponent(Constants.Animations.Idle));
        player.AddComponent(new CollisionComponent(
            player.GetComponent<PositionComponent>(),
            Constants.Player.XOffset,
            Constants.Player.YOffset,
            Constants.Player.HitboxWidth,
            Constants.Player.HitboxHeight));
        player.AddComponent(new MovementComponent(Constants.Player.Speed));

        var inv = new InventoryComponent();
        player.AddComponent(inv);
        inventorySystem.InitInventory(inv);


        return player;
    }
}
