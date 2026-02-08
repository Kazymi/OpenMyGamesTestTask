using UnityEngine;
using Zenject;

public class GameplayStatePersistence
{
    [Inject] private SaveLoadService _saveLoadService;
    private const string PrefsKey = "GameplayState";

    public void Save(GameplayStateSnapshot snapshot)
    {
        _saveLoadService.Save(snapshot, PrefsKey);
    }

    public GameplayStateSnapshot Load()
    {
       return _saveLoadService.Load<GameplayStateSnapshot>(PrefsKey);
    }

    public void Clear()
    {
        PlayerPrefs.DeleteKey(PrefsKey);
        PlayerPrefs.Save();
    }
}