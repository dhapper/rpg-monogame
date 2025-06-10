using Microsoft.Xna.Framework;

public class RenderHelper
{
    public static bool IsRectangleInCameraView(Rectangle rectangle, Rectangle cameraView)
    {
        Rectangle entityRectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        return entityRectangle.Intersects(cameraView);
    }

    public static bool IsEntityInCameraView(Entity entity, Rectangle cameraView)
    {
        var position = entity.GetComponent<PositionComponent>();
        var sprite = entity.GetComponent<SpriteComponent>();

        int width = sprite.SourceRectangle.Width;
        int height = sprite.SourceRectangle.Height;
        Rectangle entityRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

        return entityRectangle.Intersects(cameraView);
    }
}