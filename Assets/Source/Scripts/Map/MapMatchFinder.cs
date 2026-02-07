using System;
using System.Collections.Generic;
using UnityEngine;

//Классический MapMatch механника для match игр, взял в одном из своих пет проектов
public class MapMatchFinder
{
    private const int MinMatchLength = 3;

    public HashSet<MapBlock> FindMatches(int width, int height, Func<int, int, MapBlock> getBlock)
    {
        var matched = new HashSet<MapBlock>();

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                CollectMatch(x, y, width, height, 1, 0, getBlock, matched); // horizontal
                CollectMatch(x, y, width, height, 0, 1, getBlock, matched); // vertical
            }
        }

        return matched;
    }

    private void CollectMatch(int startX, int startY, int width, int height, int dx, int dy, Func<int, int, MapBlock> getBlock, HashSet<MapBlock> matched)
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
