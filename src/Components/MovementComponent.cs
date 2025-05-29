using System;

public class MovementComponent
{
    public int Speed;
    public Boolean IsMoving;
    public int LastDir = -1;

    public MovementComponent(int speed)
    {
        Speed = speed;
    }
}