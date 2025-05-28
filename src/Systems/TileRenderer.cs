using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TileRenderer
{
    private SpriteBatch _spriteBatch;
    private Texture2D _tileset;
    private Tile[] _tileDefinitions;

    public TileRenderer(SpriteBatch spriteBatch, Texture2D tileset, Tile[] tileDefinitions)
    {
        _spriteBatch = spriteBatch;
        _tileset = tileset;
        _tileDefinitions = tileDefinitions;
    }

    public void Draw(TileMap tileMap)
    {

        for (int y = 0; y < tileMap.Height; y++)
        {
            for (int x = 0; x < tileMap.Width; x++)
            {
                int tileId = tileMap.GetTileId(x, y);
                if (tileId < 0) continue; // Skip empty or invalid

                var tile = _tileDefinitions[tileId];
                var position = new Vector2(x * tileMap.TileWidth * Constants.ScaleFactor, y * tileMap.TileHeight * Constants.ScaleFactor);
                _spriteBatch.Draw(
                    _tileset,
                    position,
                    tile.SourceRect,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    Constants.ScaleFactor,
                    SpriteEffects.None,
                    0f);
            }
        }
    }

    public void DrawHitbox(SpriteBatch spriteBatch, Position position)
    {
        // spriteBatch
    }
}
