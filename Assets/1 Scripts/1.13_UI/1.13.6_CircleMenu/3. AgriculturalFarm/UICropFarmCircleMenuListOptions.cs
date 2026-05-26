using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICropFarmCircleMenuListOptions : UICircleMenuListOptionsBase
{
    [SerializeField] private List<CropFarmCircleOptionConfig> aFCircleOptions;

    protected override void FillData()
    {
        base.FillData();

        int index = 0;
        foreach(var data in aFCircleOptions)
        {
            ((UICropFarmCircleMenuOption)_uICircleMenuOptions[index]).InitData(data);
            index ++;
        }
    }
}

public enum ECropFarmCircleOptionType
{
    none,
    pesticide,
    harvest,
    hoe,
    rake,
    plant_seeds,
    watering,
    fertilize,
}

[System.Serializable]
public class CropFarmCircleOptionConfig
{
    public ECropFarmCircleOptionType eCropFarmCircleOptionType;
    public Sprite sprite;
}