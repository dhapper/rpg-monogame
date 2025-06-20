using System;
using Microsoft.Xna.Framework;

public class PositionSystem
{
    public static Point GetMidPoint(Entity entity)
    {
        var posComp = entity.GetComponent<PositionComponent>();
        var collisionComp = entity.GetComponent<CollisionComponent>();
        var midX = posComp.X + collisionComp.OffsetX + collisionComp.HitboxWidth / 2;
        var midY = posComp.Y + collisionComp.OffsetY + collisionComp.HitboxHeight / 2;

        return new Point((int)midX, (int)midY);
    }

    public static void UpdateCurrentTilePos(Entity entity)
    {
        var posComp = entity.GetComponent<PositionComponent>();
        var collisionComp = entity.GetComponent<CollisionComponent>();
        var midpoint = GetMidPoint(entity);

        posComp.Col = midpoint.X / Constants.TileSize;
        posComp.Row = midpoint.Y / Constants.TileSize;

        posComp.OverStepUp = false;
        posComp.OverStepDown = false;
        posComp.OverStepLeft = false;
        posComp.OverStepRight = false;

        var maxY = midpoint.Y - collisionComp.HitboxHeight / 2;
        var minY = midpoint.Y + collisionComp.HitboxHeight / 2;

        var maxX = midpoint.X - collisionComp.HitboxWidth / 2;
        var minX = midpoint.X + collisionComp.HitboxWidth / 2;

        if (maxY < posComp.Row * Constants.TileSize)
            posComp.OverStepUp = true;
        else if (minY > posComp.Row * Constants.TileSize + Constants.TileSize)
            posComp.OverStepDown = true;

        if (maxX < posComp.Col * Constants.TileSize)
            posComp.OverStepLeft = true;
        else if (minX > posComp.Col * Constants.TileSize + Constants.TileSize)
            posComp.OverStepRight = true;
    }
}