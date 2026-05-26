using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICropFarmCircleMenuOption : UICircleMenuOptionBase
{
    public CropFarmCircleOptionConfig circleOptionConfig;

    public void InitData(CropFarmCircleOptionConfig cropFarmCircleOptionConfig)
    {
        if(cropFarmCircleOptionConfig.eCropFarmCircleOptionType == ECropFarmCircleOptionType.none)
        {
            Visible(false);
        }
        else 
        {
            Visible(true);
            circleOptionConfig = cropFarmCircleOptionConfig;
            _imageOption.sprite = cropFarmCircleOptionConfig.sprite;
        }
    }
}
