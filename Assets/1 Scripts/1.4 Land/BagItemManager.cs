using System;
using System.Collections.Generic;
using UnityEngine;

public class BagItemManager : MonoBehaviour
{
    public static BagItemManager Instance;

    [SerializeField] private List<BagItemSlot> bagItemSlots = new List<BagItemSlot>();

    public event Action OnBagItemChanged;

    private void Awake()
    {
        Instance = this;
    }

    public List<BagItemSlot> GetAllSlots()
    {
        return bagItemSlots;
    }

    public List<BagItemSlot> GetSlotsByCategory(EBagItemCategory category)
    {
        return bagItemSlots.FindAll(slot =>
            slot != null &&
            slot.itemConfig != null &&
            slot.itemConfig.category == category &&
            slot.amount > 0
        );
    }

    public void AddItem(BagItemConfig itemConfig, int amount)
    {
        if (itemConfig == null || itemConfig.itemData == null || amount <= 0)
            return;

        BagItemSlot slot = bagItemSlots.Find(x =>
            x.itemConfig != null &&
            x.itemConfig.itemData == itemConfig.itemData
        );

        if (slot != null)
        {
            slot.amount += amount;
        }
        else
        {
            bagItemSlots.Add(new BagItemSlot
            {
                itemConfig = itemConfig,
                amount = amount
            });
        }

        OnBagItemChanged?.Invoke();
    }

    public void RemoveItem(BagItemConfig itemConfig, int amount)
    {
        if (itemConfig == null || itemConfig.itemData == null || amount <= 0)
            return;

        BagItemSlot slot = bagItemSlots.Find(x =>
            x.itemConfig != null &&
            x.itemConfig.itemData == itemConfig.itemData
        );

        if (slot == null) return;

        slot.amount -= amount;

        if (slot.amount <= 0)
        {
            bagItemSlots.Remove(slot);
        }

        OnBagItemChanged?.Invoke();
    }

    public bool TryUseItem(
        EBagItemCategory category,
        int amount = 1,
        ESeedsCircleOptionType seedType = ESeedsCircleOptionType.none
    )
    {
        if (amount <= 0) return false;

        BagItemSlot slot = FindItemSlot(category, amount, seedType);

        return slot != null;
    }

    public bool DecreaseItemAmount(
        EBagItemCategory category,
        int amount = 1,
        ESeedsCircleOptionType seedType = ESeedsCircleOptionType.none
    )
    {
        if (amount <= 0) return false;

        BagItemSlot slot = FindItemSlot(category, amount, seedType);

        if (slot == null)
        {
            return false;
        }

        slot.amount -= amount;

        if (slot.amount <= 0)
        {
            bagItemSlots.Remove(slot);
        }

        OnBagItemChanged?.Invoke();

        return true;
    }

    private BagItemSlot FindItemSlot(
        EBagItemCategory category,
        int amount,
        ESeedsCircleOptionType seedType
    )
    {
        return bagItemSlots.Find(x =>
            x != null &&
            x.itemConfig != null &&
            x.itemConfig.category == category &&
            x.amount >= amount &&
            IsCorrectItemType(x.itemConfig, category, seedType)
        );
    }

    private bool IsCorrectItemType(
        BagItemConfig itemConfig,
        EBagItemCategory category,
        ESeedsCircleOptionType seedType
    )
    {
        if (category == EBagItemCategory.seed)
        {
            return itemConfig.seedType == seedType;
        }

        return true;
    }
}