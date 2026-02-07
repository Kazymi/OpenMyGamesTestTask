using UnityEngine;

[CreateAssetMenu(fileName = "LevelListConfiguration", menuName = "Game/Level List Configuration")]
public class LevelListConfiguration : ScriptableObject
{
    [SerializeField] private LevelConfiguration[] _levels;

    public int Count => _levels?.Length ?? 0;

    public LevelConfiguration GetLevel(int index)
    {
        if (_levels == null || index < 0 || index >= _levels.Length)
            return null;
        return _levels[index];
    }
}
