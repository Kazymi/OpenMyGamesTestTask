using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelBlockConfiguration", menuName = "Game/Level Block Configuration")]
public class LevelBlockConfiguration : ScriptableObject
{
    [Header("Prefabs")]
    [SerializeField] private LevelBlockData[] _blocks;

    [field:Header("Grids and other blocs setting")]
    [field: SerializeField] public Vector3 ObjectInterval { get; private set; }
    [field: SerializeField] public float SpawnHeightAbove { get; private set; } 
    [field: SerializeField] public float FallDuration { get; private set; } 
    [field: SerializeField] public float SwapDuration { get; private set; } 

    public MapBlock GetPrefab(string blockType)
    {
        var levelBlockData = _blocks.FirstOrDefault(t => t.BlockType == blockType);
        if (levelBlockData != null)
            return levelBlockData.Prefab;
        Debug.LogError($"Prefab {blockType} could not be found");
        return null;
    }
}