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

    public static class Location
    {
        public const int Location1Index = 0;
        public const int Location2Index = 1;

        public const string Location1FileName = "shop_tent.json";
        public const string Location2FileName = "town.json";

        public static readonly int[] Locations = [
            Location1Index,
            Location2Index
        ];

        public static readonly Dictionary<int, string> IndexToFileName = new()
        {
            { Location1Index, Location1FileName },
            { Location2Index, Location2FileName }
        };
    }

    public static class Tile
    {
        public const string PathsSheetName = "Tileset1";
        public const string CollisionSheetIndex = "Tileset2";
        public const string RenderAboveSpritesSheedIndex = "Tileset3";

        public const int DrySoil = 40;
        public const int WetSoil = 41;
        public const int DryFertilizedSoil = 48;
        public const int WetFertilizedSoil = 49;


        public static readonly string[] SolidTilesets = { CollisionSheetIndex };

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
        public const int Bypass = -1;
        public const int DefaultStackLimit = 9;
        public const int DefaultCapacity = 3;

        public static class Name
        {
            public const string WateringCan = "Watering Can";
            public const string Pickaxe = "Pickaxe";

            public const string PumpkinSeeds = "Pumpkin Seeds";
            public const string PotatoSeeds = "Potato Seeds";
            public const string Pumpkin = "Pumpkin";
            public const string Potato = "Potato";

            public const string Juicer = "Juicer";
            public const string JamJar = "Jam Jar";
            public const string PickleJar = "Pickle Jar";
            public const string Keg = "Keg";

            public const string Juice = "Juice";
            public const string FruitJam = "Fruit Jam";
            public const string PickledVeggie = "Pickled Veggie";
            public const string Wine = "Wine";
        }

        public static class Config
        {
            // Tools
            public static readonly ItemConfig WateringCan = new ItemConfig(
                Name.WateringCan, ItemType.Tool, new Rectangle(0 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize), Bypass, DefaultCapacity);
            public static readonly ItemConfig Pickaxe = new ItemConfig(
                Name.Pickaxe, ItemType.Tool, new Rectangle(1 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize));

            // Seeds
            public static readonly ItemConfig PumpkinSeeds = new ItemConfig(
                Name.PumpkinSeeds, ItemType.Plantable, new Rectangle(2 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize), DefaultStackLimit);
            public static readonly ItemConfig PotatoSeeds = new ItemConfig(
                Name.PotatoSeeds, ItemType.Plantable, new Rectangle(3 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize), DefaultStackLimit);

            // Crops
            public static readonly ItemConfig Pumpkin = new ItemConfig(
                Name.Pumpkin, ItemType.Crop, new Rectangle(4 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize), DefaultStackLimit);
            public static readonly ItemConfig Potato = new ItemConfig(
                Name.Potato, ItemType.Crop, new Rectangle(5 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize), DefaultStackLimit);

            // Machines
            public static readonly ItemConfig Juicer = new ItemConfig(
                Name.Juicer, ItemType.Machine, new Rectangle(0 * DefaultTileSize, 1 * DefaultTileSize, DefaultTileSize, DefaultTileSize));
            public static readonly ItemConfig JamJar = new ItemConfig(
                Name.JamJar, ItemType.Machine, new Rectangle(1 * DefaultTileSize, 1 * DefaultTileSize, DefaultTileSize, DefaultTileSize));
            public static readonly ItemConfig PickleJar = new ItemConfig(
                Name.PickleJar, ItemType.Machine, new Rectangle(2 * DefaultTileSize, 1 * DefaultTileSize, DefaultTileSize, DefaultTileSize));
            public static readonly ItemConfig Keg = new ItemConfig(
                Name.Keg, ItemType.Machine, new Rectangle(3 * DefaultTileSize, 1 * DefaultTileSize, DefaultTileSize, DefaultTileSize));

            // Artisan goods
            public static readonly ItemConfig Juice = new ItemConfig(
                Name.Juice, ItemType.Artisan, new Rectangle(0 * DefaultTileSize, 2 * DefaultTileSize, DefaultTileSize, DefaultTileSize), DefaultStackLimit);
            public static readonly ItemConfig FruitJam = new ItemConfig(
                Name.FruitJam, ItemType.Artisan, new Rectangle(1 * DefaultTileSize, 2 * DefaultTileSize, DefaultTileSize, DefaultTileSize), DefaultStackLimit);
            public static readonly ItemConfig PickledVeggie = new ItemConfig(
                Name.PickledVeggie, ItemType.Artisan, new Rectangle(2 * DefaultTileSize, 2 * DefaultTileSize, DefaultTileSize, DefaultTileSize), DefaultStackLimit);
            public static readonly ItemConfig Wine = new ItemConfig(
                Name.Keg, ItemType.Artisan, new Rectangle(3 * DefaultTileSize, 2 * DefaultTileSize, DefaultTileSize, DefaultTileSize), DefaultStackLimit);
        }
    }

    public static class Crops
    {
        public const int DefaultSpriteSize = 48;
        public const int DefaultStages = 4;

        public static readonly CropConfig Pumpkin = new CropConfig(Items.Name.Pumpkin, DefaultStages, new Rectangle(0, 0 * DefaultSpriteSize, DefaultSpriteSize, DefaultSpriteSize));
        public static readonly CropConfig Potato = new CropConfig(Items.Name.Potato, DefaultStages, new Rectangle(0, 1 * DefaultSpriteSize, DefaultSpriteSize, DefaultSpriteSize));

        public static readonly Dictionary<string, CropConfig> NameToConfig = new()
        {
            { Pumpkin.Name, Pumpkin },
            { Potato.Name, Potato }
        };
    }

    public static class Machines
    {
        public static readonly MachineConfig Juicer = new MachineConfig(
            Items.Name.Juicer, new Rectangle(0 * DefaultTileSize, 0 * DefaultTileSize, DefaultTileSize, DefaultTileSize), 1);
        public static readonly MachineConfig JamJar = new MachineConfig(
            Items.Name.JamJar, new Rectangle(0 * DefaultTileSize, 1 * DefaultTileSize, DefaultTileSize, DefaultTileSize), 3);
        public static readonly MachineConfig PickleJar = new MachineConfig(
            Items.Name.PickleJar, new Rectangle(0 * DefaultTileSize, 2 * DefaultTileSize, DefaultTileSize, DefaultTileSize), 5);
        public static readonly MachineConfig Keg = new MachineConfig(
            Items.Name.Keg, new Rectangle(0 * DefaultTileSize, 3 * DefaultTileSize, DefaultTileSize, DefaultTileSize), 7);

        public static readonly Dictionary<string, MachineConfig> NameToConfig = new()
        {
            { Juicer.Name, Juicer },
            { JamJar.Name, JamJar },
            { PickleJar.Name, PickleJar },
            { Keg.Name, Keg },
        };
    }

    public static class Value
    {
        public static readonly Dictionary<string, int> NameToValue = new()
        {
            // Crops
            { Items.Name.Potato,  100},
            { Items.Name.Pumpkin,  300},

            // Seeds
            { Items.Name.PotatoSeeds,  50},
            { Items.Name.PumpkinSeeds,  100},
        };

    }

    public static class SeedCropMapping
    {
        public static readonly Dictionary<string, CropConfig> SeedNameToCrop = new()
        {
            { Items.Name.PumpkinSeeds, Crops.Pumpkin },
            { Items.Name.PotatoSeeds, Crops.Potato }
        };

        public static readonly Dictionary<string, ItemConfig> PlantedCropNameToCrop = new()
        {
            { Items.Name.Pumpkin, Items.Config.Pumpkin },
            { Items.Name.Potato, Items.Config.Potato }
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
