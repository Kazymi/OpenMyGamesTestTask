using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MapAnimator
{
    private const float DestroyDelayPerBlock = 0.1f;

    private readonly MapGrid _grid;
    private readonly float _moveDuration;

    public MapAnimator(MapGrid grid, float moveDuration)
    {
        _grid = grid;
        _moveDuration = moveDuration;
    }

    private Tween AnimateTo(MapBlock block, int x, int y, Ease ease = Ease.Linear)
    {
        if (block == null)
        {
            return DOTween.Sequence();
        }
        return block.transform.DOMove(_grid.GetWorldPosition(x, y), _moveDuration).SetEase(ease).SetAutoKill(true)
            .SetTarget(block.gameObject);
    }

    public void SwapAndAnimate(Vector2Int from, Vector2Int to, TweenCallback onComplete)
    {
        var blockA = _grid.GetBlock(from.x, from.y);
        var blockB = _grid.GetBlock(to.x, to.y);
        _grid.SetBlockAt(from.x, from.y, null);
        _grid.SetBlockAt(to.x, to.y, null);
        _grid.SetBlockAt(to.x, to.y, blockA);
        _grid.SetBlockAt(from.x, from.y, blockB);
        var sequence = DOTween.Sequence();
        sequence.Join(AnimateTo(blockA, to.x, to.y));
        sequence.Join(AnimateTo(blockB, from.x, from.y));
        sequence.OnComplete(onComplete);
    }

    public void MoveAndAnimate(Vector2Int from, Vector2Int to, TweenCallback onComplete)
    {
        var block = _grid.GetBlock(from.x, from.y);
        _grid.SetBlockAt(from.x, from.y, null);
        _grid.SetBlockAt(to.x, to.y, block);
        AnimateTo(block, to.x, to.y).OnComplete(onComplete);
    }

    public void ApplyGravity(TweenCallback onComplete)
    {
        var moves = _grid.ApplyGravityAndSetToData();
        var sequence = DOTween.Sequence();
        foreach (var (block, x, y) in moves)
        {
            sequence.Join(AnimateTo(block, x, y, Ease.InCubic));
        }

        if (sequence.Duration() > 0f)
        { 
            sequence.OnComplete(onComplete);
        }
        else
        {
            onComplete?.Invoke();
        }
    }

    public void AnimateMatchReturnPool(List<MapBlock> listBlocks, System.Action<int> onComplete)
    {
        var count = listBlocks.Count;
        var pending = count;
        var raiseSequence = DOTween.Sequence();
        raiseSequence.AppendCallback(() => listBlocks[0].RaiseMatched(TryFinish));
        for (var i = 1; i < listBlocks.Count; i++)
        {
            var block = listBlocks[i];
            raiseSequence.AppendInterval(DestroyDelayPerBlock);
            raiseSequence.AppendCallback(() => block.RaiseMatched(TryFinish));
        }

        void TryFinish()
        {
            pending--;
            if (pending > 0)
            {
                return;
            }

            onComplete(count);
        }
    }
}