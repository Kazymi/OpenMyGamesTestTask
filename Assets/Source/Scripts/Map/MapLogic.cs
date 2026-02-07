using System.Collections.Generic;
using UnityEngine;

public class MapLogic
{
    private readonly MapGrid _grid;
    private readonly MapMatchFinder _matchFinder;
    private readonly MapAnimator _animator;
    private readonly System.Action _onLevelCleared;
    private readonly System.Action _onLevelFailed;
    private bool _isBusy;

    public bool IsInputLocked => _isBusy;

    public MapLogic(MapGrid grid, MapMatchFinder matchFinder, MapAnimator animator,
        System.Action onLevelCleared = null, System.Action onLevelFailed = null)
    {
        _grid = grid;
        _matchFinder = matchFinder;
        _animator = animator;
        _onLevelCleared = onLevelCleared;
        _onLevelFailed = onLevelFailed;
    }

    public bool TrySwipe(SwipeableMapBlock block, Vector2Int direction)
    {
        if (_isBusy)
            return false;

        var from = block.GridPosition;
        var to = from + direction;
        var isUp = from.y > to.y;
        if (_grid.IsInBounds(to.x, to.y) == false || (isUp && _grid.GetBlock(to.x, to.y) == null))
        {
            return false;
        }

        _isBusy = true;
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
        _animator.ApplyGravity(OnLandedAfterSwipe);
    }

    private void OnLandedAfterSwipe()
    {
        FindAndProcessMatches(OnMatchesProcessed);
    }

    private void OnLandedAfterMatch()
    {
        FindAndProcessMatches(OnMatchesProcessed);
    }

    private void OnMatchesProcessed(int removedCount)
    {
        if (removedCount > 0)
        {
            _animator.ApplyGravity(OnLandedAfterMatch);
        }
        else
        {
            _isBusy = false;
            if (_grid.IsEmpty())
                _onLevelCleared?.Invoke();
            else if (_grid.HasAnyTypeWithCountTwoOrLess())
                _onLevelFailed?.Invoke();
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
