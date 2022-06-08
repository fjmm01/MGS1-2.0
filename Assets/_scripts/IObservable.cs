using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservable<T>
{
    public void Notify(T value);

}

