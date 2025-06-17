using static Constants.Location;

public class LocationManager
{

    private EntityManager[] _entityManagers;
    private MapSystem[] _mapSystems;

    public LocationManager()
    {
        _entityManagers = new EntityManager[Locations.Length];
        _mapSystems = new MapSystem[Locations.Length];
        for (int i = 0; i < Locations.Length; i++)
        {
            _entityManagers[i] = new EntityManager();
            _mapSystems[i] = new MapSystem(_entityManagers[i]);
            _mapSystems[i].InitMap(i);
        }
    }

    public EntityManager GetEntityManager(int index)
    {
        return _entityManagers[index];
    }

    public MapSystem GetMapSystem(int index)
    {
        return _mapSystems[index];
    }


}