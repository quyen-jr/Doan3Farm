using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleMenuUI : MonoBehaviour
{
    [NonSerialized] public static CircleMenuUI Instance;

    [Header("Color")]
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color pressColor;

    [Header("Circle Menu Info")]
    [SerializeField] List<ItemData> initialDataCircleMenu;
    [SerializeField] List<ItemSlot> circleList;
    [SerializeField] private Transform cirleMenuPanel;

    [Header("Crop Info Slider")]
    [SerializeField] private Slider timeSlider;
    [SerializeField] private Image sliderIcon;


    [Header("Problem Icon UI")]
    [SerializeField] private List<Image> problemImage;
    [SerializeField] private Sprite fertilizerSprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite pesticidesSprite;

    private LandPlot currentPlotSelected;
    private int selection;
    private int previousSelection;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    void Update()
    {
        HoverCirclUI();
        DisplayCropSelectedInfomationUI();
    }


    public LandPlot GetCurrentSelectedPlot()
    {
        return currentPlotSelected;
    }
    public void SetCurrentSelectedPlot(LandPlot _selectedPlot)
    {
        currentPlotSelected = _selectedPlot;
    }

    #region UI when select Plot
    public void LoadCircleMenuContent(List<ItemData> itemList)
    {
        //foreach (ItemSlot item in circleList)
        //{
        //    item.itemData = null;
        //    item.image.gameObject.SetActive(false);
        //}
        //for (int i = 0; i < itemList.Count; i++)
        //{
        //    if (currentPlotSelected != null)
        //    {
        //        if (currentPlotSelected.IsFree() && !currentPlotSelected.IsRanking())
        //        {
        //            if (!(itemList[i].type == ItemType.Hoe || itemList[i].type == ItemType.Pitchfork || itemList[i].type == ItemType.Cancle)) continue;
        //        }
        //        if (currentPlotSelected.IsFree() && currentPlotSelected.IsRanking())
        //        {
        //            if (!(itemList[i].type == ItemType.NextSeedCircle || itemList[i].type == ItemType.Crop || itemList[i].type == ItemType.Cancle)) continue;
        //        }
        //        if (currentPlotSelected.GetCurrentCrop() != null)
        //        {
        //            if (currentPlotSelected.GetCurrentCrop().IsRipe())
        //            {
        //                if (!(itemList[i].type == ItemType.Haverst || itemList[i].type == ItemType.Cancle)) continue;
        //            }
        //            else
        //            {
        //                // check if crop has problem ==> displlay ui in circle menu
        //                bool displayProblemOption = false;
        //                Crop currentCrop = currentPlotSelected.GetCurrentCrop();
        //                if (itemList[i].type == ItemType.Water && currentCrop.IsLackWater() ||
        //                    itemList[i].type == ItemType.Fertilizer && currentCrop.IsLackOfFertilizer() ||
        //                    itemList[i].type == ItemType.Pesticides && currentCrop.IsHasWorn() || itemList[i].type == ItemType.Cancle
        //                    )
        //                {
        //                    displayProblemOption = true;
        //                }
        //                if (!displayProblemOption) continue;

        //            }

        //        }
        //    }
        //    circleList[i].itemData = itemList[i];
        //    circleList[i].image.gameObject.SetActive(true);
        //    circleList[i].DisplayInfo();
        //}
    }
    private void DisplayCropSelectedInfomationUI()
    {
        //// display time to growth
        //if (currentPlotSelected)
        //{
        //    if (currentPlotSelected.GetCurrentCrop() != null)
        //    {
        //        if (!timeSlider.gameObject.activeSelf)
        //        {
        //            timeSlider.gameObject.SetActive(true);
        //        }
        //        float timeToRipe = currentPlotSelected.GetCurrentCrop().GetHoursToRipe();
        //        float timeHasGrowth = currentPlotSelected.GetCurrentCrop().GetCurrentGrowthTimeElapsed();
        //        timeSlider.value = timeHasGrowth / timeToRipe;
        //    }
        //    else
        //    {
        //        timeSlider.gameObject.SetActive(false);
        //    }
        //}
    }
    private void HoverCirclUI()
    {
        Vector2 normaliseMousePos = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
        float currentAngle = Mathf.Atan2(normaliseMousePos.y, normaliseMousePos.x) * Mathf.Rad2Deg;
        currentAngle = (currentAngle + 360) % 360;
        selection = (int)currentAngle / 45;
        if (selection != previousSelection)
        {
            ItemSlot previousItemSlot = circleList[previousSelection].GetComponent<ItemSlot>();
            previousItemSlot.GetComponent<Image>().color = baseColor;

            previousSelection = selection;
            ItemSlot currentItemSlot = circleList[selection].GetComponent<ItemSlot>();
            if (currentItemSlot.itemData == null) return;
            currentItemSlot.GetComponent<Image>().color = hoverColor;

            currentItemSlot.DisplayInfo();
        }
    }
    private void DisplayProblemUI()
    {

        //if (GetCurrentSelectedPlot() == null) return;
        //if (GetCurrentSelectedPlot().GetCurrentCrop() == null) return;

        //foreach (Image iconImage in problemImage)
        //{
        //    iconImage.gameObject.SetActive(false);
        //}
        //if (GetCurrentSelectedPlot().GetCurrentCrop().IsRipe()) return;
        //// set icon problem 
        //bool isSetFertilizerIcon = false;
        //bool isSetWitheringIcon = false;
        //bool isSetPesticideIcon = false;


        //foreach (Image iconImage in problemImage)
        //{
        //    Debug.Log("set image");
        //    if (GetCurrentSelectedPlot().GetCurrentCrop().IsLackOfFertilizer() && !isSetFertilizerIcon)
        //    {
        //        iconImage.sprite = fertilizerSprite;
        //        isSetFertilizerIcon = true;
        //        iconImage.gameObject.SetActive(true);
        //    }
        //    else
        //    if (GetCurrentSelectedPlot().GetCurrentCrop().IsLackWater() && !isSetWitheringIcon)
        //    {
        //        iconImage.sprite = waterSprite;
        //        isSetWitheringIcon = true;
        //        iconImage.gameObject.SetActive(true);
        //    }
        //    else
        //    if (GetCurrentSelectedPlot().GetCurrentCrop().IsHasWorn() && !isSetPesticideIcon)
        //    {
        //        iconImage.sprite = pesticidesSprite;
        //        isSetPesticideIcon = true;
        //        iconImage.gameObject.SetActive(true);
        //    }
        //}

    }
    public void ToggleCircleUI(bool _isEnable)
    {
        cirleMenuPanel.gameObject.SetActive(_isEnable);
        DisplayProblemUI();
        LoadCircleMenuContent(initialDataCircleMenu);
    }
    #endregion
}
