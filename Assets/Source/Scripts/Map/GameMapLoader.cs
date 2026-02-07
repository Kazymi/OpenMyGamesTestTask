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

    private readonly Dictionary<GameBlockType, IPool<MapBlock>> _pools = new();

    public void LoadMap(Transform parent)
    {
        var level = _levelProvider.GetCurrentLevel();
        if (CanLoad(level) == false)
            return;
        InitializeMap(level.Width, level.Height, parent);
        SpawnAllBlocks(level, parent);
    }

    public void LoadMapFromSnapshot(GameplayStateSnapshot snapshot, Transform parent)
    {
        if (snapshot == null || _blockConfiguration == null)
            return;
        InitializeMap(snapshot.Width, snapshot.Height, parent);
        SpawnAllBlocksFromSnapshot(snapshot, parent);
    }

    private bool CanLoad(LevelConfiguration level)
    {
        return level != null && _blockConfiguration != null;
    }

    private void InitializeMap(int width, int height, Transform parent)
    {
        _mapController.Init(width, height, parent.position,
            _blockConfiguration.ObjectInterval, _blockConfiguration.SwapDuration);
    }

    private void SpawnAllBlocks(LevelConfiguration level, Transform parent)
    {
        var spawnY = CalculateSpawnY(level.Height, parent.position.y);
        var cellIndex = 0;
        for (var y = level.Height - 1; y >= 0; y--)
        {
            for (var x = level.Width - 1; x >= 0; x--)
            {
                var type = level.GetCell(x, y);
                if (type == GameBlockType.None)
                {
                    continue;
                }

                SpawnBlock(type, x, y, spawnY, cellIndex, level.Width, level.Height, parent);
                cellIndex++;
            }
        }
    }

    private void SpawnAllBlocksFromSnapshot(GameplayStateSnapshot snapshot, Transform parent)
    {
        var spawnY = CalculateSpawnY(snapshot.Height, parent.position.y);
        var cellIndex = 0;
        for (var y = snapshot.Height - 1; y >= 0; y--)
        {
            for (var x = snapshot.Width - 1; x >= 0; x--)
            {
                var type = snapshot.GetCell(x, y);
                if (type == GameBlockType.None)
                    continue;
                SpawnBlock(type, x, y, spawnY, cellIndex, snapshot.Width, snapshot.Height, parent);
                cellIndex++;
            }
        }
    }

    private void SpawnBlock(GameBlockType type, int x, int y, float spawnY, int cellIndex,
        int width, int height, Transform parent)
    {
        var block = GetBlockFromPool(type, parent);
        block.transform.localScale = Vector3.one;
        DOTween.Kill(block.gameObject);
        var targetPosition = _mapController.GetWorldPosition(x, y);
        var spawnPosition = CreateSpawnPosition(targetPosition, spawnY);
        block.transform.position = spawnPosition;
        SetupBlock(block, type, x, y, width, height, spawnPosition);
        AnimateBlockDrop(block, targetPosition, cellIndex);
    }

    private MapBlock GetBlockFromPool(GameBlockType type, Transform parent)
    {
        if (_pools.TryGetValue(type, out var pool))
        {
            return pool.Pull();
        }

        pool = CreatePool(type, parent);
        _pools[type] = pool;
        return pool.Pull();
    }

    private IPool<MapBlock> CreatePool(GameBlockType type, Transform parent)
    {
        var prefab = _blockConfiguration.GetPrefab(type);
        var factory = _container.Instantiate<FactoryMonoDIObject<MapBlock>>();
        factory.Initialize(prefab.gameObject, parent);
        return new Pool<MapBlock>(factory);
    }

    private void SetupBlock(MapBlock block, GameBlockType type, int x, int y,
        int width, int height, Vector3 spawnPosition)
    {
        DOTween.Kill(block.gameObject);
        block.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
        block.SetOrder(CalculateSortingOrder(x, y, width, height));
        _mapController.RegisterBlock(x, y, block, type);
        if (block is SwipeableMapBlock swipeable)
            swipeable.Init(_mapController);
    }

    private void AnimateBlockDrop(MapBlock block, Vector3 targetPosition, int cellIndex)
    {
        var delay = CalculateDropDelay(cellIndex);
        block.transform
            .DOMove(targetPosition, _blockConfiguration.FallDuration)
            .SetDelay(delay)
            .SetEase(Ease.InCubic)
            .OnComplete(() => PlayLandingAnimation(block))
            .SetAutoKill(true)
            .SetTarget(block.gameObject);
    }

    private void PlayLandingAnimation(MapBlock block)
    {
        block.transform.DOShakeScale(0.2f, 0.3f).SetAutoKill(true).SetTarget(block.gameObject).OnComplete(() =>
        {
            block.transform.localScale = Vector3.one;
        });
    }

    private float CalculateSpawnY(int height, float originY)
    {
        return originY + (height - 1) * _blockConfiguration.ObjectInterval.y +
               _blockConfiguration.SpawnHeightAbove;
    }

    private Vector3 CreateSpawnPosition(Vector3 target, float spawnY)
    {
        return new Vector3(target.x, spawnY, target.z);
    }

    private int CalculateSortingOrder(int x, int y, int width,
        int height) //Я долго думал как можно по красивому сделать без смены ордера, но видимо в 22х версиях unity отрисовка sr не зависит от позиции в иерархии(
    {
        return (height - 1 - y) * width + x;
    }

    private float CalculateDropDelay(int cellIndex)
    {
        return cellIndex * DropDelayPerCell;
    }
}