using Microsoft.Xna.Framework;

public static class PlayerFactory
{
    public static Entity CreatePlayer(EntityManager entityManager, Microsoft.Xna.Framework.Graphics.Texture2D texture)
    {
        var player = entityManager.CreateEntity();

        int spriteWidth = 19;
        int spriteHeight = 21;

        player.AddComponent(new Position(100, 100));
        player.AddComponent(new Sprite(texture, new Rectangle(0, 0, spriteWidth, spriteHeight)) { Color = Color.Red });
        player.AddComponent(new Animation(frameCount: 4, frameDuration: 0.2f));

        return player;
    }
}
