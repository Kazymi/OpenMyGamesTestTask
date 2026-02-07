using Zenject;

public class ServiceInstaller : MonoInstaller<ServiceInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<MapController>().AsSingle();
        Container.Bind<GameMapLoader>().AsSingle();
    }
}
