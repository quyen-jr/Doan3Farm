using System;

[Serializable]
public class BagItemConfig
{
    public EBagItemCategory category;
    public ItemData itemData;
    // Chỉ dùng khi category là seed
    public ESeedsCircleOptionType seedType;
}
public enum EBagItemCategory
{
    seed,
    fertilizer,
    pesticide,
    other
}