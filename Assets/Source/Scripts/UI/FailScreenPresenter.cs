using System;
using DG.Tweening;

public class FailScreenPresenter : Presenter<FailView>
{
    private const float DelayBeforeReload = 1.5f;
    private const string FailMessage = "Fail! Restart";

    private Tween _reloadDelayTween;

    public FailScreenPresenter(FailView view) : base(view)
    {
    }

    public void Show(Action onReload)
    {
        _reloadDelayTween?.Kill();
        if (View == null)
        {
            onReload?.Invoke();
            return;
        }

        View.Show(FailMessage, () =>
        {
            _reloadDelayTween = DOVirtual.DelayedCall(DelayBeforeReload, () =>
            {
                View.Hide();
                onReload?.Invoke();
            }).SetTarget(View.gameObject);
        });
    }
}
