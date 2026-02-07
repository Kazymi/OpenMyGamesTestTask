using System;
using UnityEngine;

[Serializable]
public class GameplayStateSnapshot
{
    public int LevelIndex;
    public int Width;
    public int Height;
    public int[] Grid;

    public GameplayStateSnapshot() { }

    public GameplayStateSnapshot(int levelIndex, int width, int height, int[] grid)
    {
        LevelIndex = levelIndex;
        Width = width;
        Height = height;
        Grid = grid ?? Array.Empty<int>();
    }

    public GameBlockType GetCell(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return GameBlockType.None;
        var index = y * Width + x;
        return index >= 0 && index < Grid.Length ? (GameBlockType)Grid[index] : GameBlockType.None;
    }
}
