using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLogin : MonoBehaviour
{
    [SerializeField] private Button loginBtn;
    [SerializeField] private Button registerBtn;
    void Start()
    {
        loginBtn.onClick.AddListener(() => { UILoginManager.Instance.CreatePanel(1); });
        registerBtn.onClick.AddListener(() => { UILoginManager.Instance.CreatePanel(2); });
    }
}
