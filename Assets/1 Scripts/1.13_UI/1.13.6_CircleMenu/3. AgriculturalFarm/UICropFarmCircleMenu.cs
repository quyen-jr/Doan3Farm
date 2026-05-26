using UnityEngine;
using UnityEngine.UI;

public class UICropFarmCircleMenu : UICircleMenuBase
{
    [SerializeField] private Button _buttonClose;

    private void OnEnable()
    {
        _buttonClose.onClick.AddListener(OnClickButtonClose);
    }

    private void OnDisable()
    {
        _buttonClose.onClick.RemoveListener(OnClickButtonClose);
    }

    private void OnClickButtonClose()
    {
        UICircleMenuMgr.instance.CloseAll();
    }

    protected override void OnSelectOption()
    {
        base.OnSelectOption();

        UICropFarmCircleMenuOption option = (UICropFarmCircleMenuOption)_uICircleMenuListOptions.GetOptionByIndex(_currentIndexPiece);

        Debug.Log("Crop circle menu select " + option.circleOptionConfig.eCropFarmCircleOptionType);

        if (option.circleOptionConfig.eCropFarmCircleOptionType == ECropFarmCircleOptionType.hoe)
        {
            UIController.Instance.GetCurrentSelectedLandPlot().DoAction(LandPlot.ActionType.Hoeing);
        }
        if (option.circleOptionConfig.eCropFarmCircleOptionType == ECropFarmCircleOptionType.rake)
        {
            UIController.Instance.GetCurrentSelectedLandPlot().DoAction(LandPlot.ActionType.Ranking);
        }
        if (option.circleOptionConfig.eCropFarmCircleOptionType == ECropFarmCircleOptionType.plant_seeds)
        {
            UICircleMenuMgr.instance.Open(ECircleMenu.seeds);
        }
        if (option.circleOptionConfig.eCropFarmCircleOptionType == ECropFarmCircleOptionType.harvest)
        {
            UIController.Instance.GetCurrentSelectedLandPlot().DoAction(LandPlot.ActionType.Haverst);
        }
        if (option.circleOptionConfig.eCropFarmCircleOptionType == ECropFarmCircleOptionType.watering)
        {
            UIController.Instance.GetCurrentSelectedLandPlot().DoAction(LandPlot.ActionType.Watering);
        }

        if (option.circleOptionConfig.eCropFarmCircleOptionType == ECropFarmCircleOptionType.fertilize)
        {
            bool hasFertilizer = BagItemManager.Instance.TryUseItem(
                EBagItemCategory.fertilizer,
                1
            );

            if (!hasFertilizer)
            {
                Debug.Log("Không có phân bón trong balo!");
                UICircleMenuMgr.instance.CloseAll();
                return;
            }

            UIController.Instance.GetCurrentSelectedLandPlot()
                .DoAction(LandPlot.ActionType.Fertilizer);
            UICircleMenuMgr.instance.CloseAll();
            return;
        }

        if (option.circleOptionConfig.eCropFarmCircleOptionType == ECropFarmCircleOptionType.pesticide)
        {
            bool hasPesticide = BagItemManager.Instance.TryUseItem(
                EBagItemCategory.pesticide,
                1
            );

            if (!hasPesticide)
            {
                Debug.Log("Không có thuốc trừ sâu trong balo!");
                UICircleMenuMgr.instance.CloseAll();
                return;
            }

            UIController.Instance.GetCurrentSelectedLandPlot()
                .DoAction(LandPlot.ActionType.Pesticedes);
            UICircleMenuMgr.instance.CloseAll();

            return;
        }


        if (option.circleOptionConfig.eCropFarmCircleOptionType == ECropFarmCircleOptionType.none)
        {
            UIController.Instance.SetCurrentSelectedLandPlot(null);
            UIController.Instance.SetCurrentSelectedSmallPlot(null);
        }

        if (option.circleOptionConfig.eCropFarmCircleOptionType != ECropFarmCircleOptionType.plant_seeds)
        {
            UICircleMenuMgr.instance.CloseAll();
        }
    }
}
