using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LevelConfiguration", menuName = "Game/Level Configuration")]
public class LevelConfiguration : ScriptableObject
{
    [SerializeField] private int _width = 4;
    [SerializeField] private int _height = 4;
    [SerializeField] private string[] _grid;

    public int Width => _width;
    public int Height => _height;

    public string GetCell(int x, int y)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height)
        {
            Debug.LogError($"Out of range: {x}, {y}");
            return "";
        }
        var index = y * _width + x;
        if (index >= 0 && index < _grid.Length)
            return _grid[index] ?? "";
        Debug.LogError($"Invalid get position: {x}, {y}");
        return "";
    }

    public void SetCell(int x, int y, string value)
    {
        var index = y * _width + x;
        if (index >= 0 && index < _grid.Length)
        {
            _grid[index] = value;
        }
        else
        {
            Debug.LogError("Can't change cell");
        }
    }

    public void InitGrid(int width, int height)
    {
        this._width = Mathf.Max(1, width);
        _height = Mathf.Max(1, height);
        _grid = new string[this._width * _height];
    }

    public void EnsureGridSize()
    {
        if (_grid == null || _grid.Length != _width * _height)
        {
            _grid = new string[_width * _height];
        }
    }
}