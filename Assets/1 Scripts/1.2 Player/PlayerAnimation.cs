using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerAnimation
{
    picked_up,
}

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private string[] triggers;

    [Header("Animation name")]
    [SerializeField] private string _pickedUpAnimationName;

//PRIVATE VARIABLE
    private Dictionary<EPlayerAnimation,AnimationClip> _dictAnimationClip;
    

    private bool _locking = false;
    private Player _player;
    private Animator _animator;

    private int _velocityHashID;

    #region Initialize
    private void Awake()
    {
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
        AttachAniamtionClip();

        GetAllHashID();
    }

    private void AttachAniamtionClip()
    {
        _dictAnimationClip = new Dictionary<EPlayerAnimation, AnimationClip>();

        _dictAnimationClip.Add(EPlayerAnimation.picked_up,GetAnimationLenght(_pickedUpAnimationName));
    }

    private void GetAllHashID()
    {
        _velocityHashID = Animator.StringToHash("Velocity");
    }
    #endregion
    public void PlayPickedUpAnimation(Action onAnimationDone)
    {
        StartCoroutine(IEPickedUp(onAnimationDone));
    }

    private IEnumerator IEPickedUp(Action onAnimationDone)
    {
        SetPickedUpTrigger();
        yield return new WaitForSeconds(GetAnimationClip(EPlayerAnimation.picked_up).length);
        onAnimationDone?.Invoke();
    }

    public void SetVelocityAnim(float velocity)
    {
//        Debug.Log(velocity);
        SetFloat(_velocityHashID, velocity);
    }

    public void SetAnimBool(string id, bool value)
    {
        if (!_locking)
        {
            _animator.SetBool(id, value);
        }
    }

    public void SetAnimTrigger(string id)
    {
        if (!_locking)
        {
            ClearTrigger("Jump");
            _animator.SetTrigger(id);
        }
    }
    #region  set each trigger anim
    public void SetWalkTrigger()
    {
        SetAnimTrigger("Walk");
    }
    public void ClearWalkTrigger()
    {
        ClearTrigger("Walk");
    }
    public void SetSitTrigger()
    {
        SetAnimTrigger("Sit");
    }
    public void ClearSitTrigger()
    {
        ClearTrigger("Sit");
    }
    public void SetIdleTrigger()
    {
        SetAnimTrigger("Idle");
    }
    public void ClearIdleTrigger()
    {
        ClearTrigger("Idle");
    }
    public void SetJumpTrigger()
    {
        SetAnimTrigger("Jump");
    }
    public void ClearJumpTrigger()
    {
        ClearTrigger("Jump");
    }

    public void SetPickedUpTrigger()
    {
        SetAnimTrigger("PickedUp");
    }

    public void ClearPickedUpTrigger()
    {
        ClearTrigger("PickedUp");
    }

    #endregion
    public void SetFloat(string id, float value)
    {
        _animator.SetFloat(id, value);
    }

    private void SetFloat(int id, float value)
    {
        _animator.SetFloat(id, value);
    }
    public void SetIdleImmediatly()
    {
        GetAnimator().Play("Idle", 0, 0);
    }

    public bool CheckCurrentAnim(string id) => _animator.GetCurrentAnimatorStateInfo(0).IsName(id);
    public string GetCurrentAnimName()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        string currentAnimationName = stateInfo.IsTag("tag") ? "TaggedAnimation" : _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        return currentAnimationName;
    }

    public void ClearAllTrigger()
    {
        //  Debug.Log("reset trigger");
        foreach (string trigger in triggers)
        {
            _animator.ResetTrigger(trigger);
        }
    }

    public AnimationClip GetAnimationLenght(string animationName)
    {
        if (_animator != null)
        {
            RuntimeAnimatorController controller = _animator.runtimeAnimatorController;
            foreach (AnimationClip clip in controller.animationClips)
            {
                if (clip.name == animationName)
                {
                    return clip;
                }
            }

            return null;
        }
        else
        {
            return null;
        }
    }

    // lock until animation is done
    public void LockTransition() => _locking = true;
    public void UnlockTransition() => _locking = false;
    public bool IsLockingTransition() => _locking;
    public void ClearTrigger(string name) => _animator.ResetTrigger(name);
    public Animator GetAnimator() => _animator;
    public AnimationClip GetAnimationClip(EPlayerAnimation ePlayerAnimation) => _dictAnimationClip[ePlayerAnimation];
    public void ResetAnimator()
    {
        _animator = GetComponent<Animator>();
    }
    public void SetAnimator(Animator animator)
    {
        _animator = animator;
    }
}
