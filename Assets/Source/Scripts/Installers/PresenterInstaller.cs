using UnityEngine;
using Zenject;

public class PresenterInstaller : MonoInstaller<PresenterInstaller>
{
    [Header("Views")]
    [SerializeField] private FailView _failView;
    [SerializeField] private GameMenuView _gameMenuView;
    [SerializeField] private GameMapLoaderView _gameMapLoaderView;
    [SerializeField] private BalloonsView _balloonsView;

    //Уточнение!!! Это View-Presenter, люблю его) удобен и легко читаем
    public override void InstallBindings()
    {
        Container.Bind<FailView>().FromInstance(_failView).AsSingle();
        Container.Bind<GameMenuView>().FromInstance(_gameMenuView).AsSingle();
        Container.Bind<GameMapLoaderView>().FromInstance(_gameMapLoaderView).AsSingle();
        Container.Bind<BalloonsView>().FromInstance(_balloonsView).AsSingle();
        //
        Container.Bind<FailScreenPresenter>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameMenuPresenter>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameMapLoaderPresenter>().AsSingle();
        Container.BindInterfacesAndSelfTo<BalloonsPresenter>().AsSingle();
    }
}