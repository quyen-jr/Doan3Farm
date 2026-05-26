using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public enum ActionMap
    {
        Player,
        Land,
        Menu
    }
    private static Player _localPlayer;
    public static Player LocalPlayer
    {
        get
        {
            if (_localPlayer == null) return null;
            if (_localPlayer.photonView != null && !_localPlayer.photonView.IsMine) return null;
            return _localPlayer;
        }
        private set => _localPlayer = value;
    }
    public float moveSpeed;
    public float jumpForce;
    public float cameraRotateSpeed;
    public float gravity;
    public float landInteractDistance;

    [HideInInspector]
    public PlayerMovement playerMovement;
    [HideInInspector]
    public PlayerLook playerLook;
    [HideInInspector]
    public PlayerAnimation playerAnimation;
    [HideInInspector]
    public PlayerInteractHandler playerInteract;
    [HideInInspector]
    public PlayerInputEvent playerInputEvent;
    [HideInInspector]
    public PlayerInteractHandler playerInteractHandler;
    [HideInInspector]
    public PlayerInput playerInput;
    [Header("Female Player")]
    [SerializeField] private Transform femalePlayer;
    [SerializeField] List<GameObject> femaleTool;

    [Header("Male Player")]
    [SerializeField] private Transform malePlayer;
    [SerializeField] List<GameObject> maleTool;
    [SerializeField] private CameraController cameraController;


    [SerializeField] public PhotonView photonView;


    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView != null && photonView.IsMine)
        {
            LocalPlayer = this;
            UIController.Instance.player = this;
        }
        else if (photonView == null && LocalPlayer == null)
        {
            // Fallback for non-network/offline usage.
            LocalPlayer = this;
        }
    }

    private void Awake()
    {
        playerLook = GetComponent<PlayerLook>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerInteract = GetComponent<PlayerInteractHandler>();
        playerInputEvent = GetComponent<PlayerInputEvent>();
        playerInteractHandler = GetComponent<PlayerInteractHandler>();
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    public void SwitchPlayer()
    {
        if (photonView != null && !photonView.IsMine) return;
        // Đảo ngược trạng thái hoạt động của các GameObject
        bool isFemaleActive = !femalePlayer.gameObject.activeSelf;
        femalePlayer.gameObject.SetActive(isFemaleActive);
        malePlayer.gameObject.SetActive(!isFemaleActive);

        //Đặt Avatar cho Animator
        if (isFemaleActive)
        {
            //playerAnimation.GetAnimator().avatar = femaleAvatar;
            UIController.Instance.landInteraction.SwitchToolPlayer(femaleTool);
        }
        else
        {
            // playerAnimation.GetAnimator().avatar = maleAvatar;
            UIController.Instance.landInteraction.SwitchToolPlayer(maleTool);
        }

        // Reset trạng thái cho Animator
        playerInputEvent.SwitchActionMapPlayer();
        playerAnimation.ClearAllTrigger();
        ReverseChildObjects();
        playerAnimation.GetAnimator().Rebind();

        GetComponentInChildren<PlayerAnimEvent>().DisableAllTools();
        playerMovement.CancleActionWhenUseJoyStick();
        playerMovement.SetMoving(false);
    }
    void ReverseChildObjects()
    {
        Transform parentTransform = transform;
        int childCount = parentTransform.childCount;

        // Chuyển các con sang danh sách để đảo ngược
        Transform[] children = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            children[i] = parentTransform.GetChild(i);
        }

        // Đặt lại thứ tự các con theo thứ tự ngược
        for (int i = 0; i < childCount; i++)
        {
            children[i].SetSiblingIndex(childCount - i - 1);
        }
    }
}


