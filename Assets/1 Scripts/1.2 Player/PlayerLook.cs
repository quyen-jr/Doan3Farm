using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private Vector2 _lookDir = Vector2.zero;
    private CameraController _cameraAnchor;
    private GameObject _currentCamera;
    private Player _player;
    private float _currentHorizontalAngle = 0f;
    private float _currentVerticalAngle = 0f;
    private Vector2 _deltaPos;
    private Vector2 _previousTouchPos;
    private Vector2 _currentTouchPos;
    private bool _touchInZone = false;
    private bool _cameraInertia = false;
    private Quaternion horizontalRotation;
    private Quaternion verticalRotation;
    private Vector2 _previousLookDir = Vector2.zero;

    private Vector2 _inertiaDir = Vector2.zero;
    private float _inertiaSpeed;

    public float minMagnitude;
    public float scaleFactor;
    private float inertiaDecay;
    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _cameraAnchor = CameraController.Instance;
        _currentHorizontalAngle = _cameraAnchor.transform.eulerAngles.y;
        _currentVerticalAngle = _cameraAnchor.transform.eulerAngles.x;
        inertiaDecay = 0.1f;
    }

    private void Update()
    {
        if (_lookDir != _previousLookDir)
        {
            _previousLookDir = _lookDir;
        }
        else
        {
            _lookDir = Vector2.zero;
        }



        if (!_cameraAnchor)
        {
            return;
        }

        if (!_cameraAnchor.GetComponent<LandCameraZoom>())
        {
            return;
        }

        if (_cameraAnchor.GetComponent<LandCameraZoom>().GetSecondaryTouchPos() != Vector2.zero)
        {
            return;
        }

        if (_lookDir.magnitude >= minMagnitude || _cameraInertia)
        {

            // Apply input rotation
            if (_lookDir.magnitude >= minMagnitude)
            {
                _currentHorizontalAngle += _lookDir.x * _player.cameraRotateSpeed;
                _currentVerticalAngle -= _lookDir.y * _player.cameraRotateSpeed;
                _currentVerticalAngle = Mathf.Clamp(_currentVerticalAngle, -30f, 30f);

                _inertiaDir = _lookDir;
                _inertiaSpeed = _player.cameraRotateSpeed;
            }

            if (_cameraInertia)
            {
                // Debug.Log("inertia");
                _currentHorizontalAngle += _inertiaDir.x * _inertiaSpeed;
                _currentVerticalAngle -= _inertiaDir.y * _inertiaSpeed;
                _currentVerticalAngle = Mathf.Clamp(_currentVerticalAngle, -30f, 30f);

                _inertiaSpeed *= inertiaDecay;
                if (_inertiaSpeed < 0.01f)
                {
                    _cameraInertia = false;
                    _inertiaDir = Vector2.zero;
                    _lookDir = Vector2.zero;
                }
            }

            horizontalRotation = Quaternion.Euler(0, _currentHorizontalAngle, 0);
            verticalRotation = Quaternion.Euler(_currentVerticalAngle, 0, 0);

            //if (_cameraAnchor.GetCurrentMode() == CameraController.CameraMode.FPP) {
            //    _cameraAnchor.transform.rotation = horizontalRotation;

            //    _currentCamera = _cameraAnchor.GetComponent<CameraController>().GetCurrentCamera();
            //    _currentCamera.transform.localRotation = verticalRotation;
            //    Vector3 currentRotation = _currentCamera.transform.rotation.eulerAngles;
            //    currentRotation.y = 0;
            //    currentRotation.z = 0;
            //    _currentCamera.transform.localRotation = Quaternion.Euler(currentRotation);
            //} else if (_cameraAnchor.GetCurrentMode() == CameraController.CameraMode.TPP) {
            //    _cameraAnchor.transform.rotation = horizontalRotation * verticalRotation;
            //} else if (_cameraAnchor.GetCurrentMode() == CameraController.CameraMode.Land) {
            //    _cameraAnchor.GetCurrentCamera().transform.rotation = horizontalRotation * verticalRotation;
            //}
        }
    }

    public void SetTouchPos(Vector2 pos)
    {
        // Debug.Log("compare: " + pos + " " + _previousTouchPos);
        // Debug.Log("distance: " + Vector3.Distance(pos, _previousTouchPos));
        if (pos != Vector2.zero && _previousTouchPos != Vector2.zero && _touchInZone)
        {
            if (Vector3.Distance(pos, _previousTouchPos) >= 2)
            {
                _deltaPos = _previousTouchPos - pos;
                _previousTouchPos = pos;
                _lookDir = _deltaPos / scaleFactor;
                _cameraInertia = false;
            }
            else
            {
                _lookDir = Vector2.zero;
            }

        }
        else if (pos != Vector2.zero && _previousTouchPos == Vector2.zero && -pos.x >= 0.4f * Screen.width)
        {
            _lookDir = Vector2.zero;
            _previousTouchPos = pos;
            _touchInZone = true;
            _cameraInertia = false;
        }
        else if (pos == Vector2.zero)
        {
            _previousTouchPos = pos;
            _touchInZone = false;

            if (_lookDir.magnitude >= minMagnitude)
            {
                _cameraInertia = true;
            }

            _lookDir = Vector2.zero;
        }
        else
        {
            // Debug.Log("exception: " + pos + " - " + _previousTouchPos);
        }
        _currentTouchPos = pos;
    }
}
