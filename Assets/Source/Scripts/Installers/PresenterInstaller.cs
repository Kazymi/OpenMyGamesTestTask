using UnityEngine;
using Zenject;

public class PresenterInstaller : MonoInstaller<PresenterInstaller>
{
    [SerializeField] private FailPresenterView _failView;

    public override void InstallBindings()
    {
        Container.Bind<FailPresenterView>().FromInstance(_failView).AsSingle();
        //
        Container.Bind<FailScreenPresenter>().AsSingle();
    }
}