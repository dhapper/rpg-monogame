using Microsoft.Xna.Framework;

public static class PlayerFactory
{
    public static Entity CreatePlayer(EntityManager entityManager, Microsoft.Xna.Framework.Graphics.Texture2D texture)
    {
        var player = entityManager.CreateEntity();

        player.AddComponent(new Position(100, 100, Constants.Player.Width, Constants.Player.Height));
        player.AddComponent(new Sprite(texture, new Rectangle(0, 0, Constants.Player.Width, Constants.Player.Height)) { Color = Color.Red });
        player.AddComponent(new Animation(frameCount: 4, frameDuration: 0.2f));

        return player;
    }
}
