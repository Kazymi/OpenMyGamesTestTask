using System;

public abstract class Presenter<TView> where TView : IPresenterView
{
    protected TView View;

    protected Presenter(TView view)
    {
        View = view ?? throw new ArgumentNullException(nameof(view));
    }
}