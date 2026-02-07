using UnityEngine;

public class GameplayStatePersistence
{
    private const string PrefsKey = "GameplayState";

    public void Save(GameplayStateSnapshot snapshot)
    {
        if (snapshot == null)
            return;
        var json = JsonUtility.ToJson(snapshot);
        PlayerPrefs.SetString(PrefsKey, json);
        PlayerPrefs.Save();
    }

    public GameplayStateSnapshot Load()
    {
        if (PlayerPrefs.HasKey(PrefsKey) == false)
            return null;
        var json = PlayerPrefs.GetString(PrefsKey);
        return JsonUtility.FromJson<GameplayStateSnapshot>(json);
    }

    public void Clear()
    {
        PlayerPrefs.DeleteKey(PrefsKey);
        PlayerPrefs.Save();
    }
}
