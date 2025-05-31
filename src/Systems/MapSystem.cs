using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

public class MapSystem
{
    int[,] map_data = new int[,]
    {
        {2, 3, 4, 5, 6, 0, 1, 8, 8, 8},
        {1, 1, 1, 1, 1, 0, 1, 8, 8, 8},
        {1, 1, 1, 1, 1, 1, 1, 8, 8, 8},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 0, 0, 1, 1, 1},
        {1, 1, 1, 1, 1, 0, 0, 1, 0, 1},
        {1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
    };

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
        int sheetWidth = _assetStore.TileSheet.Width;
        int size = Constants.TileSize;
        float scale = Constants.ScaleFactor;

        for (int row = 0; row < map_data.GetLength(0); row++)
        {
            for (int col = 0; col < map_data.GetLength(1); col++)
            {
                int id = map_data[row, col];
                Entity Tile = _entityManager.CreateEntity();

                Tile.AddComponent(new TileComponent());
                Tile.AddComponent(new PositionComponent(col * size * scale, row * size * scale, size, size));
                int tilesPerRow = sheetWidth / size;
                int x = id % tilesPerRow * size;
                int y = id / tilesPerRow * size;
                Tile.AddComponent(new SpriteComponent(_assetStore.TileSheet, new Rectangle(x, y, size, size)));

                // add hitbox
                if (Constants.Tile.defaultHitbox.Contains(id))
                    Tile.AddComponent(new CollisionComponent(Tile.GetComponent<PositionComponent>(), 0, 0, size, size));
                if(Constants.Tile.insetHitbox.Contains(id))
                    Tile.AddComponent(new CollisionComponent(Tile.GetComponent<PositionComponent>(), 1, 1, size-2, size-2));

                // add animations
                if (Constants.Tile.tileAnimations.ContainsKey(id))
                    Tile.AddComponent(new AnimationComponent(Constants.Tile.tileAnimations[id]));
                
                map.Add(Tile);
            }
        }
    }
}
