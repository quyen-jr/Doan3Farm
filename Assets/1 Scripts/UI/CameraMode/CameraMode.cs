using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMode : Menu, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        _isOpen = !_isOpen;
        menu.SetActive(_isOpen);
    }    
    public void TPPMode(){
      //  CameraController.Instance.SetMode(CameraController.CameraMode.TPP);
    }
    public void FPPMode(){
      //  CameraController.Instance.SetMode(CameraController.CameraMode.FPP);
    }
}
