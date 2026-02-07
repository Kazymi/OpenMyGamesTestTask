using System;
using UnityEngine;
using Zenject;

public class FactoryMonoDIObject<T> : IFactory<T>
{
    private GameObject _prefab;
    private Transform _parent;
    [Inject] private DiContainer _diContainer;

    public void Initialize(GameObject prefab, Transform parent)
    {
        _parent = parent;
        _prefab = prefab;
        _parent = parent.transform;
    }
    public T CreatePoolObject()
    {
        var prefab = _diContainer.InstantiatePrefab(_prefab, _parent);
        var returnValue = prefab.GetComponent<T>();
        var iPooled = prefab.GetComponent<IPooledObject>();
        if (iPooled != null)
        {
            iPooled.ReturnedToPoolEvent += () => { prefab.transform.SetParent(_parent); };
        }

        prefab.SetActive(false);
        if (returnValue != null)
        {
            return returnValue;
        }
        else
        {
            throw new InvalidOperationException(
                $"The requested object is missing from the prefab {typeof(T)} >> {_prefab.name}");
        }
    }
}