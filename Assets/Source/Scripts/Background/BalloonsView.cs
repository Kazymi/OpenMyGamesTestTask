using System;
using UnityEngine;

public class BalloonsView : MonoBehaviour, IPresenterView
{
    [field: SerializeField] public GameObject BalloonPrefab { get; private set; }
    [field: SerializeField] public Transform BalloonParetTransform { get; private set; }
    [SerializeField] private float _marginFromEdge = 2f;
    [SerializeField] private float _minY = -3f;
    [SerializeField] private float _maxY = 3f;

    private float _leftBound = -1f;
    private float _rightBound = -1f;
    private float _topBound = -1f;
    private Action<FloatingBalloon> _onBalloonExited;
    private Camera _camera;

    public float MinY => _minY;
    public float MaxY => _maxY;

    public float MinX()
    {
        EnsureBounds();
        return _leftBound;
    }

    public float MaxX()
    {
        EnsureBounds();
        return _rightBound;
    }

    public float TopY()
    {
        EnsureBounds();
        return _topBound;
    }

    private void EnsureBounds()
    {
        if (_leftBound < _rightBound)
        {
            return;
        }

        _camera ??= Camera.main;
        var zPosition = Mathf.Abs(_camera.transform.position.z);
        _leftBound = _camera.ViewportToWorldPoint(new Vector3(0f, 0.5f, zPosition)).x + _marginFromEdge;
        _rightBound = _camera.ViewportToWorldPoint(new Vector3(1f, 0.5f, zPosition)).x - _marginFromEdge;
        _topBound = _camera.ViewportToWorldPoint(new Vector3(0.5f, 1f, zPosition)).y - _marginFromEdge;
    }

    private void Awake()
    {
        EnsureBounds();
    }

    public void SetBalloonExitedCallback(Action<FloatingBalloon> onBalloonExited)
    {
        _onBalloonExited = onBalloonExited;
    }

    public void SetupBalloon(FloatingBalloon balloon, float startX, float startY, float speed)
    {
        EnsureBounds();
        balloon.Setup(startX, startY, speed, _topBound, _onBalloonExited);
    }
}
