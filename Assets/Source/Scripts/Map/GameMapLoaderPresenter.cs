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
    private readonly MapSaveService _saveService;

    public GameMapLoaderPresenter(GameMapLoaderView view, GameMapLoader gameMapLoader, LevelProvider levelProvider,
        MapController mapController, FailScreenPresenter failScreenPresenter, MapSaveService saveService) : base(view)
    {
        _gameMapLoader = gameMapLoader;
        _levelProvider = levelProvider;
        _mapController = mapController;
        _failScreenPresenter = failScreenPresenter;
        _saveService = saveService;
    }

    public void Initialize()
    {
        var savedState = _saveService.TryGetSavedState();
        if (savedState != null)
        {
            _levelProvider.SetLevelIndex(savedState.LevelIndex);
            _gameMapLoader.LoadMapFromSnapshot(savedState, View.MapStartPositionTransform);
            _saveService.SaveCurrentState();
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

    public void LoadNextLevel()
    {
        if (_levelProvider.AdvanceToNextLevel() == false)
        {
            _levelProvider.ResetToFirstLevel();
        }
        _gameMapLoader.LoadMap(View.MapStartPositionTransform);
        _saveService.SaveCurrentState();
    }

    public void LoadCurrentLevel()
    {
        _gameMapLoader.LoadMap(View.MapStartPositionTransform);
        _saveService.SaveCurrentState();
    }

    private void OnLevelFailed()
    {
        _failScreenPresenter.Show(LoadCurrentLevel);
    }

    private void OnLevelCleared()
    {
        _saveService.ClearSavedState();
        if (_levelProvider.AdvanceToNextLevel())
        {
            _gameMapLoader.LoadMap(View.MapStartPositionTransform);
            _saveService.SaveCurrentState();
        }
        else
        {
            _levelProvider.SetLevelIndex(LevelIndexAfterEndOfList);
            LoadCurrentLevel();
        }
    }
}
