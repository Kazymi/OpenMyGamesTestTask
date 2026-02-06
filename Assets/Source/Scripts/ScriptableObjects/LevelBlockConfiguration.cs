using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelBlockConfiguration", menuName = "Game/Level Block Configuration")]
public class LevelBlockConfiguration : ScriptableObject
{
    [SerializeField] private LevelBlockData[] _blocks;
    [field: SerializeField] public Vector3 ObjectInterval { get; private set; } = new Vector3(1f, 1f, 0f);
    [field: SerializeField] public float SpawnHeightAbove { get; private set; } = 5f;
    [field: SerializeField] public float FallDuration { get; private set; } = 0.5f;
    [field: SerializeField] public float SwapDuration { get; private set; } = 0.5f;

    public MapBlock GetPrefab(GameBlockType blockType)
    {
        var levelBlockData = _blocks.FirstOrDefault(t => t.BlockType == blockType);
        if (levelBlockData != null)
        {
            return levelBlockData.Prefab;
        }

        Debug.LogError($"Prefab {blockType} could not be found");
        return null;
    }
}