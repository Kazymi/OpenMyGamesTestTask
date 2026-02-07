using System;

public class Factory<T> : IFactory<T>
{
    private readonly object[] _args;

    public Factory(params object[] args)
    {
        _args = args;
    }

    public T CreatePoolObject()
    {
        return (T) Activator.CreateInstance(typeof(T), _args);
    }
}