using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

abstract public class EnemyObservable : MonoBehaviour, IObservable<float>
{
    public static System.Action<bool> EnemyCallbackAction;

    public bool OnNotifyAI = false;

    public abstract void Notify(float value);

   /* public IEnumerator NotifyToggleIE()
    {
        yield return new WaitForSeconds(10);
        OnNotifyAI = true;
        yield return null;
    }
   */

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
