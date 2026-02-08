using DG.Tweening;
using UnityEngine;
using Zenject;

public class BalloonsPresenter : Presenter<BalloonsView>, IInitializable
{
    private const int MaxBalloonsOnScreen = 3;
    private const float SpawnInterval = 1.5f;
    private const float MinSpeed = 2f;
    private const float MaxSpeed =3f;

    private readonly DiContainer _container;
    private IPool<FloatingBalloon> _pool;

    public BalloonsPresenter(BalloonsView view, DiContainer container) : base(view)
    {
        _container = container;
    }

    public void Initialize()
    {
        var factory = _container.Instantiate<FactoryMonoDIObject<FloatingBalloon>>();
        factory.Initialize(View.BalloonPrefab, View.BalloonParetTransform);
        _pool = new Pool<FloatingBalloon>(factory);
        View.SetBalloonExitedCallback(OnBalloonExited);
        for (var i = 0; i < MaxBalloonsOnScreen; i++)
        {
            DOVirtual.DelayedCall(SpawnInterval * i, SpawnOneBalloon);
        }
    }

    private void OnBalloonExited(FloatingBalloon balloon)
    {
        balloon.ReturnToPool();
        SpawnOneBalloon();
    }

    private void SpawnOneBalloon()
    {
        var balloon = _pool.Pull();
        balloon.Initialize();
        var startX = Random.Range(View.MinX(), View.MaxX());
        var startY = Random.Range(View.MinY, View.MaxY);
        var speed = Random.Range(MinSpeed, MaxSpeed);
        View.SetupBalloon(balloon, startX, startY, speed);
    }
}
