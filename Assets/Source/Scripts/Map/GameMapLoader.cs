using UnityEngine;
using Zenject;

public class GameMapLoader
{
    [Inject] private LevelConfiguration _levelConfiguration;
    [Inject] private LevelBlockConfiguration _blockConfiguration;

    public void LoadMap(Transform parent)
    {
        if (_levelConfiguration == null || _blockConfiguration == null)
            return;

        var interval = _blockConfiguration.ObjectInterval;
        var width = _levelConfiguration.Width;
        var height = _levelConfiguration.Height;
        var origin = parent.position;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var blockType = _levelConfiguration.GetCell(x, y);
                if (blockType == GameBlockType.None)
                {
                    continue;
                }

                var prefab = _blockConfiguration.GetPrefab(blockType);
                if (prefab == null)
                {
                    continue;
                }

                var position = new Vector3(
                    origin.x + x * interval.x,
                    origin.y + y * interval.y,
                    origin.z);
                var instance = Object.Instantiate(prefab, position, Quaternion.identity, parent);
                var spriteRenderer = instance.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                    spriteRenderer.sortingOrder = (height - 1 - y) * width + x;
            }
        }
    }
}
