using System;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class FailPresenterView : MonoBehaviour, IPresenterView
{
    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private Ease _scaleEase = Ease.OutBack;

    private void Awake()
    {
        if (_root != null)
        {
            _root.SetActive(false);
        }
    }

    public void Show(string message, Action onAnimationComplete)
    {
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
            _messageText.transform.DOScale(Vector3.one, _animationDuration).SetEase(_scaleEase)
                .OnComplete(() => onAnimationComplete?.Invoke());
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
