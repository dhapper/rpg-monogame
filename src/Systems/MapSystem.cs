using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using System;

public class MapSystem
{
    // int[,] map_data = new int[,]
    // {
    //     {2, 3, 4, 5, 6, 0, 1, 8, 8, 8},
    //     {1, 1, 1, 1, 1, 0, 1, 8, 8, 8},
    //     {1, 1, 1, 1, 1, 1, 1, 8, 8, 8},
    //     {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    //     {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    //     {1, 1, 1, 1, 1, 0, 0, 1, 1, 1},
    //     {1, 1, 1, 1, 1, 0, 0, 1, 0, 1},
    //     {1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
    //     {1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
    //     {1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
    // };

    List<Entity> map = new List<Entity>();

    private EntityManager _entityManager;
    private AssetStore _assetStore;

    public MapSystem(EntityManager entityManager, AssetStore assetStore)
    {
        _entityManager = entityManager;
        _assetStore = assetStore;

        InitMap();
    }

    public void InitMap()
    {
        // Load JSON string from file, make path relative
        string jsonPath = @"C:\Users\Dhaarshan. P\Desktop\main\Coding\VS projects\C#\MonoGame\rpg\src\Data\map.json";
        string jsonText = File.ReadAllText(jsonPath);

        // Deserialize into a list of list of TileData
        var tileMap = JsonConvert.DeserializeObject<List<List<TileData>>>(jsonText);

        // int sheetWidth = _assetStore.TileSheet.Width;
        // int size = Constants.TileSize;
        // float scale = Constants.ScaleFactor;

        int rows = tileMap.Count;
        int cols = tileMap[0].Count;

        TileData[,] mapData = new TileData[rows, cols];

        // convert to 2d array
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                mapData[r, c] = tileMap[r][c];
            }
        }

        // init tiles
        for (int row = 0; row < mapData.GetLength(0); row++)
        {
            for (int col = 0; col < mapData.GetLength(1); col++)
            {
                int id = mapData[row, col].Id;
                int type = mapData[row, col].Type;
                int? background = mapData[row, col].Background;

                // int sheetWidth = _assetStore.TileSheet.Width;
                int size = Constants.TileSize;
                float scale = Constants.ScaleFactor;

                Entity Tile = _entityManager.CreateEntity();

                Tile.AddComponent(new PositionComponent(col * size * scale, row * size * scale, size, size));
                // int tilesPerRow = sheetWidth / size;
                // int x = id % tilesPerRow * size;
                // int y = id / tilesPerRow * size;

                Tile.AddComponent(new TileComponent());

                var sheet = _assetStore.TileSheet;
                switch (type)
                {
                    case Constants.Tile.WalkableSheetIndex:
                        sheet = _assetStore.GroundTiles;
                        break;
                    case Constants.Tile.CollisionSheetIndex:
                        sheet = _assetStore.CollisionTiles;
                        break;
                    case Constants.Tile.AnimationCollisionSheetIndex:
                        sheet = _assetStore.AnimatedTiles;
                        // x = 0;
                        // y = id * size;
                        break;
                }

                int sheetWidth = sheet.Width;
                int tilesPerRow = sheetWidth / size;
                int x = id % tilesPerRow * size;
                int y = id / tilesPerRow * size;
                if (type == Constants.Tile.AnimationCollisionSheetIndex) {
                    x = 0;
                    y = id * size;
                }

                Tile.AddComponent(new SpriteComponent(sheet, new Rectangle(x, y, size, size)));

                if (Constants.Tile.SolidSheets.Contains(type))
                    Tile.AddComponent(new CollisionComponent(Tile.GetComponent<PositionComponent>(), 0, 0, size, size));

                if (type == Constants.Tile.AnimationCollisionSheetIndex
                    && Constants.Tile.tileAnimations.TryGetValue(id, out AnimationConfig animationConfig))
                    Tile.AddComponent(new AnimationComponent(animationConfig));

                if (background.HasValue)
                {
                    sheetWidth = _assetStore.BackgroundTiles.Width;
                    int tilesInRow = sheetWidth / size;
                    int bgRow = background.Value / tilesInRow;
                    int bgCol = background.Value % tilesInRow;
                    int xPos = bgCol * size; // Calculate x position based on column
                    int yPos = bgRow * size; // Calculate y position based on row
                    Tile.GetComponent<TileComponent>().Background = new Rectangle(xPos, yPos, size, size);
                }

            }
        }

        // for (int row = 0; row < map_data.GetLength(0); row++)
        // {
        //     for (int col = 0; col < map_data.GetLength(1); col++)
        //     {
        //         int id = map_data[row, col];
        //         Entity Tile = _entityManager.CreateEntity();

        //         Tile.AddComponent(new TileComponent());
        //         Tile.AddComponent(new PositionComponent(col * size * scale, row * size * scale, size, size));
        //         int tilesPerRow = sheetWidth / size;
        //         int x = id % tilesPerRow * size;
        //         int y = id / tilesPerRow * size;
        //         Tile.AddComponent(new SpriteComponent(_assetStore.TileSheet, new Rectangle(x, y, size, size)));

        //         // // add hitbox
        //         // if (Constants.Tile.defaultHitbox.Contains(id))
        //         //     Tile.AddComponent(new CollisionComponent(Tile.GetComponent<PositionComponent>(), 0, 0, size, size));
        //         // if(Constants.Tile.insetHitbox.Contains(id))
        //         //     Tile.AddComponent(new CollisionComponent(Tile.GetComponent<PositionComponent>(), 1, 1, size-2, size-2));

        //         // // add animations
        //         // if (Constants.Tile.tileAnimations.ContainsKey(id))
        //         //     Tile.AddComponent(new AnimationComponent(Constants.Tile.tileAnimations[id]));

        //         if (Constants.Tile.)

        //             map.Add(Tile);
        //     }
        // }
    }

    public class TileData
    {
        public int Type { get; set; }
        public int Id { get; set; }
        public int? Background { get; set; }

        public TileData(int type, int id, int? background = null)
        {
            Type = type;
            Id = id;
            Background = background;
        }
    }
}
