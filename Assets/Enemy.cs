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
    // Unity ������, ��ü Ŭ������ ����ϴ� ���� �ϳ� ������.
    // ������ �ʰ�

    // 
    // Enter, Update, Exit

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>(true);

        // UniRX,
        // ���� ���
        // ���赵 ������ �� ���õ���, ��Ʈ�������� �� �־��

        // ������Ʈ�� ���Ѻ��� �������� �����,
        // �� �̺�Ʈ�� �����Ѵ�.
        //this.UpdateAsObservable().Subscribe(_ => {
        //    if(animator.GetCurrentAnimatorStateInfo(0).IsName("FindEnemy"))
        //    {
        //        Debug.LogError("I am Find Enemy");
        //        // ���� ã�ƺ�,
        //        animator.SetBool("IsFindEnemy", true);
        //    }
        //}).AddTo(this);

        var animatorStateObserver = animator.gameObject.AddComponent<AnimatorStateObserver>();
        animatorStateObserver.SubscribeAnimatorState(AnimatorState.FindEnemy, () => { }, () => { }, () => { });
        animatorStateObserver.SubscribeAnimatorState(AnimatorState.Locomotion, () => { }, () => { }, () => { });
    }
}
