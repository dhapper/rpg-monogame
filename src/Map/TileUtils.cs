using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class TileUtils
{
    public static Tile[] LoadTiles(int tileWidth, int tileHeight, int tilesetWidth, int tilesetHeight)
    {
        List<Tile> tiles = new();

        for (int y = 0; y < tilesetHeight / tileHeight; y++)
        {
            for (int x = 0; x < tilesetWidth / tileWidth; x++)
            {
                Rectangle rect = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);
                tiles.Add(new Tile(rect));
            }
        }

        return tiles.ToArray();
    }

}