using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputEvent : MonoBehaviour
{
    public Vector2 _touchPos;
    public Vector2 _secondaryTouchPos = Vector2.zero;

    private Player player;
    private PlayerInput _playerInput;
    private PhotonView _photonView;

    public Vector2 _tapPos;

    private bool _isPrimaryPressing = false;
    private bool _isSecondaryPressing = false;
    private bool _isJoystickPressing = false;

    private Vector2 _UITouchPos;

    [Header("Cinemachine Camera")]
    public CinemachineFreeLook freeLookCamera;

    [Header("Camera Touch Area")]
    [Range(0f, 1f)]
    public float rightScreenStart = 0.5f;

    [Header("Camera Sensitivity")]
    public float cameraSensitivityX = 0.18f;
    public float cameraSensitivityY = 0.003f;

    private Vector2 _lastPrimaryTouchPos;
    private Vector2 _lastSecondaryTouchPos;
    private bool _hasLastPrimaryTouch = false;
    private bool _hasLastSecondaryTouch = false;

    private void Awake()
    {
        player = GetComponent<Player>();
        _playerInput = GetComponent<PlayerInput>();
        _photonView = GetComponent<PhotonView>();

        // Nếu là player của người khác thì tắt input trên máy mình
        if (!IsLocalPlayer())
        {
            if (_playerInput != null)
            {
                _playerInput.enabled = false;
            }

            enabled = false;
            return;
        }
    }

    private bool IsLocalPlayer()
    {
        // Nếu không có PhotonView thì cho chạy offline bình thường
        return _photonView == null || _photonView.IsMine;
    }

    // player movement
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer()) return;

        if (context.performed)
        {
            _isJoystickPressing = true;

            if (player != null && player.playerMovement != null)
            {
                player.playerMovement.SetDir(context.ReadValue<Vector2>());
            }
        }
        else if (context.canceled)
        {
            _isJoystickPressing = false;

            if (player != null && player.playerMovement != null)
            {
                player.playerMovement.SetDir(Vector2.zero);
            }
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer()) return;

        // Không cần dùng nếu camera xoay bằng touch nửa phải màn hình
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer()) return;

        if (context.performed)
        {
            if (player != null && player.playerInteract != null)
            {
                Vector2 finalInteractPos = Vector2.zero;

                // CÁCH CHẮC CHẮN NHẤT TRÊN PC: Kiểm tra xem chuột trái có đang được nhấn giữ/click không
                if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                {
                    finalInteractPos = Mouse.current.position.ReadValue();
                }

                // Nếu không phải do click chuột (hoặc đang chạy trên Mobile), lấy vị trí Tap
                if (finalInteractPos == Vector2.zero)
                {
                    finalInteractPos = _tapPos;
                }

                // Nếu có vị trí hợp lệ thì mới bắn lệnh tương tác
                if (finalInteractPos != Vector2.zero)
                {
                    player.playerInteract.Interact(finalInteractPos);
                }
            }
        }
    }

    public void TouchPosition(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer()) return;

        _touchPos = context.ReadValue<Vector2>();

        if (!context.performed)
            return;

        if (!_isPrimaryPressing)
            return;

        // Chỉ xoay camera khi touch nằm ở nửa phải màn hình
        if (_touchPos.x < Screen.width * rightScreenStart)
        {
            _hasLastPrimaryTouch = false;
            return;
        }

        if (!_hasLastPrimaryTouch)
        {
            _lastPrimaryTouchPos = _touchPos;
            _hasLastPrimaryTouch = true;
            return;
        }

        Vector2 delta = _touchPos - _lastPrimaryTouchPos;
        RotateFreeLookCamera(delta);

        _lastPrimaryTouchPos = _touchPos;
    }

    public void SecondaryTouchPosition(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer()) return;

        _secondaryTouchPos = context.ReadValue<Vector2>();

        if (!context.performed)
            return;

        if (!_isSecondaryPressing)
            return;

        // Ngón thứ 2 cũng chỉ xoay nếu nằm bên phải màn hình
        if (_secondaryTouchPos.x < Screen.width * rightScreenStart)
        {
            _hasLastSecondaryTouch = false;
            return;
        }

        if (!_hasLastSecondaryTouch)
        {
            _lastSecondaryTouchPos = _secondaryTouchPos;
            _hasLastSecondaryTouch = true;
            return;
        }

        Vector2 delta = _secondaryTouchPos - _lastSecondaryTouchPos;
        RotateFreeLookCamera(delta);

        _lastSecondaryTouchPos = _secondaryTouchPos;
    }

    private void RotateFreeLookCamera(Vector2 delta)
    {
        if (!IsLocalPlayer()) return;

        if (freeLookCamera == null)
            return;

        // X: xoay trái phải
        freeLookCamera.m_XAxis.Value += delta.x * cameraSensitivityX;

        // Y: xoay lên xuống
        freeLookCamera.m_YAxis.Value -= delta.y * cameraSensitivityY;

        // Y Axis của FreeLook thường nằm từ 0 đến 1
        freeLookCamera.m_YAxis.Value = Mathf.Clamp01(freeLookCamera.m_YAxis.Value);
    }

    public void OnPrimaryPress(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer()) return;

        if (context.performed)
        {
            _isPrimaryPressing = true;
            _hasLastPrimaryTouch = false;
        }
        else if (context.canceled)
        {
            _isPrimaryPressing = false;
            _touchPos = Vector2.zero;
            _hasLastPrimaryTouch = false;

            if (player != null && player.playerLook != null)
            {
                player.playerLook.SetTouchPos(Vector2.zero);
            }
        }
    }

    public void OnSecondaryPress(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer()) return;

        if (context.performed)
        {
            _isSecondaryPressing = true;
            _hasLastSecondaryTouch = false;
        }
        else if (context.canceled)
        {
            _isSecondaryPressing = false;
            _secondaryTouchPos = Vector2.zero;
            _hasLastSecondaryTouch = false;

            if (player != null && player.playerLook != null)
            {
                player.playerLook.SetTouchPos(Vector2.zero);
            }
        }
    }

    public void SwitchActionMap(Player.ActionMap actionMap)
    {
        if (!IsLocalPlayer()) return;

        if (_playerInput != null)
        {
            _playerInput.SwitchCurrentActionMap(actionMap.ToString());
        }
    }

    public void SwitchActionMapPlayer()
    {
        SwitchActionMap(Player.ActionMap.Player);
    }

    public void SwitchActionMapMenu()
    {
        SwitchActionMap(Player.ActionMap.Menu);
    }

    public void TapPos(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer()) return;

        if (context.performed)
        {
            _tapPos = context.ReadValue<Vector2>();
        }
    }

    // menu
    public void OnUITouchPos(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer()) return;

        _UITouchPos = context.ReadValue<Vector2>();
    }

    public Vector2 GetTouchPos() => _touchPos;

    public Vector2 GetUITouchPos() => _UITouchPos;
}