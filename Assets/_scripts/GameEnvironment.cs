using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameEnvironment 
{

    private static GameEnvironment instance;
    public List<GameObject> checkPoints = new List<GameObject>();
    public List<GameObject>CheckPoints { get { return checkPoints; } }

    public List<GameObject> ruta2 = new List<GameObject>();
    public List<GameObject> Ruta2 { get { return ruta2; } }

    public static GameEnvironment Singleton
    {
        get
        {
            if(instance == null)
            {
                instance = new GameEnvironment();
                instance.Ruta2.AddRange(GameObject.FindGameObjectsWithTag("Ruta2"));
                instance.CheckPoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
            }
            return instance;
        }
    }
   
}
