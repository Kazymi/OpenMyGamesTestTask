using UnityEngine;

[System.Serializable]
public class LevelBlockData
{
    [field: SerializeField] public string BlockType { get; private set; } //String нужен для того что-бы дать возможность добавить новый тип блока, не лезя в код
    [field: SerializeField] public MapBlock Prefab { get; private set; }
}