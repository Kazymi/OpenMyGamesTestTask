using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MapGrid
{
    private int _width;
    private int _height;
    private Vector3 _origin;
    private Vector3 _interval;
    private float _moveDuration;
    private MapBlock[,] _blocks;

    public void Init(int width, int height, Vector3 origin, Vector3 interval, float moveDuration)
    {
        _width = width;
        _height = height;
        _origin = origin;
        _interval = interval;
        _moveDuration = moveDuration;
        _blocks = new MapBlock[_width, _height];
    }

    public void RegisterBlock(int x, int y, MapBlock block)
    {
        if (IsInBounds(x, y) == false)
            return;
        _blocks[x, y] = block;
        block.SetGridPosition(x, y);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(
            _origin.x + x * _interval.x,
            _origin.y + (_height - 1 - y) * _interval.y,
            _origin.z);
    }

    public bool TrySwipe(SwipeableMapBlock block, Vector2Int direction)
    {
        var from = block.GridPosition;
        var to = from + direction;
        var isUp = from.y > to.y;
        if (IsInBounds(to.x, to.y) == false || (isUp && GetBlock(to.x, to.y) == null))
        {
            return false;
        }

        if (GetBlock(to.x, to.y) != null)
        {
            SwapAndAnimate(from, to, OnSwipeAnimationComplete);
        }
        else
        {
            MoveAndAnimate(from, to, OnSwipeAnimationComplete);
        }

        return true;
    }

    private void OnSwipeAnimationComplete()
    {
        ApplyGravity();
    }

    private MapBlock GetBlock(int x, int y)
    {
        if (IsInBounds(x, y) == false)
            return null;
        return _blocks[x, y];
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < _width && y >= 0 && y < _height;
    }

    private int GetSortingOrder(int x, int y)
    {
        return (_height - 1 - y) * _width + x;
    }

    private void SetBlockAt(int x, int y, MapBlock block)
    {
        _blocks[x, y] = block;
        if (block != null)
        {
            block.SetGridPosition(x, y);
            block.SetOrder(GetSortingOrder(x, y));
        }
    }

    private Tween AnimateTo(MapBlock block, int x, int y)
    {
        return block.transform.DOMove(GetWorldPosition(x, y), _moveDuration);
    }

    private void SwapAndAnimate(Vector2Int from, Vector2Int to, TweenCallback onComplete)
    {
        var blockA = _blocks[from.x, from.y];
        var blockB = _blocks[to.x, to.y];
        SetBlockAt(from.x, from.y, null);
        SetBlockAt(to.x, to.y, null);
        SetBlockAt(to.x, to.y, blockA);
        SetBlockAt(from.x, from.y, blockB);
        var sequence = DOTween.Sequence();
        sequence.Join(AnimateTo(blockA, to.x, to.y));
        sequence.Join(AnimateTo(blockB, from.x, from.y));
        sequence.OnComplete(onComplete);
    }

    private void MoveAndAnimate(Vector2Int from, Vector2Int to, TweenCallback onComplete)
    {
        var block = _blocks[from.x, from.y];
        SetBlockAt(from.x, from.y, null);
        SetBlockAt(to.x, to.y, block);
        AnimateTo(block, to.x, to.y).OnComplete(onComplete);
    }

    private void ApplyGravity()
    {
        for (var x = 0; x < _width; x++)
        {
            var columnBlocks = new List<MapBlock>();
            for (var y = 0; y < _height; y++)
            {
                var block = _blocks[x, y];
                if (block != null)
                {
                    columnBlocks.Add(block);
                    _blocks[x, y] = null;
                }
            }

            for (var i = 0; i < columnBlocks.Count; i++)
            {
                var newY = _height - columnBlocks.Count + i;
                var block = columnBlocks[i];
                SetBlockAt(x, newY, block);
                AnimateTo(block, x, newY);
            }
        }
    }
}