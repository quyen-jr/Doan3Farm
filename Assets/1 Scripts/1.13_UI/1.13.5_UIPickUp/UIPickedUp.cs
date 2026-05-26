using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPickedUp : MonoBehaviour
{
    public static event Action OnClickPickedUp;

    [SerializeField] private Button _btnPickedUp;

    private void OnEnable() 
    {
        _btnPickedUp.onClick.AddListener(OnClickButtonPickedUp);    
    }

    private void OnDisable() 
    {
        _btnPickedUp.onClick.RemoveListener(OnClickButtonPickedUp);    
    }

    private void OnClickButtonPickedUp()
    {
        OnClickPickedUp?.Invoke();
    }

    private void OnTriggerPickedUpObject(ETriggerState eTriggerState)
    {
        if(eTriggerState == ETriggerState.enter)
        {
            _btnPickedUp.gameObject.SetActive(true);
        }
        else 
        {
            _btnPickedUp.gameObject.SetActive(false);
        }
    }
}
