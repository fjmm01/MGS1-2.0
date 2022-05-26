using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    void update(bool encontrado);
}
public class Observer : IObserver
{
    public string UserName { get; set; }

    public Observer(string userName, ISubject subject)
    {
        UserName = userName;
        subject.RegisterOberserver(this);
    }

    public void update(bool encontrado)
    {
        Debug.Log(UserName + "ha encontrado a Snake");
    }
}
