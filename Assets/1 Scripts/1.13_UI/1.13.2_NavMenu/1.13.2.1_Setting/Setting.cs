using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : UINavMenu
{
    [Header("Button")]
    [SerializeField] private Button _soundBtn;
    [SerializeField] private Button _musicBtn;
    [SerializeField] private Button _likeBtn;

    [Header("Sound sprites")]
    [SerializeField] private Sprite _sound;
    [SerializeField] private Sprite _muteSound;

    private bool _isPlaySound = true;

    public override void OnEnable() {
        base.OnEnable();
        _soundBtn.onClick.AddListener(ToggleSound);
        
    }
    public override void OnDisable() {
        base.OnDisable();
        _soundBtn.onClick.RemoveListener(ToggleSound);
    }
    private void ToggleSound(){
        _isPlaySound = !_isPlaySound;

        if(_isPlaySound){
            _soundBtn.gameObject.GetComponentsInChildren<Image>()[1].sprite = _sound;
            AudioListener.volume = 1;
        } else {
            _soundBtn.gameObject.GetComponentsInChildren<Image>()[1].sprite = _muteSound;
            AudioListener.volume = 0;
        }
    }
}
