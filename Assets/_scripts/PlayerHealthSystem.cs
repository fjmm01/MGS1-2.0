using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthSystem : MonoBehaviour, IObserver<float>
{


    [SerializeField]
    float maxHP;

    
    private float currentHP;
    public float CurrentHP { get { return currentHP; } private set { } }
    private Animator anim;

    public static UnityAction<float> UpdateHPAction;
   

    void Awake()
    {

        UpdateHPAction += UpdateObserver;

        currentHP = maxHP;
        anim = GetComponentInChildren<Animator>();
    }

    private void OnDisable()
    {
        UpdateHPAction -= UpdateObserver;
    }


    void Update()
    {
        TakeDamage();
    }


    private void TakeDamage()
    {
        anim.SetFloat("Health", currentHP);       
    }


    public void UpdateObserver(float value)
    {
        currentHP += value;
    }
}
