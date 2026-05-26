using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMenu : Menu
{
    public GameObject interactBtn;

    public override void Open() {
        base.Open();
        interactBtn.SetActive(false);
        UIController.Instance.TogglePlayerControl(false);
     //   CameraController.Instance.SetMode(CameraController.CameraMode.Land);
        // CameraController.Instance.SetCameraTarget(Player.LocalPlayer.playerInteractHandler.GetCurrentInteractObj());
        if (Player.LocalPlayer != null) Player.LocalPlayer.playerInputEvent.SwitchActionMap(Player.ActionMap.Land);
    }
    public override void Close()
    {
        base.Close();
        interactBtn.SetActive(false);
        UIController.Instance.TogglePlayerControl(true);
    //    CameraController.Instance.SetMode(CameraController.Instance.GetPreviousMode());
        // CameraController.Instance.SetCameraTarget(Player.LocalPlayer.gameObject);
        // CameraController.Instance.SetCurrentCameraToOldPos();
        if (Player.LocalPlayer != null) Player.LocalPlayer.playerInputEvent.SwitchActionMap(Player.ActionMap.Player);
    }   
    public void ToggleInteractBtn(bool toggle){
        interactBtn.SetActive(toggle);
    }
}


