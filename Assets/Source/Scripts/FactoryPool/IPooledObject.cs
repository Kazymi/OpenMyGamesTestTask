using System;

public interface IPooledObject
{
    Action ReturnedToPoolEvent { get; set; }
    void ReturnToPool();
    void Initialize();
    void SetParentPool<T>(IPool<T> parent) where T : IPooledObject;
}