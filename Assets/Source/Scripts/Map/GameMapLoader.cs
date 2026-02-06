using DG.Tweening;
using UnityEngine;
using Zenject;

public class GameMapLoader
{
    private const float DropDelayPerCell = 0.03f;

    [Inject] private LevelConfiguration _levelConfiguration;
    [Inject] private LevelBlockConfiguration _blockConfiguration;
    [Inject] private MapGrid _mapGrid;

    public void LoadMap(Transform parent)
    {
        if (_levelConfiguration == null || _blockConfiguration == null)
            return;

        var interval = _blockConfiguration.ObjectInterval;
        var width = _levelConfiguration.Width;
        var height = _levelConfiguration.Height;
        var origin = parent.position;
        var spawnY = origin.y + (height - 1) * interval.y + _blockConfiguration.SpawnHeightAbove;
        var fallDuration = _blockConfiguration.FallDuration;
        _mapGrid.Init(width, height, origin, interval, _blockConfiguration.SwapDuration);
        var cellIndex = 0;

        for (var y = height - 1; y >= 0; y--)
        {
            for (var x = width - 1; x >= 0; x--)
            {
                var blockType = _levelConfiguration.GetCell(x, y);
                if (blockType == GameBlockType.None)
                    continue;
                var prefab = _blockConfiguration.GetPrefab(blockType);
                if (prefab == null)
                    continue;

                var targetPosition = _mapGrid.GetWorldPosition(x, y);
                var spawnPosition = new Vector3(targetPosition.x, spawnY, targetPosition.z);
                var instance = Object.Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
                instance.SetOrder((height - 1 - y) * width + x);
                _mapGrid.RegisterBlock(x, y, instance);
                if (instance is SwipeableMapBlock swipeable)
                    swipeable.Init(_mapGrid);

                var delay = cellIndex * DropDelayPerCell;
                instance.transform.DOMove(targetPosition, fallDuration).SetDelay(delay).SetEase(Ease.OutBounce);
                cellIndex++;
            }
        }
    }
}