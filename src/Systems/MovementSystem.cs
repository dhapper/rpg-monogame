using Microsoft.Xna.Framework;

public class MovementSystem
{

    public Vector2 CalculateSpeed(float speed, bool[] dir)
    {

        float xSpeed = 0f;
        float ySpeed = 0f;

        if (dir[Constants.Directions.Left])
            xSpeed -= speed;
        if (dir[Constants.Directions.Right])
            xSpeed += speed;
        if (dir[Constants.Directions.Up])
            ySpeed -= speed;
        if (dir[Constants.Directions.Down])
            ySpeed += speed;

        // cancel out opposing directions
        if (dir[Constants.Directions.Up] && dir[Constants.Directions.Down])
            ySpeed = 0;
        if (dir[Constants.Directions.Left] && dir[Constants.Directions.Right])
            xSpeed = 0;

        // diagonal speed
        if (xSpeed != 0 && ySpeed != 0)
        {
            float diagonalFactor = 0.7071f;
            xSpeed *= diagonalFactor;
            ySpeed *= diagonalFactor;
        }

        return new Vector2(xSpeed, ySpeed);
    }
}