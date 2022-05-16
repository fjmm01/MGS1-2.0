using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{


    [SerializeField]
    float maxHP;

    [SerializeField]
    float currentHP;

    private Animator anim;

    void Awake()
    {
        currentHP = maxHP;
        anim = GetComponentInChildren<Animator>();
    }

    
    void Update()
    {
        TakeDamage();
    }


    private void TakeDamage()
    {
        anim.SetFloat("Health", currentHP); 





        
        
    }

    
}
