using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject
{
    void RegisterOberserver(IObserver obserever);
    void RemoveObserver(IObserver observer);
    void NotifyObservers();
}
public class Subject : ISubject
{
    private List<IObserver> observers = new List<IObserver>();
    private string Name { get; set; }
    private bool Encontrado { get; set; }

    public Subject(bool encontrado, string name)
    {
        Name = name; 
        Encontrado = encontrado;
    }

    public bool getEncontrado()
    {
        return Encontrado;
    }

    public void setEncontrado(bool encontrado)
    {
        this.Encontrado = encontrado;
        Debug.Log("pillado");
        NotifyObservers();

    }


    public void AddObservers(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        Debug.Log("HEMOS ENCONTRADO A SNAKE");
        foreach (IObserver observer in observers)
        {
            observer.update(Encontrado);
        }
    }

    public void RegisterOberserver(IObserver obserever)
    {
        Debug.Log("Observer Added : " + ((Observer)obserever).UserName);
        observers.Add(obserever);
    }
}
