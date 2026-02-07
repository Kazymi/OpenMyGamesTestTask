using System;
using DG.Tweening;
using UnityEngine;
using TMPro;

// В теории не обязательно Mono; сделан так для удобства биндинга в инсталлере. При необходимости можно заменить Awake на IInitializable.
public class FailView : MonoBehaviour, IPresenterView
{
    [Header("UI References")]
    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _messageText;

    [Header("Animation")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private Ease _scaleEase = Ease.OutBack;

    private Tween _showTween;

    private void Awake()
    {
        if (_root != null)
        {
            _root.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _showTween?.Kill();
    }

    public void Show(string message, Action onAnimationComplete)
    {
        _showTween?.Kill();
        if (_messageText != null)
        {
            _messageText.text = message;
        }
        if (_root != null)
        {
            _root.SetActive(true);
        }

        if (_messageText != null)
        {
            _messageText.transform.localScale = Vector3.zero;
            _showTween = _messageText.transform
                .DOScale(Vector3.one, _animationDuration)
                .SetEase(_scaleEase)
                .OnComplete(() => onAnimationComplete?.Invoke())
                .SetTarget(_messageText.gameObject);
        }
        else
        {
            onAnimationComplete?.Invoke();
        }
    }

    public void Hide()
    {
        if (_root != null)
        {
            _root.SetActive(false);
        }
    }
}
