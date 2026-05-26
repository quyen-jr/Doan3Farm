using UnityEngine;

public class LandCamera : MonoBehaviour
{
    private CameraController _cameraController;
    private void Awake()
    {
        _cameraController = GetComponent<CameraController>();
    }
    void Update()
    {

    }
}
