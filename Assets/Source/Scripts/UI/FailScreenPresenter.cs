using System;
using DG.Tweening;

public class FailScreenPresenter : Presenter<FailPresenterView>
{
    private const float DelayBeforeReload = 1.5f;
    private const string FailMessage = "Fail! Restart";

    public FailScreenPresenter(FailPresenterView view) : base(view)
    {
    }

    public void Show(Action onReload)
    {
        if (View == null)
        {
            onReload?.Invoke();
            return;
        }

        View.Show(FailMessage, () =>
        {
            DOVirtual.DelayedCall(DelayBeforeReload, () =>
            {
                View.Hide();
                onReload?.Invoke();
            });
        });
    }
}