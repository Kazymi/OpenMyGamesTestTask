using UnityEngine;

public class SwipeableMapBlock : MapBlock
{
    private const float SwipeThreshold = 0.35f;

    private MapGrid _mapGrid;
    private Camera _camera;
    private Vector3 _pointerDownWorld;

    public void Init(MapGrid mapGrid)
    {
        _mapGrid = mapGrid;
        _camera = Camera.main;
    }

    private Vector3 PointerWorldPosition()
    {
        var screen = Input.mousePosition;
        screen.z = Mathf.Abs(_camera.transform.position.z);
        return _camera.ScreenToWorldPoint(screen);
    }

    private bool TryGetSwipeDirection(Vector3 delta, out Vector2Int direction)
    {
        direction = default;
        if (Mathf.Abs(delta.x) < SwipeThreshold && Mathf.Abs(delta.y) < SwipeThreshold)
        {
            return false;
        }

        if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
        {
            direction = delta.x > 0 ? Vector2Int.right : Vector2Int.left;
        }
        else
        {
            direction = delta.y > 0 ? Vector2Int.down : Vector2Int.up;
        }

        return true;
    }

    private void OnMouseDown()
    {
        _pointerDownWorld = PointerWorldPosition();
    }

    private void OnMouseUp()
    {
        if (_mapGrid == null)
        {
            return;
        }
        var deltaPosition = PointerWorldPosition() - _pointerDownWorld;
        if (TryGetSwipeDirection(deltaPosition, out var direction) == false)
        {
            return;
        }
        _mapGrid.TrySwipe(this, direction);
    }
}