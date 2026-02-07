using System;
using UnityEngine;
using Zenject;

public class GameMapLoaderPresenter : Presenter<GameMapLoaderView>, IInitializable, IDisposable
{
    private const int LevelIndexAfterEndOfList = 0;

    private readonly GameMapLoader _gameMapLoader;
    private readonly LevelProvider _levelProvider;
    private readonly MapController _mapController;
    private readonly FailScreenPresenter _failScreenPresenter;
    private readonly LoadSaveService _loadSaveService;

    public GameMapLoaderPresenter(GameMapLoaderView view, GameMapLoader gameMapLoader, LevelProvider levelProvider,
        MapController mapController, FailScreenPresenter failScreenPresenter, LoadSaveService loadSaveService) : base(view)
    {
        _gameMapLoader = gameMapLoader;
        _levelProvider = levelProvider;
        _mapController = mapController;
        _failScreenPresenter = failScreenPresenter;
        _loadSaveService = loadSaveService;
    }

    public void Initialize()
    {
        var savedState = _loadSaveService.TryGetSavedState();
        if (savedState != null)
        {
            _levelProvider.SetLevelIndex(savedState.LevelIndex);
            _gameMapLoader.LoadMapFromSnapshot(savedState, View.MapStartPositionTransform);
            _loadSaveService.SaveCurrentState();
        }
        else
        {
            LoadCurrentLevel();
        }
        _mapController.OnLevelCleared += OnLevelCleared;
        _mapController.OnLevelFailed += OnLevelFailed;
    }

    public void Dispose()
    {
        _mapController.OnLevelCleared -= OnLevelCleared;
        _mapController.OnLevelFailed -= OnLevelFailed;
    }

    public bool LoadNextLevel()
    {
        if (_levelProvider.AdvanceToNextLevel() == false)
        {
            return false;
        }
        _gameMapLoader.LoadMap(View.MapStartPositionTransform);
        _loadSaveService.SaveCurrentState();
        return true;
    }

    public void LoadCurrentLevel()
    {
        _gameMapLoader.LoadMap(View.MapStartPositionTransform);
        _loadSaveService.SaveCurrentState();
    }

    private void OnLevelFailed()
    {
        _failScreenPresenter.Show(LoadCurrentLevel);
    }

    private void OnLevelCleared()
    {
        _loadSaveService.ClearSavedState();
        if (_levelProvider.AdvanceToNextLevel())
        {
            _gameMapLoader.LoadMap(View.MapStartPositionTransform);
            _loadSaveService.SaveCurrentState();
        }
        else
        {
            _levelProvider.SetLevelIndex(LevelIndexAfterEndOfList);
            LoadCurrentLevel();
        }
    }
}
