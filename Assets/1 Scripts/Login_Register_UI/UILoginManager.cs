using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoginManager : MonoBehaviour
{
    public static UILoginManager Instance;
    [SerializeField] private Transform parentObjectPanel;
    [SerializeField] private Transform backgroundGroup;
    [SerializeField] private GameObject[] menuUIPrefab;

    [SerializeField] private GameObject authencationObjectPrefab;
    [SerializeField] private GameObject notificationObjectPrefab;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        CreatePanel(0);
    }
    public void CreatePanel(int index)
    {
        Transform[] uiContentChildren = parentObjectPanel.GetComponentsInChildren<Transform>(true);
        foreach (Transform uiContent in uiContentChildren)
        {
            if (uiContent != parentObjectPanel.transform )
            {
                Destroy(uiContent.gameObject);
            }
        }
        // instantiae new ui object
        GameObject uiObject = Instantiate(menuUIPrefab[index], parentObjectPanel.transform);

    }
    public void CreateNotification(string message)
    {
        GameObject notificationObject= Instantiate(notificationObjectPrefab,parentObjectPanel.transform);
        notificationObject.GetComponent<Notification>().SetNotification(message);
    }
    public void CreateAuthencationPanel(string email)
    {
        GameObject authencationOBJ = Instantiate(authencationObjectPrefab, parentObjectPanel.transform);
        authencationOBJ.GetComponent<Authencation>().SetEmailTitle(email);
      // notificationObject.GetComponent<Notification>().SetNotification(_message);
    }

    public void OpenKeyBoard() {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false);
    }
}
