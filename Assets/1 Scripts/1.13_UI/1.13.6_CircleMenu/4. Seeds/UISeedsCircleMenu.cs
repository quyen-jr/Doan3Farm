using UnityEngine;

public class UISeedsCircleMenu : UICircleMenuBase
{
    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void OnClickButtonClose()
    {
        UICircleMenuMgr.instance.CloseAll();
    }

    protected override void OnSelectOption()
    {
        base.OnSelectOption();

        UISeedsCircleMenuOption option =
            (UISeedsCircleMenuOption)_uICircleMenuListOptions.GetOptionByIndex(_currentIndexPiece);

        if (option == null || option.seedBagItem == null)
        {
            UICircleMenuMgr.instance.CloseAll();
            return;
        }

        if (option.seedBagItem.seedType == ESeedsCircleOptionType.none)
        {
            UICircleMenuMgr.instance.CloseAll();
            return;
        }

        bool hasSeed = BagItemManager.Instance.TryUseItem(
            EBagItemCategory.seed,
            1,
            option.seedBagItem.seedType
        );

        if (!hasSeed)
        {
            Debug.Log("Không còn hạt giống trong balo!");
            UICircleMenuMgr.instance.CloseAll();
            return;
        }

        BagItemManager.Instance.DecreaseItemAmount(
            EBagItemCategory.seed,
             1,
         option.seedBagItem.seedType
        );

        UIController.Instance.GetCurrentSelectedLandPlot().DoPlant(option.seedBagItem);

        UICircleMenuMgr.instance.CloseAll();
    }
}
