using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class TileFactory
{

    private static int size = Constants.DefaultTileSize;
    private static float scale = Constants.ScaleFactor;

    public static Entity CreateTile(int id, string type, int? background, int row, int col, EntityManager entityManager, AssetStore assets)
    {

        var tile = entityManager.CreateEntity();

        // tile.AddComponent(new TileComponent());
        tile.AddComponent(new TileComponent
        {
            Type = type,
            Id = id
        });

        tile.AddComponent(new PositionComponent(col * size * scale, row * size * scale, size, size));

        var sheet = GetTileset(type, assets);

        int sheetWidth = sheet.Width;
        int tilesPerRow = sheetWidth / size;
        int x = id % tilesPerRow * size;
        int y = id / tilesPerRow * size;

        tile.AddComponent(new SpriteComponent(sheet, new Rectangle(x, y, size, size)));

        if (Array.IndexOf(Constants.Tile.SolidTilesets, type) != -1)
            tile.AddComponent(new CollisionComponent(tile.GetComponent<PositionComponent>(), 0, 0, size, size));

        if (background.HasValue)
            SetBackground(sheetWidth, background, tile, assets);

        return tile;
    }

    public static Texture2D GetTileset(string type, AssetStore assets)
    {
        switch (type)
        {
            case "Tileset1":
                return assets.PathTiles;
            case "Tileset2":
                return assets.CollisionTiles;
            case "Tileset3":
                return assets.WaterTiles;
            default:
                return assets.PathTiles;
        }
    }

    public static void SetBackground(int sheetWidth, int? background, Entity tile, AssetStore assets)
    {
        sheetWidth = assets.BackgroundTiles.Width;
        int tilesInRow = sheetWidth / size;
        int bgRow = background.Value / tilesInRow;
        int bgCol = background.Value % tilesInRow;
        int xPos = bgCol * size;
        int yPos = bgRow * size;
        tile.GetComponent<TileComponent>().Background = new Rectangle(xPos, yPos, size, size);
    }
}
