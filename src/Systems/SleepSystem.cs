using System;

public class SleepSystem
{

    private bool sleeping;

    private InteractionSystem _interactionSystem;
    private AssetStore _assets;
    private EntityManager _entityManager;

    public SleepSystem(EntityManager entityManager, AssetStore assets, InteractionSystem interactionSystem)
    {
        _entityManager = entityManager;
        _assets = assets;
        _interactionSystem = interactionSystem;

    }

    public void Update()
    {
        
    }

    public void StartSleepCycle()
    {
        Console.WriteLine("StartSleepCycle");
        _interactionSystem.PlantInteractions.GrowPlants();
        _entityManager.ChangeTiles(Constants.Tile.PathsSheetName, 41, Constants.Tile.PathsSheetName, 40, _assets);
        _entityManager.ChangeTiles(Constants.Tile.PathsSheetName, 49, Constants.Tile.PathsSheetName, 48, _assets);
    }
}