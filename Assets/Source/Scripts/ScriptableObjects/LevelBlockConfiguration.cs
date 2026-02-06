using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelBlockConfiguration", menuName = "Game/Level Block Configuration")]
public class LevelBlockConfiguration : ScriptableObject
{
    [SerializeField] private LevelBlockData[] _blocks;
    [SerializeField] private Vector3 _objectInterval = new Vector3(1f, 1f, 0f);

    public Vector3 ObjectInterval => _objectInterval;

    public GameObject GetPrefab(GameBlockType blockType)
    {
        var levelBlockData = _blocks.FirstOrDefault(t=>t.BlockType == blockType);
        if (levelBlockData != null)
        {
            return levelBlockData.Prefab;
        }
        Debug.LogError($"Prefab {blockType} could not be found");
        return null;
    }
}
