using System;
using System.Drawing;
using Microsoft.Xna.Framework;

public class ZoneSystem
{

    private MapSystem _mapSystem;
    private EntityManager _entityManager;
    private SleepSystem _sleepSystem;
    // private GameStateManager _gameStateManager;

    public ZoneSystem(MapSystem mapSystem, EntityManager entityManager, SleepSystem sleepSystem)
    {
        _mapSystem = mapSystem;
        _entityManager = entityManager;
        _sleepSystem = sleepSystem;

        LoadTentZones();
        LoadShopZones();

        _entityManager.RefreshZones();
    }

    public void LoadShopZones()
    {
        var matchingPositions = _mapSystem.MatchingTiles(Constants.Tile.CollisionSheetIndex, 25);

        foreach (var pos in matchingPositions)
        {
            Action action = delegate ()
            {
                GameStateManager.SetState(GameState.Shop);
            };

            Entity zone = _entityManager.CreateEntity();
            zone.AddComponent(new ZoneComponent(ZoneType.Tent, action));
            zone.AddComponent(new PositionComponent(pos.col * Constants.TileSize, pos.row * Constants.TileSize + Constants.TileSize));
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

    public void LoadTentZones()
    {
        var matchingPositions = _mapSystem.MatchingTiles(Constants.Tile.CollisionSheetIndex, 22);

        foreach (var pos in matchingPositions)
        {

            Action action = delegate ()
            {
                GameStateManager.SetState(GameState.DialogueBox);
                // _sleepSystem.StartSleepCycle();
            };

            Entity zone = _entityManager.CreateEntity();
            zone.AddComponent(new ZoneComponent(ZoneType.Tent, action));
            zone.AddComponent(new PositionComponent(pos.col * Constants.TileSize + Constants.TileSize / 2, pos.row * Constants.TileSize + Constants.TileSize));
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

public enum ZoneType
{
    Tent,
    Shop,
}