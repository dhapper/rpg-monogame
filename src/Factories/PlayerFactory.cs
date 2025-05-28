using Microsoft.Xna.Framework;

public static class PlayerFactory
{
    public static Entity CreatePlayer(int x, int y, EntityManager entityManager, Microsoft.Xna.Framework.Graphics.Texture2D texture)
    {
        var player = entityManager.CreateEntity();

        player.AddComponent(new Position(x, y, Constants.Player.Width, Constants.Player.Height));
        player.AddComponent(new Sprite(texture, new Rectangle(0, 0, Constants.Player.Width, Constants.Player.Height)) { Color = Color.White });
        player.AddComponent(new Animation(frameCount: 4, frameDuration: 0.2f));
        player.AddComponent(new Collision(player.GetComponent<Position>(), true));
        

        return player;
    }
}
