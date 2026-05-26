using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class StretchWidthToSafeAreaX : MonoBehaviour
{
    RectTransform rectTransform;
    [SerializeField] private bool callUpdateDebug;
    [SerializeField] private RectTransform navBarUI; // Sử dụng Transform cho navBarUI

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        SetAnchorToNavBar();
    }

    private void SetAnchorToNavBar()
    {
        // Chỉ cần cập nhật anchorMax.x của UI element này bằng anchorMin.x của navBarUI
        rectTransform.anchorMax = new Vector2(navBarUI.anchorMin.x, rectTransform.anchorMax.y);
    }

    // private void Update()
    // {

    //     SetAnchorToNavBar();

    // }
}
