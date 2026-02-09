using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGrid
{
    private Vector3 _origin;
    private Vector3 _interval;
    private MapBlock[,] _blocks;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public void Init(int width, int height, Vector3 origin, Vector3 interval)
    {
        Width = width;
        Height = height;
        _origin = origin;
        _interval = interval;
        _blocks = new MapBlock[Width, Height];
    }

    public void RegisterBlock(int x, int y, MapBlock block, string blockType)
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
        return new Vector3(positionX, positionY, _origin.z);
    }

    public MapBlock GetBlock(int x, int y)
    {
        return IsInBounds(x, y) == false ? null : _blocks[x, y];
    }

    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public bool IsEmpty()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                if (_blocks[x, y] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    //True, если какой-то тип блоков представлен 2 или менее блоками (невозможно собрать матч из 3)
    //Вроде как стандартная практика в матч играх, но не уверен поэтому TODO
    public bool HasAnyTypeWithCountTwoOrLess()
    {
        var countByType = new Dictionary<string, int>();
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var block = _blocks[x, y];
                if (block == null || string.IsNullOrEmpty(block.BlockType))
                {
                    continue;
                }
                var type = block.BlockType;
                countByType[type] = countByType.TryGetValue(type, out var c) ? c + 1 : 1;
            }
        }
        return countByType.Values.Any(count => count <= 2);
    }

    public void SetBlockAt(int x, int y, MapBlock block)
    {
        _blocks[x, y] = block;
        if (block == null)
        {
            return;
        }
        block.SetGridPosition(x, y);
        block.SetOrder((Height - 1 - y) * Width + x);
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
                if (block == null)
                {
                    continue;
                }
                columnBlocks.Add(block);
                _blocks[x, y] = null;
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

    public void RemoveBlocksAndReturnPool(IEnumerable<MapBlock> blocks)
    {
        foreach (var block in blocks)
        {
            if (block == null)
            {
                continue;
            }

            var position = block.GridPosition;
            _blocks[position.x, position.y] = null;
            block.ReturnToPool();
        }
    }

    public string[] GetSnapshotGrid()
    {
        var result = new string[Width * Height];
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var block = _blocks[x, y];
                result[y * Width + x] = block == null ? "" : block.BlockType;
            }
        }
        return result;
    }
}