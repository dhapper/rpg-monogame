using Microsoft.Xna.Framework;

public class CollisionComponent
{
    public Rectangle Hitbox { get; set; }
    public bool IsSolid { get; set; }
    public float OffsetX, OffsetY;
    public float HitboxWidth, HitboxHeight;

    public CollisionComponent(PositionComponent position, int offsetX, int offsetY, int hitboxWidth, int hitboxHeight, bool isSolid = true)
    {
        IsSolid = isSolid;
        OffsetX = offsetX * Constants.ScaleFactor;
        OffsetY = offsetY * Constants.ScaleFactor;
        HitboxWidth = hitboxWidth * Constants.ScaleFactor;
        HitboxHeight = hitboxHeight * Constants.ScaleFactor;

        UpdateHitbox(position);
    }

    public void UpdateHitbox(PositionComponent position)
    {
        Hitbox = new Rectangle(
            (int)(position.X + OffsetX),
            (int)(position.Y + OffsetY),
            (int)HitboxWidth,
            (int)HitboxHeight);
    }
}