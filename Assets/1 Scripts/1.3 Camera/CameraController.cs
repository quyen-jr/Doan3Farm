using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    // tpp is also land camera



    private GameObject _currentCamera;
    //public GameObject _target;
    //private Vector3 _oldPos;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _currentCamera =Camera.main.gameObject;
        //_target = Player.LocalPlayer.gameObject;
        //   LandCameraZoom = GetComponent<LandCameraZoom>();
    }
    void Update()
    {
        //if (_target.gameObject.activeSelf)
        //    transform.position = _target.transform.position;
    }
    //public void SetMode(CameraMode mode)
    //{
    //    if (mode == CameraMode.TPP)
    //    {
    //        _currentCamera.SetActive(false);
    //        TPPCamera.gameObject.SetActive(true);
    //        _currentCamera = TPPCamera.gameObject;

    //    }

    //    _previousMode = _currentMode;
    //    _currentMode = mode;
    //}
    // public void SetCameraTarget(GameObject obj) => _target = obj;
    public GameObject GetCurrentCamera() => _currentCamera;
    //public CameraMode GetCurrentMode() => _currentMode;
    //public CameraMode GetPreviousMode() => _previousMode;
   // public Vector3 SetCurrentCameraToOldPos() => _currentCamera.transform.position = _oldPos;
}

