using UnityEngine;

public class SaveLoadService
{
    public void Save<T>(T saveData, string savePath)
    {
        if (saveData == null)
        {
            return;
        }
        var json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(savePath, json);
        PlayerPrefs.Save();
    }

    public T Load<T>(string loadPath, T defaultValue = default)
    {
        if (PlayerPrefs.HasKey(loadPath) == false)
        {
            return defaultValue;
        }
        var json = PlayerPrefs.GetString(loadPath);
        return JsonUtility.FromJson<T>(json);
    }
}