using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class MapGrid
{
    private Vector3 _origin;
    private Vector3 _interval;
    private MapBlock[,] _blocks;
    
    public int Width { get; private set; }
    public int Height{ get; private set; }
    
    public void Init(int width, int height, Vector3 origin, Vector3 interval)
    {
        Width = width;
        Height = height;
        _origin = origin;
        _interval = interval;
        _blocks = new MapBlock[Width, Height];
    }

    public void RegisterBlock(int x, int y, MapBlock block, GameBlockType blockType)
    {
        if (IsInBounds(x, y) == false)
        {
            return;
        }

        _blocks[x, y] = block;
        block.SetGridPosition(x, y);
        block.SetBlockType(blockType);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        var positionX = _origin.x + x * _interval.x;
        var positionY = _origin.y + (Height - 1 - y) * _interval.y;
        return new Vector3(positionX,positionY, _origin.z);
    }

    public MapBlock GetBlock(int x, int y)
    {
        return IsInBounds(x, y) == false ? null : _blocks[x, y];
    }

    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public void SetBlockAt(int x, int y, MapBlock block)
    {
        _blocks[x, y] = block;
        if (block != null)
        {
            block.SetGridPosition(x, y);
            block.SetOrder((Height - 1 - y) * Width + x);
        }
    }
    
    public List<(MapBlock block, int x, int y)> ApplyGravityAndSetToData()
    {
        var moves = new List<(MapBlock, int, int)>();
        for (var x = 0; x < Width; x++)
        {
            var columnBlocks = new List<MapBlock>();
            for (var y = 0; y < Height; y++)
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
                var newY = Height - columnBlocks.Count + i;
                var block = columnBlocks[i];
                var oldY = block.GridPosition.y;
                SetBlockAt(x, newY, block);
                if (oldY != newY)
                {
                    moves.Add((block, x, newY));
                }
            }
        }

        return moves;
    }

    public void RemoveBlocksAndDestroy(IEnumerable<MapBlock> blocks)
    {
        foreach (var block in blocks)
        {
            if (block == null)
            {
                continue;
            }
            var position = block.GridPosition;
            _blocks[position.x, position.y] = null;
            GameObject.Destroy(block.gameObject);
        }
    }
}