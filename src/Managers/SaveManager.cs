using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class SaveManager
{
    public static void SaveData(Entity playerEntity)
    {
        // var playerEntities = entityManager.EntitiesWithComponent<PlayerComponent>();
        // var playerEntity = playerEntities.FirstOrDefault();
        var position = playerEntity.GetComponent<PositionComponent>();
        float x = position.X;
        float y = position.Y;

        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string jsonPath = Path.Combine(baseDir, "Data", "SaveFile.json");
        string jsonText = File.ReadAllText(jsonPath);

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
            File.WriteAllText(jsonPath, updatedJson);
        }
    }

    public static (float x, float y) LoadData()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string jsonPath = Path.Combine(baseDir, "Data", "SaveFile.json");
        string jsonText = File.ReadAllText(jsonPath);

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
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string jsonPath = Path.Combine(baseDir, "Data", "map.json");

        // Read the existing map data
        string jsonText = File.ReadAllText(jsonPath);
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
        File.WriteAllText(jsonPath, updatedJson);
    }
}