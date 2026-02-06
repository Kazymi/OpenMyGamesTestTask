using Zenject;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfigInstaller", menuName = "Game/Level Config Installer")]
public class LevelConfigInstaller : ScriptableObjectInstaller<LevelConfigInstaller>
{
    [SerializeField] private LevelConfiguration _levelConfiguration;
    [SerializeField] private LevelBlockConfiguration _blockConfiguration;

    public override void InstallBindings()
    {
        Container.Bind<LevelConfiguration>().FromInstance(_levelConfiguration).AsSingle();
        Container.Bind<LevelBlockConfiguration>().FromInstance(_blockConfiguration).AsSingle();
    }
}