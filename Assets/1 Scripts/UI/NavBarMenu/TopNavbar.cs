using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopNavbar : MonoBehaviour
{
    [SerializeField] private Button _settingBtn;
    [SerializeField] private Transform _settingMenu;

    [SerializeField] private Button _UIBagBtn;
    void Start()
    {
        
    }
    private void OnEnable() {
        _settingBtn.onClick.AddListener(OpenSettingMenu);
        _UIBagBtn.onClick.AddListener(OpenBagMenu);
    }
    private void OnDisable() {
        _settingBtn.onClick.RemoveListener(OpenSettingMenu);
        _UIBagBtn?.onClick.RemoveListener(OpenBagMenu);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenSettingMenu(){
        _settingMenu.gameObject.SetActive(true);
    }

    public void OpenBagMenu()
    {
        Debug.Log("hahaha");
        ScreenGameManager.instance.Open<BagUIScreen>();
    }
}
