using System;
using UnityEngine;

public class MonoPooled : MonoBehaviour, IPooledObject
{
    private bool _isDisabled;
    private IPool _pool;
    public Action ReturnedToPoolEvent { get; set; }

    public virtual void Initialize()
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        _isDisabled = true;
    }

    public virtual void ReturnToPool()
    {
        if (_isDisabled)
        {
            return;
        }

        gameObject.SetActive(false);
        _pool.Push(this);
    }

    public void SetParentPool<T>(IPool<T> parent) where T : IPooledObject
    {
        _pool = parent;
    }
}