using UnityEngine;

[System.Serializable]
public class LevelBlockData
{
    [field: SerializeField] public GameBlockType BlockType { get; private set; }
    [field: SerializeField] public MapBlock Prefab { get; private set; }
}