using Zenject;

public class ServiceInstaller : MonoInstaller<ServiceInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<MapGrid>().AsSingle();
        Container.Bind<GameMapLoader>().AsSingle();
    }
}
