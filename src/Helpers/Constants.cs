using Microsoft.Xna.Framework;

public static class Constants
{
    public const float ScaleFactor = 4f;
    public const int TileSize = 16; // 16x16

    public static class Player
    {
        public const int SpriteSize = 48;
        public const int XOffset = 19;
        public const int YOffset = 18;
        public const int HitboxWidth = 10;
        public const int HitboxHeight = 13;

        public const int Speed = 4;

        public static class Animations
        {
            public static readonly AnimationConfig IdleRight = new AnimationConfig(0, 4, 0.5f);
            public static readonly AnimationConfig IdleLeft = new AnimationConfig(0, 4, 0.5f, true);
            public static readonly AnimationConfig WalkRight = new AnimationConfig(3, 8, 0.1f);
            public static readonly AnimationConfig WalkLeft = new AnimationConfig(3, 8, 0.1f, true);
            public static readonly AnimationConfig WateringCanRight = new AnimationConfig(9, 6, 0.1f);
            public static readonly AnimationConfig WateringCanLeft = new AnimationConfig(9, 6, 0.1f, true);
            public static readonly AnimationConfig PickAxeRight = new AnimationConfig(10, 5, 0.1f);
            public static readonly AnimationConfig PickAxeLeft = new AnimationConfig(10, 5, 0.1f, true);
        }
    }

    public static class Tile
    {
        public const string PathsSheetName = "Tileset1";
        public const string CollisionSheetIndex = "Tileset2";
        public const string WaterSheetIndex = "Tileset3";

        public static string[] SolidTilesets = { CollisionSheetIndex, WaterSheetIndex };

    }

    public static class ColourIndex
    {
        public static readonly Color Hair = new Color(255, 0, 0);
        public static readonly Color HairShine = new Color(0, 0, 255);
    }

    public static class Directions
    {
        public const int Up = 0;
        public const int Down = 1;
        public const int Left = 2;
        public const int Right = 3;
    }

}
