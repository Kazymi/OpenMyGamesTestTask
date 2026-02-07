using System;
using UnityEngine;

public class MapBlock : MonoPooled
{
    [SerializeField] private SpriteRenderer _mapBlockImage;

    private int _gridX;
    private int _gridY;
    private GameBlockType _blockType;

    public Vector2Int GridPosition => new Vector2Int(_gridX, _gridY);
    public GameBlockType BlockType => _blockType;

    //подписка на событие для удаление блока. 
    public event Action<Action> MatchedEvent;

    public void SetBlockType(GameBlockType blockType)
    {
        _blockType = blockType;
    }

    public void SetOrder(int order)
    {
        _mapBlockImage.sortingOrder = order;
    }

    public void SetGridPosition(int x, int y)
    {
        _gridX = x;
        _gridY = y;
    }

    public void RaiseMatched(Action onComplete)
    {
        if (MatchedEvent == null)
        {
            onComplete?.Invoke();
            return;
        }
        MatchedEvent.Invoke(onComplete);
    }
}