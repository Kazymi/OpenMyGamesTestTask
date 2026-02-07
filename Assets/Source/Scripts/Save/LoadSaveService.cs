using System;
using Zenject;

public class LoadSaveService : IInitializable, IDisposable
{
    private readonly GameplayStatePersistence _persistence;
    private readonly MapController _mapController;
    private readonly LevelProvider _levelProvider;

    public LoadSaveService(GameplayStatePersistence persistence, MapController mapController,
        LevelProvider levelProvider)
    {
        _persistence = persistence;
        _mapController = mapController;
        _levelProvider = levelProvider;
    }

    public void Initialize()
    {
        _mapController.OnTurnCompleted += SaveCurrentState;
    }

    public void Dispose()
    {
        if (_mapController == null)
            return;
        _mapController.OnTurnCompleted -= SaveCurrentState;
    }

    public GameplayStateSnapshot TryGetSavedState()
    {
        return _persistence.Load();
    }

    public void ClearSavedState()
    {
        _persistence.Clear();
    }

    public void SaveCurrentState()
    {
        var (width, height, grid) = _mapController.GetGridStateSnapshot();
        if (width == 0 || height == 0 || grid == null)
            return;
        var snapshot = new GameplayStateSnapshot(_levelProvider.CurrentIndex, width, height, grid);
        _persistence.Save(snapshot);
    }
}
