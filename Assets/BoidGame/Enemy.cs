using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public abstract class AIStateBase
{
    //public abstract void Enter();
    //public abstract void Update();
    //public abstract void Exit();

}

public class FindEnemy : AIStateBase
{

}


public class Enemy : MonoBehaviour
{
    public static class AnimatorState
    {
        public static string FindEnemy = "FindEnemy";
        public static string Dead = "Dead";
        public static string Locomotion = "Locomotion";
        public static string Attack = "Attack";
    }

    [SerializeField]
    Animator animator;
    // Unity 지향은, 자체 클래스를 사용하는 것을 꽤나 지향함.
    // 무겁지 않게

    // 
    // Enter, Update, Exit

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>(true);

        // UniRX,
        // 생상성 향상
        // 설계도 좋아질 것 세련되짐, 포트폴리오도 좀 있어보임

        // 업데이트를 지켜보는 옵저버를 만들고,
        // 그 이벤트를 구독한다.
        //this.UpdateAsObservable().Subscribe(_ => {
        //    if(animator.GetCurrentAnimatorStateInfo(0).IsName("FindEnemy"))
        //    {
        //        Debug.LogError("I am Find Enemy");
        //        // 적을 찾아봄,
        //        animator.SetBool("IsFindEnemy", true);
        //    }
        //}).AddTo(this);

        var animatorStateObserver = animator.gameObject.AddComponent<AnimatorStateObserver>();
        animatorStateObserver.SubscribeAnimatorState(AnimatorState.FindEnemy, () => { }, () => { }, () => { });
        animatorStateObserver.SubscribeAnimatorState(AnimatorState.Locomotion, () => { }, () => { }, () => { });
    }
}
