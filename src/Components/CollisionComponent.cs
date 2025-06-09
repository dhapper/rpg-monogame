using Microsoft.Xna.Framework;

public class CollisionComponent
{
    public Rectangle Hitbox { get; set; }
    public bool isSolid { get; set; }

    public int offsetX, offsetY, hitboxWidth, hitboxHeight;

    public CollisionComponent(PositionComponent position, int offsetX, int offsetY, int hitboxWidth, int hitboxHeight, bool isSolid = true)
    {
        this.isSolid = isSolid;

        this.offsetX = offsetX;
        this.offsetY = offsetY;
        this.hitboxWidth = hitboxWidth;
        this.hitboxHeight = hitboxHeight;

        UpdateHitbox(position);
    }

    public void UpdateHitbox(PositionComponent position)
    {
        Hitbox = new Rectangle(
            (int)(position.X + offsetX * Constants.ScaleFactor),
            (int)(position.Y + offsetY * Constants.ScaleFactor),
            (int)(hitboxWidth * Constants.ScaleFactor),
            (int)(hitboxHeight * Constants.ScaleFactor));
            
    }
}