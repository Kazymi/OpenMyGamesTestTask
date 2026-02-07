using Zenject;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfigInstaller", menuName = "Game/Level Config Installer")]
public class LevelConfigInstaller : ScriptableObjectInstaller<LevelConfigInstaller>
{
    [SerializeField] private LevelListConfiguration _levelList;
    [SerializeField] private LevelBlockConfiguration _blockConfiguration;

    public override void InstallBindings()
    {
        Container.Bind<LevelListConfiguration>().FromInstance(_levelList).AsSingle();
        Container.Bind<LevelProvider>().AsSingle();
        Container.Bind<LevelBlockConfiguration>().FromInstance(_blockConfiguration).AsSingle();
    }
}