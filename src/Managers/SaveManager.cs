using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class SaveManager
{

    private static string _baseDir = AppDomain.CurrentDomain.BaseDirectory;
    private static string _jsonPath = Path.Combine(_baseDir, "Data", "SaveFile.json");

    private static string ReadJsonFromFile()
    {
        return File.ReadAllText(_jsonPath);
    }

    public static void SaveEntities(int locationIndex, EntityManager entityManager, string filePath)
    {
        
        var crops = entityManager.CropEntities;
        var entities = new List<EntitySaveData>();

        foreach (var crop in crops)
        {
            var cropComponent = crop.GetComponent<CropComponent>();
            var cropCompConfig = cropComponent.config;
            var posComponent = crop.GetComponent<PositionComponent>();

            var cropData = new CropData
            {
                Name = cropCompConfig.Name,
                Stage = cropCompConfig.CurrentStage,
                Row = posComponent.Row,
                Col = posComponent.Col
            };

            var entityData = new EntitySaveData
            {
                EntityType = "crop",
                EntityInfo = cropData
            };

            entities.Add(entityData);
        }

        var wrapper = new EntitySaveWrapper
        {
            LocationIndex = locationIndex,
            Entities = entities
        };

        var json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
        Console.WriteLine("Crop count: " + crops.Count);
        Console.WriteLine("Saving entities to " + filePath);
        File.WriteAllText(filePath, json);
    }


    public static void SaveMap(string filename, EntityManager entityManager)
    {
        var tileEntities = entityManager.TileEntities;

        // Get bounds
        int maxRow = 0;
        int maxCol = 0;

        foreach (var entity in tileEntities)
        {
            var tile = entity.GetComponent<TileComponent>();
            maxRow = Math.Max(maxRow, tile.Row);
            maxCol = Math.Max(maxCol, tile.Col);
        }

        // Create 2D map structure
        var mapData = new List<List<MapSystem.TileData>>();

        for (int r = 0; r <= maxRow; r++)
        {
            var rowList = new List<MapSystem.TileData>();
            for (int c = 0; c <= maxCol; c++)
            {
                rowList.Add(null); // placeholder
            }
            mapData.Add(rowList);
        }

        // Fill in the tile data
        foreach (var entity in tileEntities)
        {
            var tileComp = entity.GetComponent<TileComponent>();
            var row = tileComp.Row;
            var col = tileComp.Col;

            var tileData = new MapSystem.TileData(tileComp.Type, tileComp.Id);

            if (tileComp.BackgroundId.HasValue)
            {
                tileData.Background = tileComp.BackgroundId;
            }

            mapData[row][col] = tileData;
        }

        // Convert nulls to default tile
        for (int r = 0; r < mapData.Count; r++)
        {
            for (int c = 0; c < mapData[r].Count; c++)
            {
                if (mapData[r][c] == null)
                    mapData[r][c] = new MapSystem.TileData("Tileset1", 0);
            }
        }

        // Serialize and write to file
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", filename);
        string json = JsonConvert.SerializeObject(mapData, Formatting.Indented);
        File.WriteAllText(fullPath, json);

        Console.WriteLine($"Map saved to {filename}");
    }


    public static void SaveData(Entity playerEntity)
    {
        var position = playerEntity.GetComponent<PositionComponent>();
        float x = position.X;
        float y = position.Y;

        var jsonText = ReadJsonFromFile();

        var jsonArray = JsonConvert.DeserializeObject<List<Dictionary<string, Dictionary<string, float>>>>(jsonText);

        if (jsonArray != null && jsonArray.Count > 0)
        {
            var firstObject = jsonArray[0];
            if (firstObject.ContainsKey("Position"))
            {
                var positionDict = firstObject["Position"];
                positionDict["x"] = x;
                positionDict["y"] = y;
            }

            string updatedJson = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
            File.WriteAllText(_jsonPath, updatedJson);
        }
    }



    public static (float x, float y) LoadData()
    {
        var jsonText = ReadJsonFromFile();

        var jsonArray = JsonConvert.DeserializeObject<List<Dictionary<string, Dictionary<string, float>>>>(jsonText);

        if (jsonArray != null && jsonArray.Count > 0)
        {
            var firstObject = jsonArray[0];
            if (firstObject.ContainsKey("Position"))
            {
                var positionDict = firstObject["Position"];

                float x = 0f;
                float y = 0f;

                if (positionDict.ContainsKey("x"))
                    x = positionDict["x"];
                if (positionDict.ContainsKey("y"))
                    y = positionDict["y"];

                return (x, y);
            }
        }


        return (200f, 200f);
    }

    public static void SaveTileData(int x, int y, int type, int id, int background = -1)
    {
        var jsonText = ReadJsonFromFile();

        var mapData = JsonConvert.DeserializeObject<List<List<Dictionary<string, object>>>>(jsonText);

        if (mapData == null || y >= mapData.Count || x >= mapData[y].Count)
        {
            Console.WriteLine("Invalid coordinates");
            return;
        }

        // Get the specific cell
        var cell = mapData[y][x];

        // Update properties
        cell["type"] = type;
        cell["id"] = id;

        if (background != -1)
            cell["background"] = background;

        // Save back to JSON
        string updatedJson = JsonConvert.SerializeObject(mapData, Formatting.Indented);
        File.WriteAllText(_jsonPath, updatedJson);
    }
}