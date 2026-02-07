using System;
using System.Collections.Generic;
using UnityEngine;


//Жизненный цикл

//Swipe
//Gravity => Matches => Gravity => Matches и до бесконечности
//Finish chain
//Save
public class MapLogic
{
    private readonly MapGrid _grid;
    private readonly MapMatchFinder _matchFinder;
    private readonly MapAnimator _animator;

    private readonly Action _onLevelCleared;
    private readonly Action _onLevelFailed;
    private readonly Action _onTurnCompleted;

    private readonly HashSet<MapBlock> _blocksInAnimation = new();
    private readonly List<MapBlock> _swipeBlocks = new();

    private bool _isChainProcessing;
    private bool _isFailed;
    private Action _pendingChainAction;

    public MapLogic(MapGrid grid, MapMatchFinder matchFinder, MapAnimator animator,
        Action onLevelCleared = null, Action onLevelFailed = null, Action onTurnCompleted = null)
    {
        _grid = grid;
        _matchFinder = matchFinder;
        _animator = animator;

        _onLevelCleared = onLevelCleared;
        _onLevelFailed = onLevelFailed;
        _onTurnCompleted = onTurnCompleted;
    }

    public bool IsBlockLocked(MapBlock block)
    {
        return _isFailed || _blocksInAnimation.Contains(block);
    }

    public bool TrySwipe(SwipeableMapBlock block, Vector2Int direction)
    {
        if (IsBlockLocked(block))
        {
            return false;
        }

        var from = block.GridPosition;
        var to = from + direction;

        if (CanSwipe(from, to) == false)
        {
            return false;
        }

        PrepareSwipeBlocks(from, to);
        StartSwipeAnimation(from, to);
        return true;
    }

    private bool CanSwipe(Vector2Int from, Vector2Int to)
    {
        if (_grid.IsInBounds(to.x, to.y) == false)
        {
            return false;
        }

        var movingUp = from.y > to.y;
        return movingUp == false || _grid.GetBlock(to.x, to.y) != null;
    }

    private void PrepareSwipeBlocks(Vector2Int from, Vector2Int to)
    {
        _swipeBlocks.Clear();
        var fromBlock = _grid.GetBlock(from.x, from.y);
        var toBlock = _grid.GetBlock(to.x, to.y);

        _swipeBlocks.Add(fromBlock);
        if (toBlock != null)
        {
            _swipeBlocks.Add(toBlock);
        }
        AddAnimatingBlocks(_swipeBlocks);
    }

    private void StartSwipeAnimation(Vector2Int from, Vector2Int to)
    {
        var hasTargetBlock = _grid.GetBlock(to.x, to.y) != null;
        if (hasTargetBlock)
        {
            _animator.SwapAndAnimate(from, to, OnSwipeAnimationFinished);
        }
        else
        {
            _animator.MoveAndAnimate(from, to, OnSwipeAnimationFinished);
        }
    }

    private void OnSwipeAnimationFinished()
    {
        RemoveAnimatingBlocks(_swipeBlocks);
        CheckQueueChain(RunGravityAfterSwipe);
    }

    private void CheckQueueChain(Action chainAction)
    {
        if (_isChainProcessing)
        {
            _pendingChainAction = chainAction;
            return;
        }

        _isChainProcessing = true;
        chainAction();
    }

    private void FinishChain()
    {
        _isChainProcessing = false;

        if (_pendingChainAction != null)
        {
            var action = _pendingChainAction;
            _pendingChainAction = null;

            _isChainProcessing = true;
            action();
            return;
        }

        CheckLevelState();
    }

    private void RunGravityAfterSwipe()
    {
        ApplyGravity(OnBlocksLandedAfterSwipe);
    }

    private void RunGravityAfterMatch()
    {
        ApplyGravity(OnBlocksLandedAfterMatch);
    }

    private void ApplyGravity(Action onComplete)
    {
        List<(MapBlock block, int x, int y)> gravityMoves = null;
        _animator.ApplyGravity(
            moves =>
            {
                gravityMoves = moves;
                foreach (var move in moves)
                {
                    AddAnimatingBlock(move.block);
                }
            },
            () =>
            {
                if (gravityMoves != null)
                {
                    foreach (var move in gravityMoves)
                    {
                        RemoveAnimatingBlock(move.block);
                    }
                }
                onComplete();
            });
    }

    private void OnBlocksLandedAfterSwipe()
    {
        ProcessMatches(RunGravityAfterMatchOrFinish);
    }

    private void OnBlocksLandedAfterMatch()
    {
        ProcessMatches(RunGravityAfterMatchOrFinish);
    }

    private void RunGravityAfterMatchOrFinish(int removedCount)
    {
        if (removedCount > 0)
        {
            RunGravityAfterMatch();
            return;
        }

        FinishChain();
    }

    private void ProcessMatches(Action<int> onComplete)
    {
        var matches = _matchFinder.FindMatches(_grid.Width, _grid.Height, _grid.GetBlock);
        if (matches.Count == 0)
        {
            onComplete(0);
            return;
        }

        var matchedBlocks = new List<MapBlock>(matches);
        AddAnimatingBlocks(matchedBlocks);
        _animator.AnimateMatchReturnPool(matchedBlocks, destroyedCount =>
        {
            _grid.RemoveBlocksAndReturnPool(matchedBlocks);
            RemoveAnimatingBlocks(matchedBlocks);
            onComplete(destroyedCount);
        });
    }

    private void CheckLevelState()
    {
        if (_grid.IsEmpty())
        {
            _onLevelCleared?.Invoke();
            return;
        }

        if (_grid.HasAnyTypeWithCountTwoOrLess() == false)
        {
            _onTurnCompleted?.Invoke();
            return;
        }

        _isFailed = true;
        _onLevelFailed?.Invoke();
    }

    private void AddAnimatingBlocks(IEnumerable<MapBlock> blocks)
    {
        foreach (var block in blocks)
        {
            AddAnimatingBlock(block);
        }
    }

    private void RemoveAnimatingBlocks(IEnumerable<MapBlock> blocks)
    {
        foreach (var block in blocks)
        {
            RemoveAnimatingBlock(block);
        }
    }

    private void AddAnimatingBlock(MapBlock block)
    {
        if (block != null)
        {
            _blocksInAnimation.Add(block);
        }
    }

    private void RemoveAnimatingBlock(MapBlock block)
    {
        if (block != null)
        {
            _blocksInAnimation.Remove(block);
        }
    }
}