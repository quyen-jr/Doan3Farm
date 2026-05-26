using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopSaleManager : MonoBehaviour
{
    [Header("List UI")]
    [SerializeField] private Transform _content;
    [SerializeField] private UIShopSellItem _shopItemPrefab;

    [Header("Sell UI")]
    [SerializeField] private Button _buttonDecrease;
    [SerializeField] private Button _buttonIncrease;
    [SerializeField] private Button _buttonSell;
    [SerializeField] private TextMeshProUGUI _textSellAmount;
    [SerializeField] private TextMeshProUGUI _textTotalSellPrice;

    [Header("Money Test")]
    [SerializeField] private int _currentGold = 3000;
    [SerializeField] private TextMeshProUGUI _textGold;

    private readonly List<UIShopSellItem> _spawnedItems = new List<UIShopSellItem>();

    private UIShopSellItem _selectedUIItem;
    private BagItemSlot _selectedSlot;

    private int _sellAmount = 1;

    private void OnEnable()
    {
        _buttonDecrease.onClick.AddListener(DecreaseAmount);
        _buttonIncrease.onClick.AddListener(IncreaseAmount);
        _buttonSell.onClick.AddListener(SellSelectedItem);

        if (BagItemManager.Instance != null)
        {
            BagItemManager.Instance.OnBagItemChanged += RefreshSaleList;
        }

        RefreshSaleList();
        UpdateGoldUI();
    }

    private void OnDisable()
    {
        _buttonDecrease.onClick.RemoveListener(DecreaseAmount);
        _buttonIncrease.onClick.RemoveListener(IncreaseAmount);
        _buttonSell.onClick.RemoveListener(SellSelectedItem);

        if (BagItemManager.Instance != null)
        {
            BagItemManager.Instance.OnBagItemChanged -= RefreshSaleList;
        }
    }

    private void RefreshSaleList()
    {
        GenerateShopItems();

        if (_spawnedItems.Count > 0)
        {
            SelectItem(_spawnedItems[0]);
        }
        else
        {
            ClearSelectedItem();
        }
    }

    private void GenerateShopItems()
    {
        ClearShopItems();

        if (BagItemManager.Instance == null) return;

        // Nếu muốn bán tất cả item trong balo thì dùng GetAllSlots()
        List<BagItemSlot> slots = BagItemManager.Instance.GetSlotsByCategory(EBagItemCategory.seed);

        foreach (BagItemSlot slot in slots)
        {
            if (slot == null || slot.amount <= 0) continue;
            if (slot.itemConfig == null || slot.itemConfig.itemData == null) continue;

            UIShopSellItem item = Instantiate(_shopItemPrefab, _content);
            item.InitData(slot, this);
            _spawnedItems.Add(item);
        }
    }

    private void ClearShopItems()
    {
        foreach (UIShopSellItem item in _spawnedItems)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }

        _spawnedItems.Clear();
    }

    public void SelectItem(UIShopSellItem uiItem)
    {
        if (uiItem == null) return;

        if (_selectedUIItem != null)
        {
            _selectedUIItem.SetSelected(false);
        }

        _selectedUIItem = uiItem;
        _selectedSlot = uiItem.ShopItemConfig;

        Debug.Log("item  "+ _selectedSlot.amount );
        _selectedUIItem.SetSelected(true);

        _sellAmount = 1;

        UpdateSellAmountUI();
    }

    private void ClearSelectedItem()
    {
        _selectedUIItem = null;
        _selectedSlot = null;
        _sellAmount = 1;

        if (_textSellAmount != null)
            _textSellAmount.text = "0";

        if (_textTotalSellPrice != null)
            _textTotalSellPrice.text = "0";
    }

    private void IncreaseAmount()
    {
        if (_selectedSlot == null) return;

        int maxCanSell = GetMaxCanSell();

        if (_sellAmount < maxCanSell)
        {
            _sellAmount++;
        }

        UpdateSellAmountUI();
    }

    private void DecreaseAmount()
    {
        if (_selectedSlot == null) return;

        if (_sellAmount > 1)
        {
            _sellAmount--;
        }

        UpdateSellAmountUI();
    }

    private int GetMaxCanSell()
    {
        if (_selectedSlot == null)
            return 1;

        return Mathf.Max(1, _selectedSlot.amount);
    }

    private void UpdateSellAmountUI()
    {
        if (_selectedSlot == null || _selectedSlot.itemConfig == null || _selectedSlot.itemConfig.itemData == null)
        {
            ClearSelectedItem();
            return;
        }

        _sellAmount = Mathf.Clamp(_sellAmount, 1, GetMaxCanSell());

        int totalSellPrice = (int)_selectedSlot.itemConfig.itemData.sellPrice * _sellAmount;

        if (_textSellAmount != null)
        {
            _textSellAmount.text = _sellAmount.ToString();
        }

        if (_textTotalSellPrice != null)
        {
            _textTotalSellPrice.text = totalSellPrice.ToString();
        }
    }

    private void SellSelectedItem()
    {

        ShopScreen shopScreen = GetComponentInParent<ShopScreen>();
        if (_selectedSlot == null)
        {
            Debug.LogWarning("Chưa chọn item để bán.");
            return;
        }

        if (BagItemManager.Instance == null)
        {
            Debug.LogError("Không tìm thấy BagItemManager trong scene.");
            return;
        }

        if (_selectedSlot.itemConfig == null || _selectedSlot.itemConfig.itemData == null)
        {
            Debug.LogWarning("Item bán bị thiếu config hoặc itemData.");
            return;
        }

        if (_sellAmount <= 0 || _sellAmount > _selectedSlot.amount)
        {
            Debug.LogWarning("Số lượng bán không hợp lệ.");
            return;
        }

        int totalSellPrice = (int)_selectedSlot.itemConfig.itemData.sellPrice * _sellAmount;

        // Cộng tiền
        _currentGold += totalSellPrice;

        // Trừ item khỏi balo
        BagItemManager.Instance.RemoveItem(_selectedSlot.itemConfig, _sellAmount);

        UpdateGoldUI();

        GenerateShopItems();

        Debug.Log($"Đã bán {_selectedSlot.itemConfig.itemData.name} x{_sellAmount}, nhận {totalSellPrice} vàng");


        if (shopScreen != null)
        {
            shopScreen.DisPlayNotification("Bán thành công!", true);
        }
    }

    private void UpdateGoldUI()
    {
        //if (_textGold != null)
        //{
        //    _textGold.text = _currentGold.ToString();
        //}
    }
}