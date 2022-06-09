using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour, IObserver<bool>
{

    NavMeshAgent agent;
    Animator anim;
    public Transform player;
    State currentState;
    public Transform shootingPoint;
    public PlayerHealthSystem playerHealth;
    
    

    public bool OnNotifyAI = false;
    
    


    // Start is called before the first frame update
    void Start()
    {
        EnemyObservable.EnemyCallbackAction += UpdateObserver;
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        currentState = new Idle(this.gameObject, agent, anim, player,shootingPoint, playerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(!OnNotifyAI)
        currentState = currentState.Process();
    }

    public void UpdateObserver(bool value)
    {
        currentState = new Pursue(this.gameObject, agent, anim, player, shootingPoint, playerHealth);
        StartCoroutine(NotifyToggleIE());

        Debug.Log("***** Me han llamado para perseguir al jugador. " + name);
    }

    public IEnumerator NotifyToggleIE()
    {
        yield return new WaitForSeconds(10);
        OnNotifyAI = true;
        yield return null;
    }
}
