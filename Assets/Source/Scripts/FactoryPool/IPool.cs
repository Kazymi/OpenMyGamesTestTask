
    public interface IPool<T> : IPool where T : IPooledObject
    {
        T Pull();
    }

    public interface IPool
    {
        void Push(IPooledObject pooledObject);
    }
