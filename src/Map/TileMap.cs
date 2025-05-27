public class TileMap
{
    public int[,] Tiles { get; private set; }
    public int TileWidth { get; }
    public int TileHeight { get; }

    public TileMap(int[,] tiles, int tileWidth, int tileHeight)
    {
        Tiles = tiles;
        TileWidth = tileWidth;
        TileHeight = tileHeight;
    }

    public int GetTileId(int x, int y)
    {
        return Tiles[x, y];
    }

    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);
}
