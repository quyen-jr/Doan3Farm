using System.Collections;
using Photon.Pun;
using UnityEngine;

public enum ECharacterMoveState
{
    move_by_joystick,
    move_to_target,
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Config parameters")]
    [SerializeField] private float _accelerationStart;
    [SerializeField] private float _accelerationStop;
    [SerializeField] private float _maxVelocity;

    private ECharacterMoveState _eCharacterMoveState { get; set; }

    private float _currentVelocity;
    private float _currentAcceleration;
    public float CurrentVelocity => _currentVelocity;

    private Vector3 _distancePerSecond;
    private bool _isMoving = false;
    public bool _isGrounded = true;

    private Vector3 _moveDir = Vector3.zero;
    private Vector3 _inputDir = Vector3.zero;
    private Vector3 _velocity;

    private CharacterController _characterController;
    private Rigidbody _rb;
    private Player player;
    private PhotonView _photonView;

    private bool isBusyDoingAction;
    private bool isJumping = false;
    private bool firstTimeTouchingJT = true;

    private float _rotationSpeed = 8f;
    private float _currentMoveParam = 0.5f;
    private int groundCheckLayer;

    private Transform smallPlotHasClickToMove;
    private SmallPlot nearestSmallPlot;
    private Vector3 stillInThisSmallPlot;

    private bool isCanOpenCircleUIEarly;
    private bool isTimeDoingAction;
    private bool canDoAction;
    private bool isOneTimeMoveBeforeHaverst;

    private void Awake()
    {
        player = GetComponent<Player>();
        _characterController = GetComponent<CharacterController>();
        _rb = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();

        // Nếu là player của người khác thì không chạy movement local trên máy mình
        if (!IsLocalPlayer())
        {
            enabled = false;
            return;
        }
    }

    private bool IsLocalPlayer()
    {
        // Nếu không có PhotonView thì vẫn chạy được offline
        return _photonView == null || _photonView.IsMine;
    }

    private void Update()
    {
        if (!IsLocalPlayer()) return;

        ApplyGravity();

        CheckUseJoyStick();

        switch (_eCharacterMoveState)
        {
            case ECharacterMoveState.move_by_joystick:
                MoveByJoyStick();
                break;

            case ECharacterMoveState.move_to_target:
                if (smallPlotHasClickToMove == null)
                {
                    _eCharacterMoveState = ECharacterMoveState.move_by_joystick;
                    return;
                }

                if (IsMoveTo(smallPlotHasClickToMove.position))
                {
                    _eCharacterMoveState = ECharacterMoveState.move_by_joystick;
                }

                break;
        }
    }

    private void ApplyGravity()
    {
        if (_characterController == null) return;

        groundCheckLayer = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Ignore Raycast"));

        _isGrounded = Physics.OverlapSphere(transform.position, 0.2f, groundCheckLayer).Length > 0;

        if (_isGrounded)
        {
            if (_velocity.y < 0f)
            {
                _velocity.y = -2f;
            }

            if (isJumping)
            {
                isJumping = false;

                if (player != null && player.playerAnimation != null)
                {
                    if (IsMoving())
                    {
                        player.playerAnimation.SetWalkTrigger();
                    }
                    else
                    {
                        player.playerAnimation.SetIdleTrigger();
                    }
                }
            }
        }
        else
        {
            if (player == null) return;

            _velocity.y += player.gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }

    private void CheckUseJoyStick()
    {
        if (player == null || player.playerAnimation == null) return;

        if (_inputDir.magnitude >= 0.1f && !player.playerAnimation.IsLockingTransition())
        {
            ResetValueAfterMoveToLandPlotHasClick();
        }
    }

    private void MoveByJoyStick()
    {
        if (player == null || player.playerAnimation == null) return;

        if (!IsbusyDoingAction())
        {
            UpdatePosition_JoyStick(_inputDir);
        }

        if (_inputDir.magnitude >= 0.1f && !player.playerAnimation.IsLockingTransition())
        {
            UpdateRotation_JoyStick(_inputDir);

            stillInThisSmallPlot = Vector3.negativeInfinity;
            ResetValueAfterMoveToLandPlotHasClick();

            CancleActionWhenUseJoyStick();
        }
        else
        {
            _moveDir = Vector3.zero;

            if (!isBusyDoingAction && _eCharacterMoveState != ECharacterMoveState.move_to_target)
            {
                _isMoving = false;
            }

            firstTimeTouchingJT = true;
        }
    }

    private void UpdateRotation_JoyStick(Vector2 inputDirection)
    {
        if (CameraController.Instance == null) return;

        GameObject currentCamera = CameraController.Instance.GetCurrentCamera();

        if (currentCamera == null) return;

        _moveDir = currentCamera.transform.right * _inputDir.x + currentCamera.transform.forward * _inputDir.y;
        _moveDir.y = 0;

        if (_moveDir == Vector3.zero) return;

        float targetRotation = Mathf.Atan2(_moveDir.x, _moveDir.z) * Mathf.Rad2Deg;

        Quaternion currentRotation = transform.rotation;
        Quaternion desiredRotation = Quaternion.Euler(0, targetRotation, 0);

        transform.rotation = Quaternion.Slerp(currentRotation, desiredRotation, Time.deltaTime * _rotationSpeed);
    }

    private void UpdatePosition_JoyStick(Vector2 inputDirection)
    {
        UpdateAcceleration((inputDirection == Vector2.zero) ? _accelerationStop : _accelerationStart);
    }

    private void UpdateAcceleration(float newAccerleration)
    {
        _currentAcceleration = newAccerleration;
        UpdateVelocity(CurrentVelocity + newAccerleration * Time.deltaTime, transform.forward);
    }

    private void UpdateVelocity(float newVelocity, Vector3 direction)
    {
        if (_characterController == null) return;

        if (newVelocity > 0f)
        {
            _currentVelocity = Mathf.Clamp(newVelocity, 0, _maxVelocity);
            _isMoving = true;

            // Only apply movement here when a direction is provided.
            // Some move-to-target paths already move by CharacterController.Move(...)
            // and call UpdateVelocity only to drive animation speed.
            if (direction != Vector3.zero)
            {
                _distancePerSecond = _currentVelocity * Time.deltaTime * direction;
                _characterController.Move(_distancePerSecond);
            }
        }
        else
        {
            _currentVelocity = 0f;
            _isMoving = false;
        }

        if (player != null && player.playerAnimation != null)
        {
            player.playerAnimation.SetVelocityAnim(_currentVelocity);
        }
    }

    public void CancleActionWhenUseJoyStick()
    {
        SetBusyDoingAction(false);

        if (UIController.Instance != null)
        {
            if (UIController.Instance.GetCurrentSelectedLandPlot() != null)
            {
                UIController.Instance.GetCurrentSelectedLandPlot().ResetAllAndEnablePlayerMovement();
            }

            UIController.Instance.SetCurrentSelectedLandPlot(null);
            UIController.Instance.SetCurrentSelectedSmallPlot(null);
            UIController.Instance.ToggleCircleUI(false);
        }

        ResetValueAfterMoveToLandPlotHasClick();
        ResetActionValue();
    }

    public void SetMoveToTarget(Transform _target)
    {
        if (!IsLocalPlayer()) return;

        if (_target != null)
        {
            if (stillInThisSmallPlot != Vector3.negativeInfinity)
            {
                if (_target.position == stillInThisSmallPlot)
                {
                    if (UIController.Instance != null)
                    {
                        UIController.Instance.SetCurrentSelectedLandPlot(_target.GetComponentInParent<LandPlot>());
                        UIController.Instance.SetCurrentSelectedSmallPlot(_target.GetComponent<SmallPlot>());
                        ResetValueAfterMoveToLandPlotHasClick();
                        UIController.Instance.ToggleCircleUI(true);
                    }

                    return;
                }
            }

            smallPlotHasClickToMove = _target;
            _eCharacterMoveState = ECharacterMoveState.move_to_target;
            _isMoving = true;
        }
    }

    #region Is Move To Func

    private Vector3 GetNearestSmallPlotPositionOffset(Vector3 _targetPlotPos)
    {
        Vector3 offsetLeft = _targetPlotPos - new Vector3(0, 0, 0.73f);
        Vector3 offsetRight = _targetPlotPos + new Vector3(0, 0, 0.73f);

        if (stillInThisSmallPlot != Vector3.negativeInfinity && _eCharacterMoveState != ECharacterMoveState.move_to_target)
        {
            if (stillInThisSmallPlot != _targetPlotPos)
            {
                stillInThisSmallPlot = Vector3.negativeInfinity;
            }
        }

        if (UIController.Instance != null && UIController.Instance.GetCurrentSelectedLandPlot() != null)
        {
            if (UIController.Instance.GetCurrentSelectedLandPlot().GetCurrentActionType() == LandPlot.ActionType.Haverst)
            {
                offsetLeft = _targetPlotPos - new Vector3(0, 0, 0.45f);
                offsetRight = _targetPlotPos + new Vector3(0, 0, 0.45f);

                _eCharacterMoveState = ECharacterMoveState.move_by_joystick;
                stillInThisSmallPlot = Vector3.zero;
            }
        }

        if (_eCharacterMoveState == ECharacterMoveState.move_to_target)
        {
            if (nearestSmallPlot == null && smallPlotHasClickToMove != null)
            {
                nearestSmallPlot = smallPlotHasClickToMove.GetComponent<SmallPlot>();
            }

            offsetLeft = _targetPlotPos - new Vector3(0, 0, 0.72f);
            offsetRight = _targetPlotPos + new Vector3(0, 0, 0.72f);
        }

        float distanceToLeft = Vector3.Distance(transform.position, offsetLeft);
        float distanceToRight = Vector3.Distance(transform.position, offsetRight);

        Vector3 nearestDistance = distanceToLeft < distanceToRight ? offsetLeft : offsetRight;
        return nearestDistance;
    }

    public float GetStopDistanceWhenMoveToPlot()
    {
        float stopDistance = 0.7f;

        if (UIController.Instance != null)
        {
            if (UIController.Instance.GetCurrentSelectedLandPlot()?.GetCurrentActionType() == LandPlot.ActionType.Haverst ||
                _eCharacterMoveState == ECharacterMoveState.move_to_target)
            {
                stopDistance = 0.1f;
            }
        }

        return stopDistance;
    }

    public bool IsMoveTo(Vector3 _targetPos)
    {
        Vector3 newTargetPos = GetNearestSmallPlotPositionOffset(_targetPos);

        if (stillInThisSmallPlot == _targetPos && _eCharacterMoveState != ECharacterMoveState.move_to_target)
        {
            stillInThisSmallPlot = Vector3.zero;
            return true;
        }

        Vector3 offsetDirection = (newTargetPos - transform.position).normalized;
        Vector3 targetDirection = _targetPos - transform.position;

        float distanceToTarget = Vector3.Distance(transform.position, newTargetPos);
        float stopDistance = GetStopDistanceWhenMoveToPlot();
        float targetAngleValue = 0.5f;
        float distanceToReduceSpeed = 0.5f;

        bool isDoActionAfterReachTarget = (_eCharacterMoveState == ECharacterMoveState.move_to_target) ? false : true;
        bool isDecreaseSpeed = (distanceToTarget < distanceToReduceSpeed && isDoActionAfterReachTarget == false) ? true : false;

        MoveCharacterWhenGoToTarget(offsetDirection, distanceToTarget, stopDistance, isDecreaseSpeed);
        CheckCanOpenCircleMenuEarly(distanceToTarget, stopDistance);

        float rotationDifference = GetDiffrentWhenRotatePlayer(offsetDirection, targetDirection, distanceToTarget, targetAngleValue, isDecreaseSpeed);

        CheckDistanceToPrePareAction(distanceToTarget, isDoActionAfterReachTarget);

        if (distanceToTarget < stopDistance && rotationDifference < targetAngleValue)
        {
            if (canDoAction && isDoActionAfterReachTarget)
            {
                UpdateVelocity(0, Vector3.zero);
                ResetActionValue();

                if (UIController.Instance != null &&
                    UIController.Instance.GetCurrentSelectedLandPlot()?.GetCurrentActionType() != LandPlot.ActionType.Haverst)
                {
                    stillInThisSmallPlot = _targetPos;
                }
                else
                {
                    stillInThisSmallPlot = Vector3.zero;
                }

                return true;
            }

            if (isDoActionAfterReachTarget == false)
            {
                UpdateVelocity(0, Vector3.zero);

                if (!isCanOpenCircleUIEarly)
                {
                    if (nearestSmallPlot != null)
                    {
                        stillInThisSmallPlot = nearestSmallPlot.transform.position;
                    }

                    EnableCircleUI();
                }

                isCanOpenCircleUIEarly = false;
                ResetValueAfterMoveToLandPlotHasClick();

                return true;
            }
        }

        return false;
    }

    private void CheckDistanceToPrePareAction(float _distanceToTarget, bool _isDoActionAfterReachTarget)
    {
        float distaceToPrepareIfAction = 1f;

        if (_distanceToTarget <= distaceToPrepareIfAction && _isDoActionAfterReachTarget)
        {
            if (UIController.Instance != null &&
                UIController.Instance.GetCurrentSelectedLandPlot()?.GetCurrentActionType() != LandPlot.ActionType.Haverst)
            {
                UpdateVelocity(0, Vector3.zero);
            }

            if (!isTimeDoingAction &&
                UIController.Instance != null &&
                UIController.Instance.GetCurrentSelectedLandPlot()?.GetCurrentActionType() != LandPlot.ActionType.Haverst)
            {
                StartCoroutine(SetIdlePrepareForAction());
                isTimeDoingAction = true;
            }
        }
    }

    private void ResetActionValue()
    {
        canDoAction = false;
        isTimeDoingAction = false;
        isOneTimeMoveBeforeHaverst = false;
    }

    private float GetDiffrentWhenRotatePlayer(
        Vector3 _offsetDirection,
        Vector3 _targetDirection,
        float _distanceToTarget,
        float _targetAngleValue,
        bool _isdecreaseSpeed)
    {
        float distanceToRotatePlot = 0.5f;
        float distanceToRotatePlotWhenAction = 1.9f;

        if (_offsetDirection == Vector3.zero)
        {
            return 0f;
        }

        Quaternion targetRotation = Quaternion.LookRotation(_offsetDirection);

        if (_eCharacterMoveState == ECharacterMoveState.move_to_target && _distanceToTarget < distanceToRotatePlot)
        {
            if (_targetDirection != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(_targetDirection);
            }
        }

        if (_distanceToTarget < distanceToRotatePlotWhenAction && _eCharacterMoveState != ECharacterMoveState.move_to_target)
        {
            if (_targetDirection != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(_targetDirection);
            }
        }

        targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        float rotationDifference = Quaternion.Angle(transform.rotation, targetRotation);

        if (rotationDifference > _targetAngleValue)
        {
            if (!_isdecreaseSpeed)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
            else
            {
                float amountToReduce = 1.5f;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed / amountToReduce);
            }
        }

        return rotationDifference;
    }

    #endregion

    #region MoveCharacter

    private void MoveCharacterWhenGoToTarget(Vector3 _offsetDirection, float _distanceToTarget, float _stopDistance, bool _isdecreaseSpeed)
    {
        bool isDoActionAfterReachTarget = (_eCharacterMoveState == ECharacterMoveState.move_to_target) ? false : true;

        if (_distanceToTarget >= _stopDistance)
        {
            if (isDoActionAfterReachTarget == false)
            {
                CaculateSpeedAndMovePlayer(_offsetDirection, _isdecreaseSpeed);
                CanStopBeforeReachTarget(_distanceToTarget, _stopDistance);
            }
            else
            {
                if (UIController.Instance != null &&
                    UIController.Instance.GetCurrentSelectedLandPlot()?.GetCurrentActionType() != LandPlot.ActionType.Haverst)
                {
                    MovePlayerAndDoAction(_offsetDirection, _distanceToTarget);
                }
                else
                {
                    MoveOneTimeBeforeHavest(_offsetDirection);
                }
            }
        }
    }

    private void MoveOneTimeBeforeHavest(Vector3 _offsetDirection)
    {
        if (_characterController == null || player == null) return;

        _characterController.Move(_offsetDirection * Time.deltaTime * (player.moveSpeed / 6f));

        if (!isOneTimeMoveBeforeHaverst)
        {
            UpdateVelocity(0.6f, Vector3.zero);
            isOneTimeMoveBeforeHaverst = true;
        }

        StartCoroutine(SetHaverstAction());
    }

    private void MovePlayerAndDoAction(Vector3 _offsetDirection, float _distanceToTarget)
    {
        if (_characterController == null || player == null) return;

        _characterController.Move(_offsetDirection * Time.deltaTime * player.moveSpeed);

        float distacnceToStop = 0.9f;

        if (_distanceToTarget <= distacnceToStop)
        {
            UpdateVelocity(0, Vector3.zero);
        }
        else
        {
            UpdateVelocity(0.6f, Vector3.zero);
        }
    }

    private void CanStopBeforeReachTarget(float _distanceToTarget, float _stopDistance)
    {
        float distanceToStop = _stopDistance * 2;

        if (_distanceToTarget <= distanceToStop)
        {
            UpdateVelocity(0, Vector3.zero);
        }
        else
        {
            UpdateVelocity(0.6f, Vector3.zero);
        }
    }

    private void CaculateSpeedAndMovePlayer(Vector3 _offsetDirection, bool _isdecreaseSpeed)
    {
        if (_characterController == null || player == null) return;

        if (!_isdecreaseSpeed)
        {
            _characterController.Move(_offsetDirection * Time.deltaTime * player.moveSpeed);
        }
        else
        {
            float amountToReduce = 1.3f;
            _characterController.Move(_offsetDirection * Time.deltaTime * player.moveSpeed / amountToReduce);
        }
    }

    private IEnumerator SetHaverstAction()
    {
        yield return new WaitForSeconds(0.5f);

        UpdateVelocity(0, Vector3.zero);
        canDoAction = true;
    }

    #endregion

    public void CancleActionMoveToPlot()
    {
        _eCharacterMoveState = ECharacterMoveState.move_by_joystick;
        isCanOpenCircleUIEarly = false;
        isOneTimeMoveBeforeHaverst = false;

        ResetValueAfterMoveToLandPlotHasClick();
    }

    private void CheckCanOpenCircleMenuEarly(float _distanceToTarget, float _stopDistance)
    {
        if (_distanceToTarget < _stopDistance &&
            !isCanOpenCircleUIEarly &&
            _eCharacterMoveState == ECharacterMoveState.move_to_target)
        {
            isCanOpenCircleUIEarly = true;

            if (nearestSmallPlot != null)
            {
                stillInThisSmallPlot = nearestSmallPlot.transform.position;
            }

            EnableCircleUI();
        }
    }

    private void EnableCircleUI()
    {
        if (UIController.Instance == null) return;
        if (smallPlotHasClickToMove == null) return;

        UIController.Instance.SetCurrentSelectedLandPlot(smallPlotHasClickToMove.GetComponentInParent<LandPlot>());
        UIController.Instance.SetCurrentSelectedSmallPlot(smallPlotHasClickToMove.GetComponent<SmallPlot>());
        UIController.Instance.ToggleCircleUI(true);
    }

    private IEnumerator SetIdlePrepareForAction()
    {
        yield return new WaitForSeconds(0.3f);

        if (player != null && player.playerAnimation != null)
        {
            player.playerAnimation.SetIdleImmediatly();
        }

        canDoAction = true;
    }

    private void ResetValueAfterMoveToLandPlotHasClick()
    {
        nearestSmallPlot = null;
        smallPlotHasClickToMove = null;
        _eCharacterMoveState = ECharacterMoveState.move_by_joystick;
    }

    public bool IsMoving() => _isMoving;

    public void SetMoving(bool _isPlayerMoving)
    {
        _isMoving = _isPlayerMoving;
    }

    public void SetDir(Vector3 dir)
    {
        if (!IsLocalPlayer()) return;

        _inputDir = dir;
    }

    public void Crouch()
    {
        if (!IsLocalPlayer()) return;
        if (player == null || player.playerAnimation == null) return;

        if (player.playerAnimation.GetCurrentAnimName() == "character_craft_sit")
        {
            player.playerAnimation.SetIdleTrigger();
        }
        else
        {
            SetDir(Vector2.zero);
            player.playerAnimation.SetSitTrigger();
        }
    }

    public void Jump()
    {
        if (!IsLocalPlayer()) return;
        if (player == null || player.playerAnimation == null) return;

        if (_isGrounded && !player.playerAnimation.IsLockingTransition())
        {
            player.playerAnimation.ClearJumpTrigger();
            player.playerAnimation.SetJumpTrigger();
        }
    }

    public void Jumping()
    {
        if (!IsLocalPlayer()) return;
        if (player == null) return;

        _velocity.y = Mathf.Sqrt(player.jumpForce * -2 * player.gravity);
    }

    public void SetJumpingTrue()
    {
        if (!IsLocalPlayer()) return;

        isJumping = true;
    }

    public void SetJumpingFalse()
    {
        if (!IsLocalPlayer()) return;

        isJumping = false;
    }

    public void SetBusyDoingAction(bool _isBusy)
    {
        if (!IsLocalPlayer()) return;

        isBusyDoingAction = _isBusy;
    }

    public bool IsbusyDoingAction() => isBusyDoingAction;

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
