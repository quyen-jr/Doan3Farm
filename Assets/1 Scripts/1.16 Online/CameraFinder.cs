using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class CameraFinder : MonoBehaviour
{
    [SerializeField] private Transform rigHighCam;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    private PhotonView photonView;
    private PlayerInputEvent playerInputEvent;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        playerInputEvent = GetComponent<PlayerInputEvent>();
    }

    private void Start()
    {
        // Only local player is allowed to bind shared scene camera.
        if (photonView != null && !photonView.IsMine) return;

        if (freeLookCamera == null)
        {
            freeLookCamera = FindObjectOfType<CinemachineFreeLook>(true);
        }

        if (freeLookCamera == null)
        {
            Debug.LogWarning("Khong tim thay CinemachineFreeLook trong scene.");
            return;
        }

        freeLookCamera.Follow = transform;
        freeLookCamera.LookAt = rigHighCam != null ? rigHighCam : transform;

        if (playerInputEvent != null)
        {
            playerInputEvent.freeLookCamera = freeLookCamera;
        }
    }
}
