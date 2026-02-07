using System.Collections.Generic;
using UnityEngine;

public class MapLogic
{
    private readonly MapGrid _grid;
    private readonly MapMatchFinder _matchFinder;
    private readonly MapAnimator _animator;

    public MapLogic(MapGrid grid, MapMatchFinder matchFinder, MapAnimator animator)
    {
        _grid = grid;
        _matchFinder = matchFinder;
        _animator = animator;
    }

    public bool TrySwipe(SwipeableMapBlock block, Vector2Int direction)
    {
        var from = block.GridPosition;
        var to = from + direction;
        var isUp = from.y > to.y;
        if (_grid.IsInBounds(to.x, to.y) == false || (isUp && _grid.GetBlock(to.x, to.y) == null))
        {
            return false;
        }

        if (_grid.GetBlock(to.x, to.y) != null)
        {
            _animator.SwapAndAnimate(from, to, OnSwipeAnimationComplete);
        }
        else
        {
            _animator.MoveAndAnimate(from, to, OnSwipeAnimationComplete);
        }

        return true;
    }

    private void OnSwipeAnimationComplete()
    {
        FindAndProcessMatches(ApplyGravityAfterMatch);
    }

    private void ApplyGravityAfterMatch(int removedCount)
    {
        _animator.ApplyGravity(OnGravityComplete);
    }

    private void OnGravityComplete()
    {
        FindAndProcessMatches(OnMatchesProcessed);
    }

    private void OnMatchesProcessed(int removedCount)
    {
        if (removedCount > 0)
        {
            _animator.ApplyGravity(OnGravityComplete);
        }
    }

    private void FindAndProcessMatches(System.Action<int> onComplete)
    {
        var matched = _matchFinder.FindMatches(_grid.Width, _grid.Height, _grid.GetBlock);
        var count = matched.Count;
        if (count == 0)
        {
            onComplete(0);
            return;
        }

        var listBlocks = new List<MapBlock>(matched);
        _animator.AnimateMatchDestroy(listBlocks, destroyedCount =>
        {
            _grid.RemoveBlocksAndDestroy(listBlocks);
            onComplete(destroyedCount);
        });
    }
}
