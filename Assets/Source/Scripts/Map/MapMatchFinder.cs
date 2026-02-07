using System;
using System.Collections.Generic;
using UnityEngine;

//взял готовое решение в однои из пет проектов и добавил чуть обновленную логику. 
public class MapMatchFinder
{
    private const int MinMatchLength = 3;
    private const int NeighborsCount = 4;
    private readonly (int dx, int dy)[] Neighbors = { (0, 1), (1, 0), (0, -1), (-1, 0) };

    public HashSet<MapBlock> FindMatches(int width, int height, Func<int, int, MapBlock> getBlock)
    {
        var lineMatches = new HashSet<MapBlock>();
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                CollectMatch(x, y, width, height, 1, 0, getBlock, lineMatches);
                CollectMatch(x, y, width, height, 0, 1, getBlock, lineMatches);
            }
        }

        return ExpandToConnectedRegions(lineMatches, width, height, getBlock);
    }

    //Собирает всю связную область одного типа для каждого блока из матча (по соседям того же типа)
    private HashSet<MapBlock> ExpandToConnectedRegions(HashSet<MapBlock> lineMatches, int width, int height,
        Func<int, int, MapBlock> getBlock)
    {
        var result = new HashSet<MapBlock>();
        var processed = new HashSet<MapBlock>();

        foreach (var start in lineMatches)
        {
            if (processed.Contains(start))
                continue;

            var blockType = start.BlockType;
            var queue = new Queue<MapBlock>();
            queue.Enqueue(start);
            processed.Add(start);
            result.Add(start);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var position = current.GridPosition;

                for (var i = 0; i < NeighborsCount; i++)
                {
                    var nextX = position.x + Neighbors[i].dx;
                    var nextY = position.y + Neighbors[i].dy;
                    if (IsInside(nextX, nextY, width, height) == false)
                    {
                        continue;
                    }

                    var neighbor = getBlock(nextX, nextY);
                    if (neighbor == null || neighbor.BlockType != blockType || processed.Contains(neighbor))
                    {
                        continue;
                    }

                    processed.Add(neighbor);
                    result.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return result;
    }

    private void CollectMatch(int startX, int startY, int width, int height, int dx, int dy,
        Func<int, int, MapBlock> getBlock, HashSet<MapBlock> matched)
    {
        var startBlock = getBlock(startX, startY);
        if (startBlock == null || startBlock.BlockType == GameBlockType.None)
        {
            return;
        }

        var prevX = startX - dx;
        var prevY = startY - dy;

        if (IsInside(prevX, prevY, width, height) &&
            getBlock(prevX, prevY)?.BlockType == startBlock.BlockType)
        {
            return;
        }

        var count = 0;
        var x = startX;
        var y = startY;

        while (IsInside(x, y, width, height) && getBlock(x, y)?.BlockType == startBlock.BlockType)
        {
            count++;
            x += dx;
            y += dy;
        }

        if (count < MinMatchLength)
        {
            return;
        }

        x = startX;
        y = startY;

        for (var i = 0; i < count; i++)
        {
            matched.Add(getBlock(x, y));
            x += dx;
            y += dy;
        }
    }

    private bool IsInside(int x, int y, int width, int height)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }
}