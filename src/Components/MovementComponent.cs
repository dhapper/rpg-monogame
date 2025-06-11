using System;

public class MovementComponent
{
    public float Speed;
    public Boolean IsMoving;
    public int LastDir = -1;

    public MovementComponent(float speed)
    {
        Speed = speed;
    }
}