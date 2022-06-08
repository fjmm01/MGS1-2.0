using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager Instance;

    // Start is called before the first frame update
    private void Start()
    {
        if(Instance == null)
        {
            Instance = new EnemyManager();
        }


    }


}
