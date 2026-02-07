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

    public GameMapLoaderPresenter(GameMapLoaderView view, GameMapLoader gameMapLoader, LevelProvider levelProvider,
        MapController mapController, FailScreenPresenter failScreenPresenter) : base(view)
    {
        _gameMapLoader = gameMapLoader;
        _levelProvider = levelProvider;
        _mapController = mapController;
        _failScreenPresenter = failScreenPresenter;
    }

    public void Initialize()
    {
        LoadCurrentLevel();
        _mapController.OnLevelCleared += OnLevelCleared;
        _mapController.OnLevelFailed += OnLevelFailed;
    }

    public void Dispose()
    {
        if (_mapController == null)
        {
            return;
        }
        _mapController.OnLevelCleared -= OnLevelCleared;
        _mapController.OnLevelFailed -= OnLevelFailed;
    }
    
    private void OnLevelFailed()
    {
        _failScreenPresenter.Show(LoadCurrentLevel);
    }

    private void OnLevelCleared()
    {
        if (_levelProvider.AdvanceToNextLevel())
        {
            ClearMap();
            _gameMapLoader.LoadMap(View.MapStartPositionTransform);
        }
        else
        {
            _levelProvider.SetLevelIndex(LevelIndexAfterEndOfList);
            LoadCurrentLevel();
        }
    }

    public bool LoadNextLevel()
    {
        if (_levelProvider.AdvanceToNextLevel() == false)
        {
            return false;
        }

        ClearMap();
        _gameMapLoader.LoadMap(View.MapStartPositionTransform);
        return true;
    }

    public void LoadCurrentLevel()
    {
        ClearMap();
        _gameMapLoader.LoadMap(View.MapStartPositionTransform);
    }

    private void ClearMap()
    {
    }
}