
public static class Constants
{
    public const float ScaleFactor = 4f;
    public const int TileSize = 16; // 16x16

    public static class Player
    {
        // public const int Height = 21;
        // public const int Width = 19;
        public const int SpriteSize = 64;

        public const int XOffset = 24;
        public const int YOffset = 18;
        public const int HitboxWidth = 16;
        public const int HitboxHeight = 30;

        public const int Speed = 4;

        public static class Animations
        {
            // Row, FrameCount, Duration
            public static readonly AnimationConfig IdleDown = new AnimationConfig(0, 4, 1f);
            public static readonly AnimationConfig IdleUp = new AnimationConfig(1, 4, 1f);
            public static readonly AnimationConfig IdleRight = new AnimationConfig(2, 4, 1f);
            public static readonly AnimationConfig IdleLeft = new AnimationConfig(2, 4, 1f, true);

            public static readonly AnimationConfig WalkDown = new AnimationConfig(3, 6, 0.15f);
            public static readonly AnimationConfig WalkUp = new AnimationConfig(4, 6, 0.15f);
            public static readonly AnimationConfig WalkRight = new AnimationConfig(5, 6, 0.15f);
            public static readonly AnimationConfig WalkLeft = new AnimationConfig(5, 6, 0.15f, true);

            public static readonly AnimationConfig RunDown = new AnimationConfig(6, 6, 0.1f);
            public static readonly AnimationConfig RunUp = new AnimationConfig(7, 6, 0.1f);
            public static readonly AnimationConfig RunRight = new AnimationConfig(8, 6, 0.1f);
            public static readonly AnimationConfig RunLeft = new AnimationConfig(8, 6, 0.1f, true);

            public static readonly AnimationConfig AxeDown = new AnimationConfig(9, 8, 0.1f);
            public static readonly AnimationConfig AxeUp = new AnimationConfig(10, 8, 0.1f);
            public static readonly AnimationConfig AxeRight = new AnimationConfig(11, 8, 0.1f);
            public static readonly AnimationConfig AxeLeft = new AnimationConfig(11, 8, 0.1f, true);
        }
    }

    public static class Directions
    {
        public const int Up = 0;
        public const int Down = 1;
        public const int Left = 2;
        public const int Right = 3;
    }
    
}
