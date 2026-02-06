using UnityEngine;

public class MapBlock : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _mapBlockImage;

    private int _gridX;
    private int _gridY;

    public Vector2Int GridPosition => new Vector2Int(_gridX, _gridY);

    public void SetOrder(int order)
    {
        _mapBlockImage.sortingOrder = order;
    }

    public void SetGridPosition(int x, int y)
    {
        _gridX = x;
        _gridY = y;
    }
}