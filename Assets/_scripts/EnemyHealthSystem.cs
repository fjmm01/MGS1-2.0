using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    [SerializeField]
    float maxHP;
    public float currentHP;


   
    void Start()
    {
        currentHP = maxHP;
    }

    
    void Update()
    {
        
    }


    
}
