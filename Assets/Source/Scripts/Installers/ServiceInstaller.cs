using Zenject;

public class ServiceInstaller : MonoInstaller<ServiceInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<MapController>().AsSingle();
        Container.Bind<GameMapLoader>().AsSingle();
        Container.Bind<GameplayStatePersistence>().AsSingle();
        Container.Bind<SaveLoadService>().AsSingle();
        Container.BindInterfacesAndSelfTo<MapSaveService>().AsSingle();
    }
}