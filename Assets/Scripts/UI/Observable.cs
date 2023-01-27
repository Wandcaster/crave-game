using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Observable<T>
{
    public Observable(T value)
    {
        this.value = value;
        onChange = null;
    }
    [SerializeField] private T value;
    public delegate void ValueChange(T oldValue, T newValue);

    public void Set(T newValue)
    {
        onChange?.Invoke(value, newValue);
        value = newValue;
    }

    public T Get()
    {
        return value;
    }


    public event ValueChange onChange;

}