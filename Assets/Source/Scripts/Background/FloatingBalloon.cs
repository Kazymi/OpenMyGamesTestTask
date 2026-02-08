using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloatingBalloon : MonoPooled
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _balloonSprites;

    private const int PathSegments = 60;
    private const float DefaultAmplitude = 1f;
    private const float DefaultFrequency = 1.5f;
    private const float AppearDuration = 0.35f;
    private const float DisappearDuration = 0.25f;

    private Tween _scaleTween;
    private Tween _moveTween;
    
    private Action<FloatingBalloon> _onExited;

    public void Setup(float startX, float endX, float startY, float speed,
        Action<FloatingBalloon> onExited)
    {
        _onExited = onExited;
        _spriteRenderer.sprite = _balloonSprites[Random.Range(0, _balloonSprites.Length)];
        _moveTween?.Kill();
        var travelWidth = endX - startX;
        var duration = Mathf.Abs(travelWidth) / speed;
        var waypoints = new Vector3[PathSegments + 1];
        for (var i = 0; i <= PathSegments; i++)
        {
            var pathProgress = (float)i / PathSegments;
            var x = startX + pathProgress * travelWidth;
            var y = startY + DefaultAmplitude * Mathf.Sin(pathProgress * Mathf.PI * 2f * DefaultFrequency);
            waypoints[i] = new Vector3(x, y, transform.position.z);
        }

        transform.position = waypoints[0];
        OnMovementStarted();
        _moveTween = transform.DOPath(waypoints, duration, PathType.Linear, PathMode.Full3D)
            .SetEase(Ease.Linear)
            .OnComplete(OnMovementEnded)
            .SetTarget(gameObject);
    }

    private void OnMovementStarted()
    {
        _scaleTween?.Kill();
        transform.localScale = Vector3.zero;
        _scaleTween = transform.DOScale(Vector3.one, AppearDuration)
            .SetEase(Ease.OutBack)
            .SetTarget(gameObject);
    }

    private void OnMovementEnded()
    {
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(Vector3.zero, DisappearDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() => _onExited?.Invoke(this))
            .SetTarget(gameObject);
    }

    public override void ReturnToPool()
    {
        _scaleTween?.Kill();
        base.ReturnToPool();
    }

    private void OnDestroy()
    {
        _moveTween?.Kill();
    }
}