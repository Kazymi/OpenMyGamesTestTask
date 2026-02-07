using System;
using Zenject;

public class GameMenuPresenter : Presenter<GameMenuView>, IInitializable, IDisposable
{
    private readonly GameMapLoaderPresenter _gameMapLoaderPresenter;

    public GameMenuPresenter(GameMenuView view, GameMapLoaderPresenter gameMapLoaderPresenter) : base(view)
    {
        _gameMapLoaderPresenter = gameMapLoaderPresenter;
    }

    public void Initialize()
    {
        View.RestartGameButton.onClick.AddListener(OnRestartClicked);
        View.NextGameButton.onClick.AddListener(OnNextLevelClicked);
    }

    public void Dispose()
    {
        View.RestartGameButton.onClick.RemoveListener(OnRestartClicked);
        View.NextGameButton.onClick.RemoveListener(OnNextLevelClicked);
    }

    private void OnRestartClicked()
    {
        View.PlayPulse(View.RestartGameButton.transform);
        _gameMapLoaderPresenter.LoadCurrentLevel();
    }

    private void OnNextLevelClicked()
    {
        View.PlayPulse(View.NextGameButton.transform);
        _gameMapLoaderPresenter.LoadNextLevel();
    }
}
