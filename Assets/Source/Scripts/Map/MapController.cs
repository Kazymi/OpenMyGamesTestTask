using UnityEngine;

public class MapController
{
    private MapGrid _grid;
    private MapLogic _logic;

    public void Init(int width, int height, Vector3 origin, Vector3 interval, float moveDuration)
    {
        _grid = new MapGrid();
        _grid.Init(width, height, origin, interval);
        var matchFinder = new MapMatchFinder();
        var animator = new MapAnimator(_grid, moveDuration);
        _logic = new MapLogic(_grid, matchFinder, animator);
    }

    public void RegisterBlock(int x, int y, MapBlock block, GameBlockType blockType)
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
}
