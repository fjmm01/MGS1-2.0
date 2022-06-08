using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

abstract public class EnemyObservable : MonoBehaviour, IObservable<bool>
{
    public static System.Action<bool> EnemyCallbackAction;

    public abstract void Notify(bool value);

}

//class EnemyA: EnemyObserver
//{

//    override notify()
//    {
//        this.Pursue();
//    }

//    void Attack()
//    {
//        if(canattackPlayer == true)
//        {
//            EnemyObserver.myAction.Invoke();
//        }
//    }
//}

//class EnemyB : EnemyObserver
//{
//    override notify()
//    {
//        this.Pursue();
//    }

//    void Attack()
//    {
//        if (canattackPlayer == true)
//        {
//            EnemyObserver.myAction.Invoke();
//        }
//    }
//}
