using System;
using UnityEngine;

public class MapController
{
    private MapGrid _grid;
    private MapLogic _logic;

    public event Action OnLevelCleared;
    public event Action OnLevelFailed;
    public event Action OnTurnCompleted;

    public void Init(int width, int height, Vector3 origin, Vector3 interval, float moveDuration)
    {
        //Ужасный монгстр TODO отрефакторить
        ReturnAllBlocksToPool();
        _grid = new MapGrid();
        _grid.Init(width, height, origin, interval);
        var matchFinder = new MapMatchFinder();
        var animator = new MapAnimator(_grid, moveDuration);
        _logic = new MapLogic(_grid, matchFinder, animator,
            () => OnLevelCleared?.Invoke(),
            () => OnLevelFailed?.Invoke(),
            () => OnTurnCompleted?.Invoke());
    }

    public void ReturnAllBlocksToPool()
    {
        if (_grid == null)
        {
            return;
        }

        for (var x = 0; x < _grid.Width; x++)
        {
            for (var y = 0; y < _grid.Height; y++)
            {
                var block = _grid.GetBlock(x, y);
                if (block != null)
                {
                    block.ReturnToPool();
                }
            }
        }
    }

    public void RegisterBlock(int x, int y, MapBlock block, string blockType)
    {
        _grid.RegisterBlock(x, y, block, blockType);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return _grid.GetWorldPosition(x, y);
    }

    public bool TrySwipe(SwipeableMapBlock block, Vector2Int direction)
    {
        return _logic.TrySwipe(block, direction);
    }

    public bool IsBlockLocked(MapBlock block)
    {
        return _logic.IsBlockLocked(block);
    }

    public (int width, int height, string[] grid) GetGridStateSnapshot()
    {
        return _grid == null ? (0, 0, (string[])null) : (_grid.Width, _grid.Height, _grid.GetSnapshotGrid());
    }
}