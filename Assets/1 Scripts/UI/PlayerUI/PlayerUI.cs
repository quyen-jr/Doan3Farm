using FairyField.Logic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    void Start()
    {
        SetName();
    }

    private void SetName()
    {
        if (UserData.instance != null)
            playerName.text = UserData.instance.GetUsername();
        if (playerName.text == null) playerName.text = "MI123";
    }
}
