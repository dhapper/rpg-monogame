using System;

public class SleepSystem
{

    private bool sleeping;

    private InteractionSystem _interactionSystem;
    private EntityManager _entityManager;

    public SleepSystem(EntityManager entityManager, InteractionSystem interactionSystem)
    {
        _entityManager = entityManager;
        _interactionSystem = interactionSystem;

    }

    public void Update()
    {

    }

    public void StartSleepCycle()
    {
        Console.WriteLine("StartSleepCycle");

        // grow plants
        _interactionSystem.PlantInteractions.GrowPlants();

        // make soil dry overnight
        var sheet = Constants.Tile.PathsSheetName;
        foreach (var tileId in Constants.Tile.WetSoilTiles)
        {
            _entityManager.ChangeTiles(sheet, tileId, sheet, Constants.Tile.OvernightSoilTransform[tileId]);
        }
        _entityManager.RefreshFilteredLists();
    }

    public void EndSleepCycle()
    {
        // _entityManager.RefreshFilteredLists();
    }
}