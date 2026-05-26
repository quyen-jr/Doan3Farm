using UnityEngine;
using UnityEngine.UI;

public class StretchUIBackground : MonoBehaviour
{
    private RectTransform _uiImage;
    private CanvasScaler canvasScaler;

    void Start()
    {
        canvasScaler=GetComponentInParent<CanvasScaler>();
        _uiImage=GetComponent<RectTransform>();
        // Tỷ lệ giữa chiều rộng và chiều cao của màn hình
        float screenAspect = (float)Screen.width / Screen.height;
        
        // Tính chiều rộng thực tế của màn hình dựa trên match 0.5
        float referenceWidth = canvasScaler.referenceResolution.x;
        float referenceHeight = canvasScaler.referenceResolution.y;
        float matchWidth = Mathf.Lerp(referenceWidth, referenceHeight * screenAspect, 0.5f);

        // Lấy vị trí x hiện tại của ảnh UI
        float currentX = _uiImage.anchoredPosition.x;

        // Tính chiều rộng cần thiết từ vị trí x hiện tại đến mép phải màn hình
        float newWidth = matchWidth - currentX;

        // Cập nhật chiều rộng của ảnh UI
        _uiImage.sizeDelta = new Vector2(newWidth, _uiImage.sizeDelta.y);
    }
}
