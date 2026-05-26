using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemSlot : MonoBehaviour
{
    public ItemData itemData;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textAmount;
    public Image image;

    private Button button;
    void Start()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() => { ImplementActionUI(); });
    }


    private void ImplementActionUI()
    {
        //if (itemData == null) return;
        //if (UIController.Instance.GetCurrentSelectedLandPlot() == null)
        //{
        //    Debug.Log(UIController.Instance.GetCurrentSelectedLandPlot());
        //    UIController.Instance.ToggleCircleUI(false);
        //    return;
        //}
        //// check if  item ui is a list circle or not
        //if (itemData.nextCircleData.Count > 0)
        //{
        //   // UIController.Instance.LoadCircleMenuContent(itemData.nextCircleData);
        //   UIController.Instance.ProcessMultipleCircleMenu(itemData.nextCircleData);
        //    return;
        //}
        //if (Player.LocalPlayer.playerAnimation.IsLockingTransition()){
        //    Debug.Log("lock transition");
        //    return;
        //}

        //if (itemData.type == ItemType.Hoe)
        //{
        //    UIController.Instance.GetCurrentSelectedLandPlot().DoAction(LandPlot.ActionType.Hoeing);
        //}
        //if (itemData.type == ItemType.Pitchfork)
        //{
        //    UIController.Instance.GetCurrentSelectedLandPlot().DoAction(LandPlot.ActionType.Ranking);
        //}
        //if (itemData.type == ItemType.Crop) 
        //{
        //    UIController.Instance.GetCurrentSelectedLandPlot().DoPlant(itemData);
        //}
        //if (itemData.type == ItemType.Haverst)
        //{
        //    UIController.Instance.GetCurrentSelectedLandPlot().DoAction(LandPlot.ActionType.Haverst);
        //}
        //if (itemData.type == ItemType.Water)
        //{
        //    UIController.Instance.GetCurrentSelectedLandPlot().DoAction(LandPlot.ActionType.Watering);
        //}
        //if (itemData.type == ItemType.Fertilizer)
        //{
        //    UIController.Instance.GetCurrentSelectedLandPlot().DoAction(LandPlot.ActionType.Fertilizer);
        //}
        //if (itemData.type == ItemType.Pesticides)
        //{
        //    UIController.Instance.GetCurrentSelectedLandPlot().DoAction(LandPlot.ActionType.Pesticedes);
        //}

        //if (itemData.type == ItemType.Cancle)
        //{
        //    UIController.Instance.SetCurrentSelectedLandPlot(null);
        //    UIController.Instance.SetCurrentSelectedSmallPlot(null);
        //}
        //Player.LocalPlayer.playerInputEvent.SwitchActionMapPlayer();

        //UIController.Instance.ToggleCircleUI(false);

    }

    public void DisplayInfo()
    {
        image.sprite = itemData.sprite;
        textName.text = itemData.itemName;
        if (itemData.type == ItemType.Crop)
            textAmount.text = "Amount : X" + itemData.GetAmount().ToString();
        else textAmount.text = "";
    }

}

