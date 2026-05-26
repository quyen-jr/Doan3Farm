using UnityEngine;
using UnityEngine.UI;
public enum WarningType
{
    Water,
    HasWorm,
    WrongProcess,
    Fertilizer,
    Ripe
}
public class WarningCropImage : MonoBehaviour
{
    [SerializeField] private Image image;
    private Transform plotParent;
    public WarningType warningType { get; private set; }
    private Vector2 offset;
    public void SetWarningImage(Sprite _image, WarningType _warningType)
    {
        image.sprite = _image;
        warningType = _warningType;
        SetUpWarningScale();
    }
    private void SetUpWarningScale()
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        if (warningType == WarningType.HasWorm)
        {

            offset = new Vector2(0.3f, 0.3f);
            rectTransform.sizeDelta = new Vector2(0.3f, 0.3f);
            // rectTransform.anchoredPosition = new Vector2(-2f, -1.24f);
        }
        if (warningType == WarningType.Water)
        {
            offset = new Vector2(0f, 0.3f);
            rectTransform.sizeDelta = new Vector2(0.3f, 0.3f);
            // rectTransform.anchoredPosition = new Vector2(-1f, -1.24f);
        }
        if (warningType == WarningType.Fertilizer)
        {
            offset = new Vector2(-0.3f, 0.3f);
            rectTransform.sizeDelta = new Vector2(0.3f, 0.3f);
            // rectTransform.anchoredPosition = new Vector2(0, -1.24f);
        }
    }
    public void SetplotParent(Transform _plotParent)
    {
        plotParent = _plotParent;
    }
    void Update()
    {
        if (plotParent != null && image.sprite != null)
            transform.position = (plotParent.position + (Vector3)offset);
    }
    public void DestroyWarningImage()
    {
        Destroy(gameObject);
    }
}
