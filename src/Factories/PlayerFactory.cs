using Microsoft.Xna.Framework;

public static class PlayerFactory
{
    public static Entity CreatePlayer(int x, int y, EntityManager entityManager, Microsoft.Xna.Framework.Graphics.Texture2D texture)
    {
        var player = entityManager.CreateEntity();

        player.AddComponent(new PositionComponent(x, y, Constants.Player.Width, Constants.Player.Height));
        player.AddComponent(new SpriteComponent(texture, new Rectangle(0, 0, Constants.Player.Width, Constants.Player.Height)) { Color = Color.White });
        player.AddComponent(new AnimationComponent(frameCount: 4, frameDuration: 0.2f));
        player.AddComponent(new CollisionComponent(
            player.GetComponent<PositionComponent>(),
            Constants.Player.XOffset,
            Constants.Player.YOffset,
            Constants.Player.HitboxWidth, 
            Constants.Player.HitboxHeight));
        

        return player;
    }
}
