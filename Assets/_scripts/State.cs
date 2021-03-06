using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State : EnemyObservable
{


    public enum STATE
    {
        IDLE,PATROL,PURSUE,ATTACK,DEATH
    };
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };


    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;
    RaycastHit hitPlayer;
    protected Transform shootingPoint;
    protected PlayerHealthSystem playerhealth;
    public AI aiBrain;
    public EnemyHealthSystem enemyHealth;
    
    

    float visDist = 10.0f;
    float visAngle = 180.0f;
    float shootDist = 7.0f;

    public float fireRate = 1f;
    public float nextFire;

   
    public State(GameObject _npc,NavMeshAgent _agent, Animator _anim, Transform _player, Transform _shootingPoint, PlayerHealthSystem _playerhealth, AI _aibrain, EnemyHealthSystem _enemyHealth)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
        shootingPoint = _shootingPoint;
        playerhealth = _playerhealth;
        aiBrain = _aibrain;
        enemyHealth = _enemyHealth;
        
    }


    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if(stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        Physics.Raycast(npc.transform.position, direction, out hitPlayer);
        
        if(direction.magnitude < visDist && angle < visAngle && hitPlayer.transform.tag.Equals("Player"))
        {
            return true;
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if(direction.magnitude < shootDist)
        {
            return true;
        }
        return false;
    }

    public override void Notify(float value)
    {
        PlayerHealthSystem.UpdateHPAction.Invoke(value);
        EnemyCallbackAction.Invoke(true);
    }
}

public class Idle: State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform _shootingPoint, PlayerHealthSystem _playerHealth, AI _aibrain, EnemyHealthSystem _enemyHealth)
                :base(_npc,_agent,_anim,_player,_shootingPoint, _playerHealth, _aibrain,_enemyHealth)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        anim.SetTrigger("isIdle");
        base.Enter();

    }

    public override void Update()
    {
        if(!OnNotifyAI)
        {
            if(CanSeePlayer())
            {
                nextState = new Pursue(npc, agent, anim, player, shootingPoint, playerhealth, aiBrain, enemyHealth);
                stage = EVENT.EXIT;
            }
            else if(Random.Range(0,100) < 10)
            {
                nextState = new Patrol(npc, agent, anim, player, shootingPoint, playerhealth, aiBrain, enemyHealth);
                stage = EVENT.EXIT;
            }
        }
        
    }
    public override void Exit()
    {
        anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class Patrol: State
{
    int currentIndex = -1;
    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform _shootingPoint, PlayerHealthSystem _playerHealth, AI _aibrain, EnemyHealthSystem _enemyHealth)
            : base(_npc, _agent, _anim, _player, _shootingPoint, _playerHealth, _aibrain, _enemyHealth)
    {
        name = STATE.PATROL;
        agent.speed = 1;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity;
        for(int i =0; i < GameEnvironment.Singleton.CheckPoints.Count; i++ )
        {
            GameObject thisWP = GameEnvironment.Singleton.CheckPoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if(distance < lastDist)
            {
                currentIndex = i-1;
                lastDist = distance;
            }
        }
        anim.SetTrigger("isWalking");
        base.Enter();
    }

    public override void Update()
    {
        if(!OnNotifyAI)
        {
            if(agent.remainingDistance < 1)
            {
                if (currentIndex >= GameEnvironment.Singleton.CheckPoints.Count - 1)
                    currentIndex = 0;
                else
                    currentIndex++;

                agent.SetDestination(GameEnvironment.Singleton.CheckPoints[currentIndex].transform.position);
            }

            if (CanSeePlayer())
            {
                nextState = new Pursue(npc, agent, anim, player, shootingPoint, playerhealth, aiBrain, enemyHealth);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isWalking");
        base.Exit();
    }
}

public class Pursue: State
{
    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform _shootingPoint, PlayerHealthSystem _playerHealth, AI _aibrain, EnemyHealthSystem _enemyHealth)
            : base(_npc, _agent, _anim, _player, _shootingPoint, _playerHealth, _aibrain, _enemyHealth)
    {
        name=STATE.PURSUE;
        agent.speed = 3;
        agent.isStopped = false;
    }
    public override void Enter()
    {
        anim.SetTrigger("isRunning");
        base.Enter();
    }
    public override void Update()
    {

        if(!OnNotifyAI)
        {
            agent.SetDestination(player.position);
            if(agent.hasPath)
            {
                if(CanAttackPlayer())
                {
                    nextState = new Attack(npc, agent, anim, player,shootingPoint, playerhealth, aiBrain, enemyHealth);
                    stage = EVENT.EXIT; 
                }
                else if(!CanSeePlayer())
                {
                    nextState = new Patrol(npc, agent, anim, player, shootingPoint, playerhealth, aiBrain, enemyHealth);
                    stage = EVENT.EXIT;
                }
            }
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class Attack : State
{
    float rotationSpeed = 2;
    AudioSource shoot;

    bool toggle = false;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform _shootingPoint, PlayerHealthSystem _playerHealth, AI _aibrain, EnemyHealthSystem _enemyHealth)
            : base(_npc, _agent, _anim, _player,_shootingPoint, _playerHealth, _aibrain, _enemyHealth)
    {
        name = STATE.ATTACK;
        shoot = _npc.GetComponent<AudioSource>();

    }

    public override void Enter()
    {
        anim.SetTrigger("isShooting");
        agent.isStopped = true;
        shoot.Play();
        base.Enter();
    }

    public override void Update()
    {

        if(!OnNotifyAI)
        {
            Vector3 direction = player.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);
            direction.y = 0;
        
        

        
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

            if(!CanAttackPlayer())
            {
                nextState = new Idle(npc, agent, anim, player, shootingPoint, playerhealth, aiBrain, enemyHealth);
                stage = EVENT.EXIT;
            }

            RaycastHit bullet;


       
            Physics.Raycast(shootingPoint.transform.position, direction, out bullet);
            if (bullet.transform.tag.Equals("Player") && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;

                //playerhealth.currentHP -= 5;

                if(!OnNotifyAI)
                {
                    Notify(5);

                    //OnNotifyAI = true;
                    aiBrain.StartCoroutine(aiBrain.NotifyToggleIE());
                }


                 //Debug.Log("TE HE DADO");
                 if (playerhealth.CurrentHP == 0f)
                 {
                    nextState = new Idle(npc, agent, anim, player, shootingPoint, playerhealth, aiBrain, enemyHealth);
                    stage = EVENT.EXIT;
                 }

            }
            if(enemyHealth.currentHP == 0f)
            {
                nextState = new Death(npc, agent, anim, player, shootingPoint, playerhealth, aiBrain, enemyHealth);
                stage = EVENT.EXIT;
            }
        }       
    }

    

    public override void Exit()
    {
        toggle = false;

        anim.ResetTrigger("isShooting");
        shoot.Stop();
        base.Exit();
    }

    
}
public class Death : State
{
    public Death(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform _shootingPoint, PlayerHealthSystem _playerHealth, AI _aibrain, EnemyHealthSystem _enemyHealth)
           : base(_npc, _agent, _anim, _player, _shootingPoint, _playerHealth, _aibrain, _enemyHealth)
    {
        name = STATE.DEATH;
        agent.speed = 0;
        agent.isStopped = true;
    }
    public override void Enter()
    {
        anim.SetTrigger("isSleeping");
        base.Enter();
    }

    public override void Update()
    {

        Destroy(npc, 1);
    }

    public override void Exit()
    {
        anim.ResetTrigger("isSleeping");
        base.Exit();
    }
}