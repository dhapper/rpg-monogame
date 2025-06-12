using System;
using System.Drawing;

public class InteractionZone
{

    private MapSystem _mapSystem;
    private EntityManager _entityManager;

    public InteractionZone(MapSystem mapSystem, EntityManager entityManager)
    {
        _mapSystem = mapSystem;
        _entityManager = entityManager;

        LoadTentZones();
        LoadShopZones();
    }

    public void LoadShopZones()
    {
        var matchingPositions = _mapSystem.MatchingTiles(Constants.Tile.CollisionSheetIndex, 25);

        foreach (var pos in matchingPositions)
        {
            // Console.WriteLine(pos.col+" "+pos.row); 
        }
    }

    public void LoadTentZones()
    {
        var matchingPositions = _mapSystem.MatchingTiles(Constants.Tile.CollisionSheetIndex, 22);

        foreach (var pos in matchingPositions)
        {
            // Console.WriteLine(pos.col + " " + pos.row);
            Entity zone = _entityManager.CreateEntity();
            zone.AddComponent(new ZoneComponent());
            zone.AddComponent(new PositionComponent(pos.col * Constants.TileSize + Constants.TileSize/2, pos.row * Constants.TileSize + Constants.TileSize));
            var posComp = zone.GetComponent<PositionComponent>();
            zone.AddComponent(new CollisionComponent(
                posComp,
                0,
                0,
                Constants.DefaultTileSize,
                (int)(2 * Constants.ScaleFactor),
                false
            ));
        }       
    }
}