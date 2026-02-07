using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GameMapLoader
{
    private const float DropDelayPerCell = 0.03f;

    [Inject] private LevelProvider _levelProvider;
    [Inject] private LevelBlockConfiguration _blockConfiguration;
    [Inject] private MapController _mapController;
    [Inject] private DiContainer _container;

    private readonly Dictionary<GameBlockType, IPool<MapBlock>> _mapBlockPools = new();

    public void LoadMap(Transform parent)
    {
        var levelConfiguration = _levelProvider?.GetCurrentLevel();
        if (levelConfiguration == null || _blockConfiguration == null)
            return;

        var interval = _blockConfiguration.ObjectInterval;
        var width = levelConfiguration.Width;
        var height = levelConfiguration.Height;
        var origin = parent.position;
        var spawnY = origin.y + (height - 1) * interval.y + _blockConfiguration.SpawnHeightAbove;
        var fallDuration = _blockConfiguration.FallDuration;
        _mapController.Init(width, height, origin, interval, _blockConfiguration.SwapDuration);
        var cellIndex = 0;

        for (var y = height - 1; y >= 0; y--)
        {
            for (var x = width - 1; x >= 0; x--)
            {
                var blockType = levelConfiguration.GetCell(x, y);
                if (blockType == GameBlockType.None)
                {
                    continue;
                }

                if (_mapBlockPools.ContainsKey(blockType) == false)
                {
                    var prefab = _blockConfiguration.GetPrefab(blockType);
                    var factory = _container.Instantiate<FactoryMonoDIObject<MapBlock>>();
                    factory.Initialize(prefab.gameObject, parent);
                    _mapBlockPools.Add(blockType, new Pool<MapBlock>(factory));
                }

                var targetPosition = _mapController.GetWorldPosition(x, y);
                var spawnPosition = new Vector3(targetPosition.x, spawnY, targetPosition.z);
                var instance = _mapBlockPools[blockType].Pull();
                instance.transform.position = spawnPosition;
                instance.transform.rotation = Quaternion.identity;
                instance.SetOrder((height - 1 - y) * width + x);
                _mapController.RegisterBlock(x, y, instance, blockType);
                if (instance is SwipeableMapBlock swipeable)
                {
                    swipeable.Init(_mapController);
                }

                var delay = cellIndex * DropDelayPerCell;
                instance.transform.DOMove(targetPosition, fallDuration).SetDelay(delay).SetEase(Ease.InCubic)
                    .OnComplete(
                        () =>
                        {
                            instance.transform.DOShakeScale(0.2f, 0.3f).SetAutoKill(true)
                                .SetTarget(instance.gameObject);
                        }).SetAutoKill(true).SetTarget(instance.gameObject);
                cellIndex++;
            }
        }
    }
}