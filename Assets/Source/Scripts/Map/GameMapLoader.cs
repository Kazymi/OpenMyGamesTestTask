using DG.Tweening;
using UnityEngine;
using Zenject;

public class GameMapLoader
{
    private const float DropDelayPerCell = 0.03f;

    [Inject] private LevelConfiguration _levelConfiguration;
    [Inject] private LevelBlockConfiguration _blockConfiguration;
    [Inject] private MapController _mapController;

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
        _mapController.Init(width, height, origin, interval, _blockConfiguration.SwapDuration);
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

                var targetPosition = _mapController.GetWorldPosition(x, y);
                var spawnPosition = new Vector3(targetPosition.x, spawnY, targetPosition.z);
                var instance = Object.Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
                instance.SetOrder((height - 1 - y) * width + x);
                _mapController.RegisterBlock(x, y, instance, blockType);
                if (instance is SwipeableMapBlock swipeable)
                    swipeable.Init(_mapController);

                var delay = cellIndex * DropDelayPerCell;
                instance.transform.DOMove(targetPosition, fallDuration).SetDelay(delay).SetEase(Ease.OutBounce);
                cellIndex++;
            }
        }
    }
}