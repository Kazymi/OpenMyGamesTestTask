using UnityEngine;
using Zenject;

public class GameMapLoaderRunner : MonoBehaviour
{
    private const int LevelIndexWhenNoNextLevel = 9;

    [Header("Scene References")]
    [SerializeField] private Transform _mapStartPositionTransform;

    [Inject] private GameMapLoader _gameMapLoader;
    [Inject] private LevelProvider _levelProvider;
    [Inject] private MapController _mapController;
    [Inject] private FailScreenPresenter _failScreenPresenter;

    private void Start()
    {
        LoadCurrentLevel();
        _mapController.OnLevelCleared += OnLevelCleared;
        _mapController.OnLevelFailed += OnLevelFailed;
    }

    private void OnDestroy()
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
        ClearMap();
        _gameMapLoader.LoadMap(_mapStartPositionTransform);
        return true;
    }

    public void LoadCurrentLevel()
    {
        ClearMap();
        _gameMapLoader.LoadMap(_mapStartPositionTransform);
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
            _gameMapLoader.LoadMap(_mapStartPositionTransform);
        }
        else
        {
            _levelProvider.SetLevelIndex(LevelIndexWhenNoNextLevel);
            LoadCurrentLevel();
        }
    }

    private void ClearMap()
    {
        for (var i = _mapStartPositionTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(_mapStartPositionTransform.GetChild(i).gameObject);
        }
    }
}
