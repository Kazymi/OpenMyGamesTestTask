using UnityEngine;

public class LevelProvider
{
    private readonly LevelListConfiguration _list;
    private int _currentIndex;

    public int CurrentIndex => _currentIndex;
    public int TotalCount => _list?.Count ?? 0;

    public LevelProvider(LevelListConfiguration list)
    {
        _list = list;
    }

    public LevelConfiguration GetCurrentLevel()
    {
        return _list?.GetLevel(_currentIndex);
    }

    public bool HasNextLevel()
    {
        return _list != null && _currentIndex + 1 < _list.Count;
    }

    /// <summary>Переход на следующий уровень. Возвращает true, если переход выполнен.</summary>
    public bool AdvanceToNextLevel()
    {
        if (HasNextLevel() == false)
            return false;
        _currentIndex++;
        return true;
    }

    public void SetLevelIndex(int index)
    {
        if (_list == null || index < 0 || index >= _list.Count)
            return;
        _currentIndex = index;
    }

    public void ResetToFirstLevel()
    {
        _currentIndex = 0;
    }
}
