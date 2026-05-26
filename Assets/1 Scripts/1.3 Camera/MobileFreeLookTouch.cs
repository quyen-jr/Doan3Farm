using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class MobileFreeLookTouch : MonoBehaviour
{
    [Header("Cinemachine")]
    public CinemachineFreeLook freeLookCamera;

    [Header("Touch Area")]
    [Range(0f, 1f)]
    public float rightScreenStart = 0.5f; // 0.5 = nửa phải màn hình

    [Header("Sensitivity")]
    public float sensitivityX = 0.18f;
    public float sensitivityY = 0.003f;

    [Header("Limit Y")]
    public float minY = 0f;
    public float maxY = 1f;

    private int activeFingerId = -1;
    private Vector2 lastTouchPos;

    void Reset()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouse();
#endif

        HandleTouch();
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0)
        {
            activeFingerId = -1;
            return;
        }

        // Nếu chưa có ngón tay nào đang điều khiển camera
        if (activeFingerId == -1)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                bool isRightSide = touch.position.x >= Screen.width * rightScreenStart;

                if (touch.phase == TouchPhase.Began && isRightSide)
                {
                    // Nếu bấm trúng UI bên phải thì bỏ qua
                    if (EventSystem.current != null &&
                        EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        continue;
                    }

                    activeFingerId = touch.fingerId;
                    lastTouchPos = touch.position;
                    break;
                }
            }
        }

        // Nếu đã có ngón tay điều khiển camera
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.fingerId != activeFingerId)
                continue;

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.position - lastTouchPos;
                RotateCamera(delta);
                lastTouchPos = touch.position;
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                activeFingerId = -1;
            }

            break;
        }
    }

    void HandleMouse()
    {
        // Test bằng chuột trong Editor / Simulator
        if (Input.GetMouseButtonDown(0))
        {
            bool isRightSide = Input.mousePosition.x >= Screen.width * rightScreenStart;

            if (isRightSide)
            {
                if (EventSystem.current != null &&
                    EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                lastTouchPos = Input.mousePosition;
            }
        }

        if (Input.GetMouseButton(0))
        {
            bool isRightSide = Input.mousePosition.x >= Screen.width * rightScreenStart;

            if (!isRightSide)
                return;

            Vector2 currentPos = Input.mousePosition;
            Vector2 delta = currentPos - lastTouchPos;

            RotateCamera(delta);

            lastTouchPos = currentPos;
        }
    }

    void RotateCamera(Vector2 delta)
    {
        if (freeLookCamera == null)
            return;

        // X: xoay trái phải
        freeLookCamera.m_XAxis.Value += delta.x * sensitivityX;

        // Y: xoay lên xuống
        freeLookCamera.m_YAxis.Value -= delta.y * sensitivityY;
        freeLookCamera.m_YAxis.Value = Mathf.Clamp(
            freeLookCamera.m_YAxis.Value,
            minY,
            maxY
        );
    }
}