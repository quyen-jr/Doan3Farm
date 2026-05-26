using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopManager : MonoBehaviour
{
    [Header("Shop Data")]
    [SerializeField] private List<ShopItemConfig> _shopItems = new List<ShopItemConfig>();

    [Header("List UI")]
    [SerializeField] private Transform _content;
    [SerializeField] private UIShopItem _shopItemPrefab;

    //[Header("Detail UI")]
    //[SerializeField] private Image _imageSelectedIcon;
    //[SerializeField] private TextMeshProUGUI _textSelectedName;
    //[SerializeField] private TextMeshProUGUI _textSelectedDescription;
    //[SerializeField] private TextMeshProUGUI _textSelectedPrice;

    [Header("Buy UI")]
    [SerializeField] private Button _buttonDecrease;
    [SerializeField] private Button _buttonIncrease;
    [SerializeField] private Button _buttonBuy;
    [SerializeField] private TextMeshProUGUI _textBuyAmount;

    [Header("Money Test")]
    [SerializeField] private int _currentGold = 3000;
    [SerializeField] private TextMeshProUGUI _textGold;

    private readonly List<UIShopItem> _spawnedItems = new List<UIShopItem>();

    private UIShopItem _selectedUIItem;
    private ShopItemConfig _selectedConfig;

    private int _buyAmount = 1;

    private void Awake()
    {
        GenerateShopItems();
    }

    private void OnEnable()
    {
        _buttonDecrease.onClick.AddListener(DecreaseAmount);
        _buttonIncrease.onClick.AddListener(IncreaseAmount);
        _buttonBuy.onClick.AddListener(BuySelectedItem);

        UpdateGoldUI();

        if (_spawnedItems.Count > 0 && _selectedUIItem == null)
        {
            SelectItem(_spawnedItems[0]);
        }
    }

    private void OnDisable()
    {
        _buttonDecrease.onClick.RemoveListener(DecreaseAmount);
        _buttonIncrease.onClick.RemoveListener(IncreaseAmount);
        _buttonBuy.onClick.RemoveListener(BuySelectedItem);
    }

    private void GenerateShopItems()
    {
        ClearShopItems();

        foreach (ShopItemConfig config in _shopItems)
        {
            UIShopItem item = Instantiate(_shopItemPrefab, _content);
            item.InitData(config, this);
            _spawnedItems.Add(item);
        }
    }

    private void ClearShopItems()
    {
        foreach (UIShopItem item in _spawnedItems)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }

        _spawnedItems.Clear();
    }

    public void SelectItem(UIShopItem uiItem)
    {
        if (uiItem == null) return;

        if (_selectedUIItem != null)
        {
            _selectedUIItem.SetSelected(false);
        }

        _selectedUIItem = uiItem;
        _selectedConfig = uiItem.ShopItemConfig;

        _selectedUIItem.SetSelected(true);

        _buyAmount = 1;

        UpdateSelectedInfo();
        UpdateBuyAmountUI();
    }

    private void UpdateSelectedInfo()
    {
        if (_selectedConfig == null || _selectedConfig.itemConfig == null || _selectedConfig.itemConfig.itemData == null)
        {
            return;
        }

        //if (_imageSelectedIcon != null)
        //    _imageSelectedIcon.sprite = _selectedConfig.itemConfig.itemData.sprite;

        //if (_textSelectedName != null)
        //    _textSelectedName.text = _selectedConfig.itemName;

        //if (_textSelectedDescription != null)
        //    _textSelectedDescription.text = _selectedConfig.description;

        //if (_textSelectedPrice != null)
        //    _textSelectedPrice.text = $"Giá: {_selectedConfig.price}";
    }

    private void IncreaseAmount()
    {
        if (_selectedConfig == null) return;

        int maxCanBuy = GetMaxCanBuy();

        if (_buyAmount < maxCanBuy)
        {
            _buyAmount++;
        }

        UpdateBuyAmountUI();
    }

    private void DecreaseAmount()
    {
        if (_buyAmount > 1)
        {
            _buyAmount--;
        }

        UpdateBuyAmountUI();
    }

    private void UpdateBuyAmountUI()
    {
        if (_textBuyAmount != null)
        {
            _textBuyAmount.text = _buyAmount.ToString();
            _textGold.text = (_buyAmount* _selectedConfig.price).ToString();
        }
    }

    private int GetMaxCanBuy()
    {
        if (_selectedConfig == null || _selectedConfig.price <= 0)
        {
            return 1;
        }

        int maxCanBuy = _currentGold / _selectedConfig.price;
        return Mathf.Max(1, maxCanBuy);
    }

    private void BuySelectedItem()
    {

        ShopScreen shopScreen = GetComponentInParent<ShopScreen>();
        if (_selectedConfig == null)
        {
            Debug.LogWarning("Chưa chọn item để mua.");
            return;
        }

        if (BagItemManager.Instance == null)
        {
            Debug.LogError("Không tìm thấy BagItemManager trong scene.");
            return;
        }

        int totalPrice = _selectedConfig.price * _buyAmount;

        if (_currentGold < totalPrice)
        {
            Debug.LogWarning("Không đủ tiền để mua.");

            if(shopScreen != null)
            {
                shopScreen.DisPlayNotification("Không đủ tiền để mua!", false);
            }
            return;
        }

        _currentGold -= totalPrice;

        int amountAddToBag = _selectedConfig.amountPerBuy * _buyAmount;

        BagItemManager.Instance.AddItem(_selectedConfig.itemConfig, amountAddToBag);

        UpdateGoldUI();

        int maxCanBuy = GetMaxCanBuy();
        _buyAmount = Mathf.Clamp(_buyAmount, 1, maxCanBuy);

        UpdateBuyAmountUI();

        Debug.Log($"Đã mua {_selectedConfig.itemName} x{amountAddToBag}");


        if (shopScreen != null)
        {
            shopScreen.DisPlayNotification("Mua Thành công!", true);
        }
    }

    private void UpdateGoldUI()
    {
        if (_textGold != null)
        {
         
        }
    }
}