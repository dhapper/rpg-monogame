using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;

public static class Constants
{
    public const float ScaleFactor = 4f;
    public const float DroppedItemScaleFactor = 2f;
    public const int DefaultTileSize = 16; // 16x16
    public const int TileSize = (int)(DefaultTileSize * ScaleFactor);

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
            public static readonly AnimationConfig PickAxeRight = new AnimationConfig(10, 5, 0.1f, false, [0.15f, 0.15f, 0.075f, 0.075f, 0.2f]);
            public static readonly AnimationConfig PickAxeLeft = new AnimationConfig(10, 5, 0.1f, true, [0.15f, 0.15f, 0.075f, 0.075f, 0.2f]);
        }
    }

    public static class Tile
    {
        public const string PathsSheetName = "Tileset1";
        public const string CollisionSheetIndex = "Tileset2";
        public const string WaterSheetIndex = "Tileset3";

        public static string[] SolidTilesets = { CollisionSheetIndex, WaterSheetIndex };

    }

    public static class Items
    {
        public static readonly ItemConfig WateringCan = new ItemConfig("WateringCan", ItemType.Tool, new Rectangle(0 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize));
        public static readonly ItemConfig Pickaxe = new ItemConfig("Pickaxe", ItemType.Tool, new Rectangle(1 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize));
        public static readonly ItemConfig Seeds = new ItemConfig("Seeds", ItemType.Plantable, new Rectangle(2 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize));
        public static readonly ItemConfig Empty = new ItemConfig("Empty", ItemType.Empty, new Rectangle(0, 0, 0, 0));
    }

    public static class Crops
    {
        public const int DefaultSpriteSize = 48;
        public const int DefaultStages = 4;

        public static readonly CropConfig Pumpkin = new CropConfig("Pumpkin", DefaultStages, new Rectangle(0, 0 * DefaultSpriteSize, DefaultSpriteSize, DefaultSpriteSize));
        public static readonly CropConfig Potato = new CropConfig("Potato", DefaultStages, new Rectangle(0, 1 * DefaultSpriteSize, DefaultSpriteSize, DefaultSpriteSize));
    }

    public static class UI
    {
        public static class Inventory
        {
            public const int Cols = 9;
            public const int Rows = 4;
            public const int FirstInventoryRowIndex = 1;

            public const int DefaultSlotSize = 22;
            public const int SlotSize = (int)(22 * Constants.ScaleFactor);
            public const int IconOffset = (int)(3 * Constants.ScaleFactor);

            public const int CollectBoxSize = (int)(Constants.DefaultTileSize * Constants.DroppedItemScaleFactor);
        }
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
