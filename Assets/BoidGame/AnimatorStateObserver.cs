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
                lastNameHash.Value = currentNameHash.Value; // keep!! ���� �̸��� ���������� �������!!
            }

            // ���⼭ ����� ���� ����,
            currentNameHash.Value = currentStateInfo.shortNameHash;
        }).AddTo(this);

        // OnEnd �� ��� �˰��̳�?
        // currentNameHash, ���� �̸��� ����� ���� �˰� ����.
        // ���� �̸��� ����Ǳ� ���� �̸��� �� �� ����.        
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
