using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuView : MonoBehaviour, IPresenterView
{
    [field: SerializeField] public Button RestartGameButton { get; private set; }
    [field: SerializeField] public Button NextGameButton { get; private set; }

    private const float PulseDuration = 0.3f;
    private const float PulseStrength = 0.2f;

    public void PlayPulse(Transform pulseTransform)
    {
        pulseTransform.transform.DOKill();
        pulseTransform.transform.DOShakeScale(PulseDuration, PulseStrength).OnComplete(() =>
        {
            pulseTransform.transform.localScale = Vector3.one;
        });
    }
}