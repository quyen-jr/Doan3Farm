using UnityEngine;
using UnityEngine.UI;

public class ResponsiveCanvas : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    public RectTransform parentRect; // Thằng cha cần responsive
    public float match = 0.5f; // Tỷ lệ match (0 đến 1)

    void Start()
    {
        AdjustCanvasScaler();
    }

    void AdjustCanvasScaler()
    {
        // Giữ Canvas ở kích thước 1920x1080
        if (canvasScaler != null)
        {
            // Giữ độ phân giải cố định của Canvas
            canvasScaler.referenceResolution = new Vector2(1920, 1080); 

            // Điều chỉnh match từ 0.5 về 0 nếu muốn scale theo chiều rộng
            canvasScaler.matchWidthOrHeight = match;

            // Cập nhật kích thước thằng cha sao cho responsive mà không thay đổi độ phân giải của Canvas
            SetParentResponsive();
        }
    }

    void SetParentResponsive()
    {
        if (parentRect != null)
        {
            // Đặt lại anchor và stretch của thằng cha
            parentRect.anchorMin = new Vector2(0f, 1f); // Góc trên bên trái
            parentRect.anchorMax = new Vector2(0f, 1f); // Góc trên bên trái
            parentRect.pivot = new Vector2(0f, 1f); // Pivot ở góc trên bên trái

            // Cập nhật kích thước thằng cha
            parentRect.sizeDelta = new Vector2(Screen.width, Screen.height); 

            // Reset lại Canvas để đảm bảo rằng Canvas cập nhật lại kích thước của nó
            ResetCanvas();
        }
    }

    void ResetCanvas()
    {
        // Cập nhật lại Canvas để nó tái tạo kích thước mới mà không thay đổi độ phân giải ban đầu
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = false;  // Tắt Canvas
            canvas.enabled = true;   // Bật lại Canvas, để nó "refresh" và cập nhật lại các thay đổi
        }
    }
}
