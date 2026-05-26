using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBagUI : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private UIBagItem _bagItemPrefab;

    [Header("Runtime")]
    [SerializeField] private List<UIBagItem> _currentItems = new List<UIBagItem>();

    [Header("Detail UI")]
    [SerializeField] private GameObject _detailPanel;
    [SerializeField] private Image _detailIcon;
    [SerializeField] private TextMeshProUGUI _detailNameText;
    [SerializeField] private TextMeshProUGUI _detailDescriptionText;
    [SerializeField] private TextMeshProUGUI _detailAmountText;

    private UIBagItem _currentSelectedItem;

    private void Awake()
    {
        _currentItems = GetComponentsInChildren<UIBagItem>(true).ToList();
    }

    private void OnEnable()
    {
        if (BagItemManager.Instance != null)
        {
            BagItemManager.Instance.OnBagItemChanged += RefreshUI;
        }

        RefreshUI();
    }

    private void OnDisable()
    {
        if (BagItemManager.Instance != null)
        {
            BagItemManager.Instance.OnBagItemChanged -= RefreshUI;
        }
    }

    private void RefreshUI()
    {
        if (BagItemManager.Instance == null)
        {
            Debug.LogWarning("BagItemManager.Instance is null");
            return;
        }

        List<BagItemSlot> slots = BagItemManager.Instance.GetAllSlots();

        while (_currentItems.Count < slots.Count)
        {
            if (_bagItemPrefab == null)
            {
                Debug.LogWarning("Bag item prefab chưa được gán!");
                break;
            }

            UIBagItem newItem = Instantiate(_bagItemPrefab, transform);
            _currentItems.Add(newItem);
        }

        for (int i = 0; i < _currentItems.Count; i++)
        {
            if (i < slots.Count)
            {
                _currentItems[i].InitData(slots[i], this);
            }
            else
            {
                _currentItems[i].InitData(null, this);
            }
        }

        // Nếu item đang chọn đã bị xóa hoặc hết số lượng thì clear detail
        if (_currentSelectedItem == null ||
            _currentSelectedItem.Slot == null ||
            _currentSelectedItem.Slot.amount <= 0)
        {
            ClearSelectedItem();
        }
        else
        {
            UpdateDetailUI(_currentSelectedItem.Slot);
        }
    }

    public void SelectItem(UIBagItem item)
    {
        if (item == null || item.Slot == null) return;

        if (_currentSelectedItem != null)
        {
            _currentSelectedItem.SetSelected(false);
        }

        _currentSelectedItem = item;
        _currentSelectedItem.SetSelected(true);

        UpdateDetailUI(item.Slot);
    }

    private void UpdateDetailUI(BagItemSlot slot)
    {
        if (slot == null || slot.itemConfig == null || slot.itemConfig.itemData == null)
        {
            ClearSelectedItem();
            return;
        }

        if (_detailPanel != null)
            _detailPanel.SetActive(true);

        if (_detailIcon != null)
            _detailIcon.sprite = slot.itemConfig.itemData.sprite;

        if (_detailNameText != null)
            _detailNameText.text = slot.itemConfig.itemData.itemName;

        if (_detailAmountText != null)
            _detailAmountText.text = "Số lượng: " + slot.amount.ToString();

        if (_detailDescriptionText != null)
            _detailDescriptionText.text =slot.itemConfig.itemData.GetDescription();
    }

    private void ClearSelectedItem()
    {
        if (_currentSelectedItem != null)
        {
            _currentSelectedItem.SetSelected(false);
        }

        _currentSelectedItem = null;

        if (_detailPanel != null)
            _detailPanel.SetActive(false);

        if (_detailIcon != null)
            _detailIcon.sprite = null;

        if (_detailNameText != null)
            _detailNameText.text = "";

        if (_detailDescriptionText != null)
            _detailDescriptionText.text = "";

        if (_detailAmountText != null)
            _detailAmountText.text = "";
    }

    private string GetItemDescription(ItemData itemData)
    {
        // Nếu ItemData của bạn có biến description thì đổi dòng này.
        // Ví dụ: return itemData.description;
        return itemData.name;
    }
}