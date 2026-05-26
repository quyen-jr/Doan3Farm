using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaAdjuster : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField]private RectTransform _fixReponsiveLeftNav;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }
    // private void Update() {
    //     ApplySafeArea();
    // }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        // Chuyển đổi tọa độ từ pixel sang tỷ lệ anchor (0 đến 1)
        float anchorMinX = safeArea.x / Screen.width;
        float anchorMaxX = (safeArea.x + safeArea.width) / Screen.width;

        // Chỉ cập nhật chiều rộng, giữ nguyên giá trị y của anchorMin và anchorMax
        rectTransform.anchorMin = new Vector2(anchorMinX, rectTransform.anchorMin.y);
        rectTransform.anchorMax = new Vector2(anchorMaxX, rectTransform.anchorMax.y);
        _fixReponsiveLeftNav.anchorMax = new Vector2(anchorMinX, _fixReponsiveLeftNav.anchorMax.y);
    }

    // void Update()
    // {
    //     ApplySafeArea();
    // }
}
