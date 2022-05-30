using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State2 
{
    public enum STATE
    {
        IDLE, PATROL, PURSUE, ATTACK
    };
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };


    public STATE name2;
    protected EVENT stage2;
    protected GameObject npc2;
    protected Animator anim2;
    protected Transform player2;
    protected State2 nextState2;
    protected NavMeshAgent agent2;

    float visDist = 10.0f;
    float visAngle = 60.0f;
    float shootDist = 7.0f;


    public State2(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc2 = _npc;
        agent2 = _agent;
        anim2 = _anim;
        stage2 = EVENT.ENTER;
        player2 = _player;
    }

    public virtual void Enter() { stage2 = EVENT.UPDATE; }
    public virtual void Update() { stage2 = EVENT.UPDATE; }
    public virtual void Exit() { stage2 = EVENT.EXIT; }

    public State2 Process()
    {
        if (stage2 == EVENT.ENTER) Enter();
        if (stage2 == EVENT.UPDATE) Update();
        if (stage2 == EVENT.EXIT)
        {
            Exit();
            return nextState2;
        }
        return this;
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = player2.position - npc2.transform.position;
        float angle = Vector3.Angle(direction, npc2.transform.forward);

        if (direction.magnitude < visDist && angle < visAngle)
        {
            return true;
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player2.position - npc2.transform.position;
        if (direction.magnitude < shootDist)
        {
            return true;
        }
        return false;
    }
}

public class Idle2 : State2
{
    public Idle2(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
                : base(_npc, _agent, _anim, _player)
    {
        name2 = STATE.IDLE;
    }

    public override void Enter()
    {
        anim2.SetTrigger("isIdle");
        base.Enter();

    }

    public override void Update()
    {
        if (CanSeePlayer())
        {
            nextState2 = new Pursue2(npc2, agent2, anim2, player2);
            stage2 = EVENT.EXIT;
        }
        else if (Random.Range(0, 100) < 10)
        {
            nextState2 = new Patrol2(npc2, agent2, anim2, player2);
            stage2 = EVENT.EXIT;
        }

    }
    public override void Exit()
    {
        anim2.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class Patrol2 : State2
{
    int currentIndex = -1;
    public Patrol2(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
            : base(_npc, _agent, _anim, _player)
    {
        name2 = STATE.PATROL;
        agent2.speed = 1;
        agent2.isStopped = false;
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity;
        for (int i = 0; i < GameEnvironment.Singleton.Ruta2.Count; i++)
        {
            GameObject thisWP = GameEnvironment.Singleton.Ruta2[i];
            float distance = Vector3.Distance(npc2.transform.position, thisWP.transform.position);
            if (distance < lastDist)
            {
                currentIndex = i - 1;
                lastDist = distance;
            }
        }
        anim2.SetTrigger("isWalking");
        base.Enter();
    }

    public override void Update()
    {
        if (agent2.remainingDistance < 1)
        {
            if (currentIndex >= GameEnvironment.Singleton.Ruta2.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;

            agent2.SetDestination(GameEnvironment.Singleton.Ruta2[currentIndex].transform.position);
        }

        if (CanSeePlayer())
        {
            nextState2 = new Pursue2(npc2, agent2, anim2, player2);
            stage2 = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        anim2.ResetTrigger("isWalking");
        base.Exit();
    }
}

public class Pursue2 : State2
{
    public Pursue2(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
            : base(_npc, _agent, _anim, _player)
    {
        name2 = STATE.PURSUE;
        agent2.speed = 3;
        agent2.isStopped = false;
    }
    public override void Enter()
    {
        anim2.SetTrigger("isRunning");
        base.Enter();
    }
    public override void Update()
    {
        agent2.SetDestination(player2.position);
        if (agent2.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState2 = new Attack2(npc2, agent2, anim2, player2);
                stage2 = EVENT.EXIT;
            }
            else if (!CanSeePlayer())
            {
                nextState2 = new Patrol2(npc2, agent2, anim2, player2);
                stage2 = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        anim2.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class Attack2 : State2
{
    float rotationSpeed = 2;
    AudioSource shoot;

    public Attack2(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
            : base(_npc, _agent, _anim, _player)
    {
        name2 = STATE.ATTACK;
        shoot = _npc.GetComponent<AudioSource>();

    }

    public override void Enter()
    {
        anim2.SetTrigger("isShooting");
        agent2.isStopped = true;
        shoot.Play();
        base.Enter();
    }

    public override void Update()
    {
        Vector3 direction = player2.position - npc2.transform.position;
        float angle = Vector3.Angle(direction, npc2.transform.forward);
        direction.y = 0;

        npc2.transform.rotation = Quaternion.Slerp(npc2.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

        if (!CanAttackPlayer())
        {
            nextState2 = new Idle2(npc2, agent2, anim2, player2);
            stage2 = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim2.ResetTrigger("isShooting");
        shoot.Stop();
        base.Exit();
    }
}
