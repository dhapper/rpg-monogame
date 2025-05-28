using Microsoft.Xna.Framework;

public class Collision
{
    public Rectangle Hitbox { get; private set; }
    public bool IsSolid { get; set; }

    public Collision(Position position, bool isSolid = false)
    {
        UpdateHitbox(position);
        IsSolid = isSolid;
    }

    public void UpdateHitbox(Position position)
    {
        Hitbox = new Rectangle(
            (int)position.X,
            (int)position.Y,
            (int)(position.Width * Constants.ScaleFactor),
            (int)(position.Height * Constants.ScaleFactor));
            
        // Hitbox = new Rectangle(
        //     (int) position.X,
        //     (int) position.Y,
        //     (int)(position.Width),
        //     (int)(position.Height));
    }
}