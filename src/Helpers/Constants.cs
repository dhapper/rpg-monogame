using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;

public static class Constants
{
    public const float ScaleFactor = 3;
    public const float DroppedItemScaleFactor = ScaleFactor / 2;
    public const int DefaultTileSize = 16; // 16x16
    public const int TileSize = (int)(DefaultTileSize * ScaleFactor);

    public static class Player
    {

        public const int SpriteSize = 80;
        public const int XOffset = 34;
        public const int YOffset = 33;
        public const int HitboxWidth = 11;
        public const int HitboxHeight = 14;

        public const float Speed = ScaleFactor;

    }

    public static class Animations
    {
        public const int DefaultIndex = -1;
        public const int Sideways = 0;
        public const int Down = 1;
        public const int Up = 2;

        public static readonly AnimationConfig Idle = new AnimationConfig(0, 4, 0.5f);
        public static readonly AnimationConfig Walk = new AnimationConfig(1, 8, 0.1f);

        public static readonly AnimationConfig Pickaxe = new AnimationConfig(7, 6, 0.1f);
        public static readonly AnimationConfig Watering = new AnimationConfig(14, 6, 0.1f);
    }

    public static class Tile
    {
        public const string PathsSheetName = "Tileset1";
        public const string CollisionSheetIndex = "Tileset2";
        public const string WaterSheetIndex = "Tileset3";

        public const int DrySoil = 40;
        public const int WetSoil = 41;
        public const int DryFertilizedSoil = 48;
        public const int WetFertilizedSoil = 49;


        public static readonly string[] SolidTilesets = { CollisionSheetIndex, WaterSheetIndex };

        // public static readonly int[] PlantableTiles = [DrySoil, WetSoil, DryFertilizedSoil, WetFertilizedSoil];
        public static readonly int[] WetSoilTiles = [WetSoil, WetFertilizedSoil];
        public static readonly int[] DrySoilTiles = [DrySoil, DryFertilizedSoil];
        public static readonly int[] PlantableTiles = WetSoilTiles.Concat(DrySoilTiles).ToArray();

        public static readonly Dictionary<int, int> OvernightSoilTransform = new()
        {
            { WetSoil, DrySoil },
            { WetFertilizedSoil, DryFertilizedSoil }
        };

        public static readonly Dictionary<int, int> WaterSoilTransform = new()
        {
            { DrySoil, WetSoil },
            { DryFertilizedSoil, WetFertilizedSoil }
        };

    }

    public static class Items
    {
        public static readonly ItemConfig WateringCan = new ItemConfig("WateringCan", ItemType.Tool, new Rectangle(0 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize));
        public static readonly ItemConfig Pickaxe = new ItemConfig("Pickaxe", ItemType.Tool, new Rectangle(1 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize));
        public static readonly ItemConfig PumpkinSeeds = new ItemConfig("PumpkinSeeds", ItemType.Plantable, new Rectangle(2 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize));
        public static readonly ItemConfig PotatoSeeds = new ItemConfig("PotatoSeeds", ItemType.Plantable, new Rectangle(3 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize));
        public static readonly ItemConfig Pumpkin = new ItemConfig("Pumpkin", ItemType.Crop, new Rectangle(4 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize));
        public static readonly ItemConfig Potato = new ItemConfig("Potato", ItemType.Crop, new Rectangle(5 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize));
        // public static readonly ItemConfig Empty = new ItemConfig("Empty", ItemType.Empty, new Rectangle(0, 0, 0, 0));
    }

    public static class Crops
    {
        public const int DefaultSpriteSize = 48;
        public const int DefaultStages = 4;

        public static readonly CropConfig Pumpkin = new CropConfig("Pumpkin", DefaultStages, new Rectangle(0, 0 * DefaultSpriteSize, DefaultSpriteSize, DefaultSpriteSize));
        public static readonly CropConfig Potato = new CropConfig("Potato", DefaultStages, new Rectangle(0, 1 * DefaultSpriteSize, DefaultSpriteSize, DefaultSpriteSize));
    }

    public static class SeedCropMapping
    {
        public static readonly Dictionary<string, CropConfig> SeedNameToCrop = new()
        {
            { Items.PumpkinSeeds.Name, Crops.Pumpkin },
            { Items.PotatoSeeds.Name, Crops.Potato }
        };

        public static readonly Dictionary<string, ItemConfig> PlantedCropNameToCrop = new()
        {
            { Crops.Pumpkin.Name, Items.Pumpkin },
            { Crops.Potato.Name, Items.Potato }
        };
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
