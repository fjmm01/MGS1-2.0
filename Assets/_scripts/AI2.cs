using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI2 : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    public Transform player;
    State2 currentState2;




    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        currentState2 = new Idle2(this.gameObject, agent, anim, player);
    }

    // Update is called once per frame
    void Update()
    {
        currentState2 = currentState2.Process();
    }
}
