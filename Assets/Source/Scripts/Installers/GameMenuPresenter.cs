using System;
using Zenject;

public class GameMenuPresenter : Presenter<GameMenuView>, IInitializable, IDisposable
{
    private readonly GameMapLoaderPresenter _gameMapLoaderPresenter;

    public GameMenuPresenter(GameMenuView view, GameMapLoaderPresenter gameMapLoaderPresenter) : base(view)
    {
        _gameMapLoaderPresenter = gameMapLoaderPresenter;
    }

    private void RestartButtonClickedHandler()
    {
        View.PlayPulse(View.RestartGameButton.transform);
        _gameMapLoaderPresenter.LoadCurrentLevel();
    }

    private void NextLevelButtonClickedHandler()
    {
        View.PlayPulse(View.NextGameButton.transform);
        _gameMapLoaderPresenter.LoadNextLevel();
    }


    public void Initialize()
    {
        View.RestartGameButton.onClick.AddListener(RestartButtonClickedHandler);
        View.NextGameButton.onClick.AddListener(NextLevelButtonClickedHandler);
    }

    public void Dispose()
    {
        View.RestartGameButton.onClick.RemoveListener(RestartButtonClickedHandler);
        View.NextGameButton.onClick.RemoveListener(NextLevelButtonClickedHandler);
    }
}