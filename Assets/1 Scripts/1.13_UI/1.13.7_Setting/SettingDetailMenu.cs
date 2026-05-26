using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingDetailMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform _deletePanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisableDetailMenu(){
        if(_deletePanel.gameObject.activeSelf) return;
        this.gameObject.SetActive(false);
    }
}
