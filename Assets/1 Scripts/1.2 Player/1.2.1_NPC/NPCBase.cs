using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : MonoBehaviour, IPlayerCanTouchable
{
    private Outline _outline;
    private NPCAnimation _nPCAnimation;

    private bool _canWaiving = true;
    private bool _isHighLight;

    public virtual void Awake()
    {
        _nPCAnimation = GetComponent<NPCAnimation>();
        _outline = GetComponent<Outline>();
    }

    public virtual void TurnOnHighLight()
    {
        if(_isHighLight) return;
        _isHighLight = true;
        _outline.enabled = true;
        _outline.OutlineMode = Outline.Mode.OutlineVisible;
    }

    public virtual void TurnOffHighLight()
    {
        if(_isHighLight == false) return;

        _isHighLight = false;
        _outline.enabled = false;
    }

    public void Waiving()
    {
        if(_canWaiving)
        {
            _nPCAnimation.PlayWaivingAnimation();
            _canWaiving = false;
            StartCoroutine(CountDownWaivingAgain());
        }
    }

    private void Update() 
    {
        if(_isHighLight)
        {
            Transform player = Player.LocalPlayer.transform;
            Vector3 direction = player.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            if(transform.rotation != targetRotation)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
            else 
            {
                Waiving();
            }
        }        
    }
    
    private IEnumerator CountDownWaivingAgain()
    {
        yield return new WaitForSeconds(10f);

        _canWaiving = true;
    }

    public Vector3 GetPosition() => transform.position;

    public virtual void OnPlayerTouch() {}
}

