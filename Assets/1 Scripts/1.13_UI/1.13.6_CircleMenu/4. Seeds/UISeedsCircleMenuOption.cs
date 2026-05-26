public class UISeedsCircleMenuOption : UICircleMenuOptionBase
{
    public BagItemConfig seedBagItem;

    public void InitData(BagItemConfig seedsCircleOptionConfig)
    {
        if (seedsCircleOptionConfig == null)
        {
            Visible(false);
            seedBagItem = null;
            return;
        }

        if (seedsCircleOptionConfig.seedType == ESeedsCircleOptionType.none)
        {
            Visible(false);
            seedBagItem = seedsCircleOptionConfig;
            return;
        }

        if (seedsCircleOptionConfig.itemData == null)
        {
            Visible(false);
            seedBagItem = seedsCircleOptionConfig;
            return;
        }

        Visible(true);
        seedBagItem = seedsCircleOptionConfig;
        _imageOption.sprite = seedsCircleOptionConfig.itemData.sprite;
    }
}