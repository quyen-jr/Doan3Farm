using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI messages;
    void Start()
    {
        button.onClick.AddListener(() => { DestroyThisNotification(); });
    }

    public void SetNotification(string _messages)
    {
        messages.text = _messages;
    }
    private void DestroyThisNotification()
    {
        Destroy(gameObject);
    }

    
}
