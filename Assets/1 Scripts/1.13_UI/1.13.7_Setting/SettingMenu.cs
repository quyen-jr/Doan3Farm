using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private Button _settingBtn;
    [SerializeField] private Button _backToMenuBtn;
    [SerializeField] private Transform _detailtSettingBTn;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BackToMenu()
    {
        _detailtSettingBTn.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
    public void OpenDetailtSetting()
    {
        _detailtSettingBTn.gameObject.SetActive(true);
    }
}
