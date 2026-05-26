using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINavMenu : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] protected Button _closeBtn;

    public virtual void OnEnable() {
        _closeBtn.onClick.AddListener(CloseMenu);
    }

    public virtual void OnDisable() {
        _closeBtn.onClick.RemoveListener(CloseMenu);
    }

    private void CloseMenu(){
        gameObject.SetActive(false);
        Player.LocalPlayer.playerInputEvent.SwitchActionMapPlayer();
    }

    
}


