using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private int _waivingID;

    private void Awake()
    {
        _waivingID = Animator.StringToHash("Waiving");
    }

    public void PlayWaivingAnimation()
    {
        _animator.SetTrigger(_waivingID);
    }
}
