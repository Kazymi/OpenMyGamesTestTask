using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuView : MonoBehaviour, IPresenterView
{
    private const float PulseDuration = 0.3f;
    private const float PulseStrength = 0.2f;

    [field:Header("Buttons")]
    [field: SerializeField] public Button RestartGameButton { get; private set; }
    [field: SerializeField] public Button NextGameButton { get; private set; }

    private Tween _pulseTween;

    private void OnDestroy()
    {
        _pulseTween?.Kill();
    }

    public void PlayPulse(Transform pulseTransform)
    {
        _pulseTween?.Kill();
        _pulseTween = pulseTransform.DOShakeScale(PulseDuration, PulseStrength)
            .OnComplete(() => pulseTransform.localScale = Vector3.one)
            .SetTarget(pulseTransform.gameObject);
    }
}
