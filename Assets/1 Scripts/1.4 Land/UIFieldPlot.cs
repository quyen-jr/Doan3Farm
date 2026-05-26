using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFieldPlot : MonoBehaviour
{
    private int _fieldPlotNumber;
    private UIFieldPlotsManager _landPlotsManager;

    [SerializeField] private Button _UIButton;
    [SerializeField] private TextMeshProUGUI _UIText;
    [SerializeField] private Image _UIImage;
    [SerializeField] private Sprite _UIHasBought;
    [SerializeField] private Sprite _UINormal;

    [Header("Màu khi chọn")]
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _selectedColor = new Color(0.55f, 0.55f, 0.55f, 1f);

    public int FieldPlotNumber => _fieldPlotNumber;

    private void Awake()
    {
        if (_UIButton == null)
            _UIButton = GetComponent<Button>();

        _landPlotsManager = GetComponentInParent<UIFieldPlotsManager>();
    }

    private void OnEnable()
    {
        if (_UIButton != null)
            _UIButton.onClick.AddListener(OnSelectThisUIFieldplot);
    }

    private void OnDisable()
    {
        if (_UIButton != null)
            _UIButton.onClick.RemoveListener(OnSelectThisUIFieldplot);
    }

    private void OnSelectThisUIFieldplot()
    {
        if (_landPlotsManager != null)
            _landPlotsManager.OnSelectUILandPlot(this);


    }

    public void SetNumber(int number)
    {
        _fieldPlotNumber = number;

        if (_UIText != null)
            _UIText.text = (number+1).ToString();
    }

    public void SetState(bool isBought)
    {
        if (_UIImage == null) return;

        if (isBought)
            _UIImage.sprite = _UIHasBought;
        else
            _UIImage.sprite = _UINormal;
    }

    public void SetSelected(bool isSelected)
    {
        if (_UIImage == null) return;

        if (isSelected)
            _UIImage.color = _selectedColor;
        else
            _UIImage.color = _normalColor;
    }


    public int GetNumberID() => _fieldPlotNumber;
}