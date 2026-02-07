using System;
using System.Collections;
using UnityEngine;

public class TemporaryMonoPooled : MonoBehaviour, IPooledObject
{
    [SerializeField] private float timer;
    private IPool _pool;
    public Action ReturnedToPoolEvent { get; set; }
    public event Action<TemporaryMonoPooled> ReadyToReturnPool;

    protected float Timer
    {
        set => timer = value;
    }

    public virtual void Initialize()
    {
        if (this == null)
        {
            return;
        }

        gameObject.SetActive(true);
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(Destroy());
        }
    }

    public void ReturnToPool()
    {
        ReadyToReturnPool?.Invoke(this);
        gameObject.SetActive(false);
        _pool.Push(this);
    }

    public void SetParentPool<T>(IPool<T> parent) where T : IPooledObject
    {
        _pool = parent;
    }

    protected IEnumerator Destroy()
    {
        yield return new WaitForSeconds(timer);
        ReturnToPool();
    }
}