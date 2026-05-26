using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button _button;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _imageIcon;
    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private TextMeshProUGUI _textAmountPerBuy;
    [SerializeField] private TextMeshProUGUI _textPrice;

    [Header("Color")]
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _selectedColor = new Color(0.45f, 0.45f, 0.45f, 1f);

    private UIShopManager _shopManager;
    private ShopItemConfig _shopItemConfig;

    public ShopItemConfig ShopItemConfig => _shopItemConfig;

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

    public void InitData(ShopItemConfig config, UIShopManager shopManager)
    {
        _shopItemConfig = config;
        _shopManager = shopManager;

        if (_shopItemConfig == null || _shopItemConfig.itemConfig == null || _shopItemConfig.itemConfig.itemData == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (_imageIcon != null)
            _imageIcon.sprite = _shopItemConfig.itemConfig.itemData.sprite;

        if (_textName != null)
            _textName.text = _shopItemConfig.itemName;

        if (_textAmountPerBuy != null)
            _textAmountPerBuy.text = $"{_shopItemConfig.amountPerBuy} hạt/gói";

        if (_textPrice != null)
            _textPrice.text = _shopItemConfig.price.ToString();

        SetSelected(false);
    }

    private void OnClickItem()
    {
        Debug.Log("Click shop item: " + _shopItemConfig.itemName);

        if (_shopManager != null)
        {
            _shopManager.SelectItem(this);
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