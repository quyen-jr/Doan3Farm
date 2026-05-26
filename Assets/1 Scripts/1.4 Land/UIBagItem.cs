using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBagItem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button _button;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _imageIcon;
    [SerializeField] private TextMeshProUGUI _textAmount;

    [Header("Color")]
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _selectedColor = Color.gray;

    [Header("Runtime")]
    [SerializeField] private BagItemSlot _slot;

    private UIBagUI _bagUI;

    public BagItemSlot Slot => _slot;

    private void Awake()
    {
        if (_button != null)
        {
            _button.onClick.AddListener(OnClickItem);
        }
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(OnClickItem);
        }
    }

    public void InitData(BagItemSlot slot, UIBagUI bagUI)
    {
        _slot = slot;
        _bagUI = bagUI;

        if (_slot == null || _slot.amount <= 0 || _slot.itemConfig == null || _slot.itemConfig.itemData == null)
        {
            SetSelected(false);
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        _imageIcon.sprite = _slot.itemConfig.itemData.sprite;
        _textAmount.text = "X " + _slot.amount.ToString();

        SetSelected(false);
    }

    private void OnClickItem()
    {
        if (_slot == null) return;

        if (_bagUI != null)
        {
            _bagUI.SelectItem(this);
        }
    }

    public void SetSelected(bool isSelected)
    {
        if (_backgroundImage != null)
        {
            _backgroundImage.color = isSelected ? _selectedColor : _normalColor;
        }
    }
}