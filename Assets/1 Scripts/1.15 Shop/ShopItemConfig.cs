using System;
using UnityEngine;

[Serializable]
public class ShopItemConfig
{
    public string itemName;

    public BagItemConfig itemConfig;

    [Min(1)] public int price = 100;

    // Mua 1 lần sẽ cộng bao nhiêu item vào bag
    // Ví dụ hạt giống: 1 gói có 4 hạt
    [Min(1)] public int amountPerBuy = 1;

    [TextArea]
    public string description;
}