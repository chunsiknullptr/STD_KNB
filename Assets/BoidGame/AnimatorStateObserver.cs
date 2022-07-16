using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class AnimatorStateObserver : MonoBehaviour
{
    Animator animator;
    ReactiveProperty<int> currentNameHash = new ReactiveProperty<int>();
    ReactiveProperty<int> lastNameHash = new ReactiveProperty<int>();

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animator.FixedUpdateAsObservable().Subscribe(_ =>
        {
            var currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if(currentNameHash.Value != currentStateInfo.shortNameHash)
            {
                lastNameHash.Value = currentNameHash.Value; // keep!! 현재 이름을 마지막으로 기억해줘!!
            }

            // 여기서 변경됨 현재 값이,
            currentNameHash.Value = currentStateInfo.shortNameHash;
        }).AddTo(this);

        // OnEnd 를 어떻게 알것이냐?
        // currentNameHash, 현재 이름이 변경될 때도 알고 있음.
        // 현재 이름이 변경되기 전의 이름도 알 수 있음.        
    }

    public void SubscribeAnimatorState(string stateName, System.Action OnEnter, System.Action OnUpdate, System.Action OnEnd, bool isFixed = true)
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        var sh = Animator.StringToHash(stateName);
        if (OnEnter != null)
        {
            currentNameHash
                .Where(_currentSh => _currentSh == sh)
                .Subscribe(_currentSh => {
                    OnEnter.Invoke();
                    Debug.LogError("On Enter!!! : " + stateName);
                }).AddTo(this);
        }

        if(OnUpdate != null)
        {
            var observable = isFixed ? animator.FixedUpdateAsObservable() : animator.UpdateAsObservable();
            observable.Subscribe(_ =>
            {
                var currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if(currentStateInfo.IsName(stateName))
                {
                    OnUpdate.Invoke();
                    Debug.LogError("On Update!!! : " + stateName);
                }
            }).AddTo(this);
        }

        if(OnEnd != null)
        {
            lastNameHash
                .Where(_lsh=>_lsh == sh)
                .Subscribe(_ => {
                    OnEnd.Invoke();
                    Debug.LogError("On End!!! : " + stateName);
                }).AddTo(this);
        }
    }
}
