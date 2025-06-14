using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class MapSystem
{

    private EntityManager _entityManager;
    private Camera2D _camera;

    public int MapWidthInPixels { get; private set; }
    public int MapHeightInPixels { get; private set; }
    TileData[,] mapData;

    public MapSystem(EntityManager entityManager, Camera2D camera, SleepSystem sleepSystem)
    {
        _entityManager = entityManager;
        _camera = camera;

        InitMap();

        ZoneSystem zoneSystem = new ZoneSystem(this, _entityManager, sleepSystem);
    }

    public void InitMap()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string jsonPath = Path.Combine(baseDir, "Data", "shop_tent.json");
        string jsonText = File.ReadAllText(jsonPath);
        var tileMap = JsonConvert.DeserializeObject<List<List<TileData>>>(jsonText);
        int rows = tileMap.Count;
        int cols = tileMap[0].Count;
        mapData = new TileData[rows, cols];

        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                mapData[r, c] = tileMap[r][c];

        for (int row = 0; row < mapData.GetLength(0); row++)
        {
            for (int col = 0; col < mapData.GetLength(1); col++)
            {
                int id = mapData[row, col].Id ?? 1;
                string type = mapData[row, col].Type ?? "Tilesheet1";
                int? background = mapData[row, col].Background;
                Entity Tile = TileFactory.CreateTile(id, type, background, row, col, _entityManager);

            }
        }

        MapWidthInPixels = (int)(mapData.GetLength(1) * Constants.DefaultTileSize * Constants.ScaleFactor);
        MapHeightInPixels = (int)(mapData.GetLength(0) * Constants.DefaultTileSize * Constants.ScaleFactor);
        _camera.SetWorldBounds(MapWidthInPixels, MapHeightInPixels);
    }

    public List<(int row, int col)> MatchingTiles(string type, int id)
    {
        List<(int, int)> matchingPositions = new List<(int, int)>();

        for (int r = 0; r < mapData.GetLength(0); r++)
        {
            for (int c = 0; c < mapData.GetLength(1); c++)
            {
                var tile = mapData[r, c];

                // Check for nulls to avoid exceptions
                if (tile != null)
                {
                    string tileType = tile.Type ?? "";
                    int tileId = tile.Id ?? 1;

                    if (tileType == type && tileId == id)
                    {
                        matchingPositions.Add((r, c));
                    }
                }
            }
        }

        return matchingPositions;
    }

    public class TileData
    {
        public string Type { get; set; }
        public int? Id { get; set; }
        public int? Background { get; set; }

        public TileData(string type, int? id = 1, int? background = null)
        {
            Type = type;
            Id = id;
            Background = background;
        }
    }
}
