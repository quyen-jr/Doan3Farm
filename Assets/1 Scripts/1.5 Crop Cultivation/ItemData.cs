using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Crop,
    Water,
    Fertilizer,
    Cancle,
    Pesticides,
    Hoe,
    Pitchfork,
    Haverst,
    NextSeedCircle
}
[CreateAssetMenu(fileName = "NewItemData", menuName = "CultivationData/ItemData", order = 1)]

[Serializable]
public class ItemData : ScriptableObject
{
    public GameObject prefabs;
    public string itemName;
    public Sprite sprite;
    public Sprite productSprite;
    public ItemType type;
    public int maxQuantityInStack;
    [SerializeField] private int amount;
    [TextArea(3, 10)]
    [SerializeField] private string description;
    public List<ItemData> nextCircleData;

    public float sellPrice;

    public int GetAmount()
    {
        return amount;
    }
    public void DecreaseAmount()
    {
        amount--;
    }
    public void IncreaseAmount(int _amount)
    {
        amount += _amount;
    }
    public string GetDescription()
    {
        return description;
    }
}
