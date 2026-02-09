using System;
using UnityEngine;

[Serializable]
public class GameplayStateSnapshot
{
    public int LevelIndex;
    public int Width;
    public int Height;
    public string[] Grid;

    public GameplayStateSnapshot(int levelIndex, int width, int height, string[] grid)
    {
        LevelIndex = levelIndex;
        Width = width;
        Height = height;
        Grid = grid ?? Array.Empty<string>();
    }

    public string GetCell(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return "";
        var index = y * Width + x;
        return index >= 0 && index < Grid.Length ? Grid[index] ?? "" : "";
    }
}
