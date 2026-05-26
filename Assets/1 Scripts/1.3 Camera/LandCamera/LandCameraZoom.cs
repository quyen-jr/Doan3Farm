using UnityEngine;

public class LandCameraZoom : MonoBehaviour
{
    private Vector2 _primaryTouchPos;
    private Vector2 _secondaryTouchPos;
    private Vector2 _previousSecondaryTouchPos = Vector2.zero;
    private float _touchDistance;
    private float _previousTouchDistance;
    public float Distance;
    public float zoomStep;

    void Update()
    {
        //if (CameraController.Instance.GetCurrentMode() == CameraController.CameraMode.Land && _secondaryTouchPos != Vector2.zero) {
        //    UIController.Instance.ToggleLandRightJoystick(false);

        //    _touchDistance = Vector2.Distance(_primaryTouchPos, _secondaryTouchPos);

        //    if (_previousTouchDistance == 0)
        //        _previousTouchDistance = _touchDistance;

        //    if (Mathf.Abs(_touchDistance - _previousTouchDistance) > Distance) {
        //        Vector3 currentPosition = CameraController.Instance.LandCamera.transform.position;
        //        Vector3 dir = CameraController.Instance.LandCamera.transform.forward;

        //        if (_touchDistance > _previousTouchDistance) {
        //            currentPosition += dir * zoomStep;  
        //        } else if (_touchDistance < _previousTouchDistance) {
        //            currentPosition -= dir * zoomStep; 
        //        }

        //        CameraController.Instance.LandCamera.transform.position = currentPosition;
        //        _previousTouchDistance = _touchDistance; 
        //    }
        //} else if (CameraController.Instance.GetCurrentMode() == CameraController.CameraMode.Land && _secondaryTouchPos == Vector2.zero) {
        //    _previousTouchDistance = 0;
        //    Player.LocalPlayer.GetComponent<PlayerLook>().SetDir(Vector3.zero);
        //    UIController.Instance.ToggleLandRightJoystick(true);
        //}
    }

    public void SetPrimaryTouchPos(Vector3 pos) => _primaryTouchPos = pos;

    public void SetSecondaryTouchPos(Vector2 pos)
    {
        if (Vector2.Distance(_previousSecondaryTouchPos, pos) > 0)
        {
            _previousSecondaryTouchPos = _secondaryTouchPos;
            _secondaryTouchPos = pos;
        }
        else
        {
            _secondaryTouchPos = Vector2.zero;
        }
    }

    public Vector2 GetSecondaryTouchPos() => _secondaryTouchPos;
}

