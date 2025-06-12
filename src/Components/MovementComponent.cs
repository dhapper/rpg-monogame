using System;

public class MovementComponent
{
    public float Speed;
    public Boolean IsMoving;
    public int LastDir = Constants.Directions.Right;

    public MovementComponent(float speed)
    {
        Speed = speed;
    }
}