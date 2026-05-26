using UnityEngine;
using UnityEngine.UI;

public class WayPointImage : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform target;
    private WarningType type;
    [SerializeField] private Image image;
    [SerializeField] private Vector3 offset;
    private Player _player;
    private float maxDistance = 20f; // distance to disable crop ui
    private void Start()
    {
        _player = Player.LocalPlayer;
    }
    private bool isDisplayBackward;
    public void SetSprite(Sprite _sprite)
    {

        image.sprite = _sprite;
        SetUpWayPointScale();
    }
    private void SetUpWayPointScale()
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();

        // Thi?t l?p pivot
        if (type == WarningType.HasWorm)
        {
            offset = new Vector2(-1.5f, offset.y);
        }
        else if (type == WarningType.Water)
        {
            offset = new Vector2(0f, offset.y);
        }
        else if (type == WarningType.Fertilizer)
        {
            offset = new Vector2(1f, offset.y);
        }
    }
    public void setTargetOBJ(Transform _target)
    {
        target = _target;
    }
    public void SetWayPointType(WarningType _wayPointType)
    {
        type = _wayPointType;
    }
    public WarningType GetWayPointType() => type;
    private void Update()
    {
        if (Vector3.Distance(_player.transform.position, target.position) > maxDistance)
        {
            image.enabled = false; // ?n waypoint n?u quá xa
            return;
        }
        float minX = image.GetPixelAdjustedRect().width * 2.5f;
        float maxX = Screen.width - minX;

        float minY = image.GetPixelAdjustedRect().height * 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);


        Vector3 viewPos = Camera.main.WorldToViewportPoint(target.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            image.enabled = true;
        }
        else
        {
            isDisplayBackward = target.GetComponentInParent<FieldPlots>().CheckPlayerInField();
            if (isDisplayBackward)
            {
                image.enabled = true;
                //return;
            }
            else
            {
                image.enabled = false;
            }
            if (pos.x < Screen.width / 2)
            {
                // Place it on the right (Since it's behind the player, it's the opposite)
                pos.x = minX;
            }
            else
            {
                // Place it on the left side

                pos.x = maxX;
            }
        }
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Update the marker's position
        image.transform.position = pos;
    }
    public void DestroyWayPoint()
    {
        Destroy(gameObject);
    }
    public void ToggleDisplay(bool _isEnable)
    {
        Debug.Log(1);
        isDisplayBackward = _isEnable;
    }
}

