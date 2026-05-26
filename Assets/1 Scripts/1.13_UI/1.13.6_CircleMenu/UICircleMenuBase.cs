using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ECircleMenuIndexSelectedType
{
    clockwise, //cùng chiều kim đồng hồ, là index = 0 là option gần tọa độ góc = 0;
    counterclockwise, //ngược kim đồng hồ, là index = 0 là option gần tọa độ góc = 0;
}

public class UICircleMenuBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Config")]
    [SerializeField] private RectTransform _circlePiece;
    [SerializeField] private Image _imageCirclePiece;
    [SerializeField] private ECircleMenu _eCircleMenu;
    [SerializeField] private float _maxPiece = 7;
    [SerializeField] private ECircleMenuIndexSelectedType _eCircleMenuIndexSelectedType;

    protected UICircleMenuListOptionsBase _uICircleMenuListOptions; // list in children
    protected int _currentIndexPiece;

    private bool _isHolding = false;
    private Vector2 _midPosScreen;

    public ECircleMenu eCircleMenu => _eCircleMenu;
    public float MaxPiece => _maxPiece;

    private void AttachVarriable()
    {
        _uICircleMenuListOptions = GetComponentInChildren<UICircleMenuListOptionsBase>();
    }

    private void Awake()
    {
        AttachVarriable();

        _midPosScreen = new Vector2(Screen.width / 2, Screen.height / 2);

        _circlePiece.gameObject.SetActive(false);
        _imageCirclePiece.fillAmount = 1 / _maxPiece;

        _uICircleMenuListOptions.GenerateOption(_maxPiece, _eCircleMenuIndexSelectedType);
    }

    public void OnPointerDown(PointerEventData eventData) //interface
    {
        _isHolding = true;
        StartCoroutine(OnPointerHolding());
    }

    public void OnPointerUp(PointerEventData eventData) //interface
    {
        _isHolding = false;
        _circlePiece.gameObject.SetActive(_isHolding);
        OnSelectOption();
    }

    private IEnumerator OnPointerHolding()
    {
        while (_isHolding)
        {
            Vector2 dirMouse = (Vector2)Input.mousePosition - _midPosScreen;

            float angle = (dirMouse.y > 0) ? Vector2.Angle(dirMouse, Vector2.right) : 360 - Vector2.Angle(dirMouse, Vector2.right);

            int currentIndexAngle = (int)(angle / (360 / _maxPiece)); // vị trí hiện tại chia theo góc của mỗi miếng

            _circlePiece.rotation = Quaternion.Euler(0, 0, currentIndexAngle * (360 / _maxPiece));

            _circlePiece.gameObject.SetActive(_isHolding);

            UpdateCurrentIndexPieceSelected(currentIndexAngle);

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void UpdateCurrentIndexPieceSelected(int currentIndexAngle)
    {
        switch (_eCircleMenuIndexSelectedType)
        {
            case ECircleMenuIndexSelectedType.counterclockwise:
                _currentIndexPiece = currentIndexAngle; //index đếm ngược lại theo chiều kim đồng hồ
                break;
            case ECircleMenuIndexSelectedType.clockwise:
                _currentIndexPiece = (int)_maxPiece - currentIndexAngle - 1; //index đếm chiều kim đồng hồ
                break;
        }
    }

    public void Visible(bool isVisible)
    {
        if (gameObject.activeSelf != isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }

    protected virtual void OnSelectOption() { }
}
