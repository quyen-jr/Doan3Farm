using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public GameObject PlayerControl;
    //  public GameObject LandRightJoystick;
    // public GameObject IntermediaryInventory;
    public Text PlayerMoney;
    //public InteractMenu InteractMenu;
    //  public RectTransform EquippedItemMenu;
    //  public RectTransform ItemSlots;
    public LandInteraction landInteraction;
    private GraphicRaycaster _graphicRaycaster;

    public Player player;

    //  [SerializeField]
    //   private GameObject taskPanel;
    //    [SerializeField]
    //   private GameObject archivementPanel;
    //[SerializeField]
    //  private AgriculturalProductsStore _agriculturalProductsStore;

    //[Header("Color")]
    //[SerializeField] private Color hoverColor;
    //[SerializeField] private Color baseColor;
    //[SerializeField] private Color pressColor;

    //[Header("Circle Menu Info")]
    //private int totalSLotCircleMenu = 7;
    //[SerializeField] List<ItemData> initialDataCircleMenu;
    //[SerializeField] List<ItemSlot> circleList;
    //[SerializeField] private Transform cirleMenuPanel;
    //[SerializeField] private TextMeshProUGUI mutipleCircleListIText;
    //[SerializeField] private Button leftChangeCircleBtn;
    //[SerializeField] private Button rightChangeCircleBtn;
    //[SerializeField] private Button backToInitialCircleBtn;
    //[SerializeField] private int currentTabInMultipleCirleMenu;
    //[SerializeField] private ItemSlot turnOffCircleMenuItem;
    //private List<ItemData> currentMultipleCircleList;

    //[Header("Crop Info Slider")]
    //    [SerializeField] private Slider timeSlider;


    [Header("Problem Incorrect Process")]
    [SerializeField] private Transform warningProcessParent;
    [SerializeField] GameObject warningImageUIPrefab;
    [SerializeField] private Sprite warningProcessSprite;
    [SerializeField] private Sprite warningWaterSprite;
    [SerializeField] private Sprite warningFertilizerSprite;
    [SerializeField] private Sprite warningHasWormSprite;
    Dictionary<SmallPlot, List<WarningCropImage>> warningDictionary = new Dictionary<SmallPlot, List<WarningCropImage>>();

    [Header("Way Point")]
    [SerializeField] private WarningWayPoint wayPoint;


    public LandPlot currentLandPlotSelected;
    public SmallPlot currentSmallPlotSelected;
    private int selection;
    private int previousSelection = -1;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _graphicRaycaster = GetComponent<GraphicRaycaster>();
    }
    private void Start()
    {
        //   landInteraction.AddTools();
        //  SetUpCirleMenuButton();
        //  AssignStoreAction();
    }



    private void Update()
    {
        //  HoverCirclUI();
        // DisplayCropSelectedInfomationUI();
    }
    private void AssignStoreAction()
    {
        // _agriculturalProductsStore.OnStoreOpen += Player.LocalPlayer.playerInputEvent.SwitchActionMapMenu;
        //   _agriculturalProductsStore.OnStoreClose += Player.LocalPlayer.playerInputEvent.SwitchActionMapPlayer;
    }
    private void UnAssignStoreAction()
    {
        //s  _agriculturalProductsStore.OnStoreOpen -= Player.LocalPlayer.playerInputEvent.SwitchActionMapMenu;
    }
    public bool IsTouchingUI(Vector2 touchPos)
    {

        if (! player.photonView.IsMine)
        {
            return false;
        }

        List<RaycastResult> results = new();
        PointerEventData pointerEventData = new(EventSystem.current);
        pointerEventData.position = touchPos;

        _graphicRaycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            for (int i = results.Count - 1; i >= 0; i--)
            {
                if (results[i].gameObject.CompareTag("RightJoystick"))
                {
                    results.RemoveAt(i);
                }
            }
        }

        return results.Count > 0 ? results[0].gameObject : null;
    }
    public void TogglePlayerControl(bool toggle) => PlayerControl.SetActive(toggle);
    //   public void ToggleLandRightJoystick(bool toggle) => LandRightJoystick.SetActive(toggle);
    public void ToggleInventory(bool toggle)
    {
      //  UIInventoryMgr.instance.ShowBag();
        if (toggle)
        {
            Player.LocalPlayer.playerInputEvent.SwitchActionMap(Player.ActionMap.Menu);
        }
    }

    public SmallPlot GetCurrentSelectedSmallPlot()
    {
        return currentSmallPlotSelected;
    }
    public void SetCurrentSelectedSmallPlot(SmallPlot _selectedSmallPlot)
    {
        currentSmallPlotSelected = _selectedSmallPlot;
    }
    public LandPlot GetCurrentSelectedLandPlot()
    {
        return currentLandPlotSelected;
    }
    public void SetCurrentSelectedLandPlot(LandPlot _selectedPlot)
    {
        currentLandPlotSelected = _selectedPlot;
    }

    #region UI when select Plot

    //private void SetUpCirleMenuButton()
    //{
    //    backToInitialCircleBtn.onClick.AddListener(() =>
    //    {
    //        LoadCircleMenuContent(initialDataCircleMenu);
    //        TurnOffAllCircleButtonSide();
    //        currentTabInMultipleCirleMenu = 0;
    //        turnOffCircleMenuItem.gameObject.SetActive(true);
    //    });
    //    leftChangeCircleBtn.onClick.AddListener(() => {
    //        currentTabInMultipleCirleMenu--;
    //        if(currentTabInMultipleCirleMenu<=0)
    //        {
    //            LoadCircleMenuContent(initialDataCircleMenu);
    //            TurnOffAllCircleButtonSide();
    //            currentTabInMultipleCirleMenu = 0;
    //            turnOffCircleMenuItem.gameObject.SetActive(true);
    //            return;
    //        }
    //        double result = Math.Ceiling(currentMultipleCircleList.Count / 7f);
    //        UpdateCircleTextList(currentTabInMultipleCirleMenu.ToString(), result.ToString());
    //        LoadMultipleCircleByIndexTab(currentMultipleCircleList, currentTabInMultipleCirleMenu); 

    //    });
    //    rightChangeCircleBtn.onClick.AddListener(() => {
    //        double result = Math.Ceiling(currentMultipleCircleList.Count / 7f);
    //        if (currentTabInMultipleCirleMenu + 1 > result) return;
    //        currentTabInMultipleCirleMenu++;
    //        UpdateCircleTextList(currentTabInMultipleCirleMenu.ToString(),result.ToString());
    //        LoadMultipleCircleByIndexTab(currentMultipleCircleList, currentTabInMultipleCirleMenu);

    //    });
    //}
    //private void UpdateCircleTextList(string _currenTabIndex, string _totalTabIndex)
    //{
    //    mutipleCircleListIText.text = _currenTabIndex + "/" + _totalTabIndex;
    //}
    //private void TurnOffAllCircleButtonSide()
    //{
    //    turnOffCircleMenuItem.gameObject.SetActive(false);
    //    mutipleCircleListIText.gameObject.SetActive(false);
    //    leftChangeCircleBtn.gameObject.SetActive(false);
    //    rightChangeCircleBtn.gameObject.SetActive(false);
    //    backToInitialCircleBtn.gameObject.SetActive(false);
    //}
    //public void ProcessMultipleCircleMenu(List<ItemData> _cirlceList)
    //{
    //    double result = Math.Ceiling(_cirlceList.Count /(float) totalSLotCircleMenu);
    //    currentMultipleCircleList = _cirlceList;
    //    if (result > 1)
    //    {
    //        TurnOffAllCircleButtonSide();
    //        currentTabInMultipleCirleMenu = 1;
    //        UpdateCircleTextList(currentTabInMultipleCirleMenu.ToString(), result.ToString());
    //        LoadMultipleCircleByIndexTab(currentMultipleCircleList, currentTabInMultipleCirleMenu);


    //        mutipleCircleListIText.gameObject.SetActive(true);
    //        leftChangeCircleBtn.gameObject.SetActive(true);
    //        rightChangeCircleBtn.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        TurnOffAllCircleButtonSide();
    //        backToInitialCircleBtn.gameObject.SetActive(true);
    //        LoadCircleMenuContent(_cirlceList);
    //    }
    //}
    //    private void LoadMultipleCircleByIndexTab(List<ItemData> itemList,int _index)
    //    {
    //        foreach (ItemSlot item in circleList)
    //        {
    //            item.itemData = null;
    //            item.image.gameObject.SetActive(false);
    //        }

    //        int startIndex = totalSLotCircleMenu*(_index-1);
    //        int indexHasPass = 0;
    //    //    Debug.Log(startIndex+" " + itemList.Count);
    //       //// int endIndex=(itemList.Count< startIndex+6)? itemList.Count:startIndex+6;
    ////        Debug.Log(TutorialController.Instance.GetCurrentStep());
    //        //if(TutorialController.Instance.CheckTutorial("Planting", 8)){
    //        //    ToggleCircleUI(false);
    //        //    TutorialController.Instance.RunTutorial();
    //        //}

    //        for (int i = startIndex; i < itemList.Count; i++)
    //        {

    //            if (indexHasPass > totalSLotCircleMenu-1) return;
    //            circleList[indexHasPass].itemData = itemList[i];
    //            circleList[indexHasPass].image.gameObject.SetActive(true);
    //            circleList[indexHasPass].DisplayInfo();
    //            indexHasPass++;
    //        }

    //    }
    //    public void LoadCircleMenuContent(List<ItemData> itemList)
    //    {
    //        foreach (ItemSlot item in circleList)
    //        {
    //            item.itemData = null;
    //            item.image.gameObject.SetActive(false);
    //            ItemSlot previousItemSlot = item.GetComponent<ItemSlot>();
    //            previousItemSlot.GetComponent<Image>().color = baseColor;
    //        }
    //        for (int i = 0; i < itemList.Count; i++)
    //        {
    //            circleList[i].itemData = itemList[i];
    //            circleList[i].image.gameObject.SetActive(true);
    //            circleList[i].DisplayInfo();
    //        }
    //    }
    //private void DisplayCropSelectedInfomationUI()
    //{
    //    //display time to growth
    //    if (currentSmallPlotSelected)
    //    {
    //        if (currentSmallPlotSelected.GetCurrentCrop() != null)
    //        {
    //            if (!timeSlider.gameObject.activeSelf)
    //            {
    //                timeSlider.gameObject.SetActive(true);
    //            }
    //            float timeToRipe = currentSmallPlotSelected.GetCurrentCrop().GetHoursToRipe();
    //            float timeHasGrowth = currentSmallPlotSelected.GetCurrentCrop().GetCurrentGrowthTimeElapsed();
    //            timeSlider.value = timeHasGrowth / timeToRipe;
    //        }
    //        else
    //        {
    //            timeSlider.gameObject.SetActive(false);
    //        }
    //    }
    //}
    //private void HoverCirclUI()
    //{
    //     if (!Input.GetMouseButtonDown(0)) return;

    //    Vector2 normaliseMousePos = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
    //    float currentAngle = Mathf.Atan2(normaliseMousePos.y, normaliseMousePos.x) * Mathf.Rad2Deg;
    //    currentAngle = (currentAngle + 360) % 360;
    //    selection = (int)(currentAngle / (360f/7f));
    //   // Debug.Log(selection);
    //    if (selection != previousSelection && selection != circleList.Count)
    //    {
    //        if(previousSelection>=0){
    //            ItemSlot previousItemSlot = circleList[previousSelection].GetComponent<ItemSlot>();
    //            previousItemSlot.GetComponent<Image>().color = baseColor;
    //        }
    //        if(previousSelection>=0){
    //            ItemSlot currentItemSlot = circleList[selection].GetComponent<ItemSlot>();
    //            if (currentItemSlot.itemData == null) return;
    //            currentItemSlot.GetComponent<Image>().color = hoverColor;
    //            currentItemSlot.DisplayInfo();
    //        }

    //        previousSelection = selection;
    //    }
    //}
    //public void DisplayWarningProcess(SmallPlot _selectedSmallPlot, WarningType _warningType)
    //{
    //    //if (_selectedSmallPlot == null) return;
    //    //if (!_selectedSmallPlot.IsProcessIsRight())
    //    //{
    //    //    if (!warningDictionary.ContainsKey(_selectedSmallPlot))
    //    //    {
    //    //        GameObject warningImageObjectUI = Instantiate(warningImageUIPrefab, warningProcessParent);
    //    //        WarningCropImage warningCropImg = warningImageObjectUI.GetComponent<WarningCropImage>();
    //    //        warningCropImg.SetplotParent(_selectedSmallPlot.transform);






    //    //AddWarning(_selectedSmallPlot, warningCropImg);
    //    //  }
    //    // }
    //}
    public void AddWarningByType(SmallPlot _smallPlot, WarningType _warningType)
    {
        // N?u _smallPlot chýa có trong dictionary, t?o danh sách m?i
        if (_smallPlot == null) return;
        if (_warningType != WarningType.WrongProcess)
            wayPoint.AddWayPointByType(_smallPlot, _warningType);

        if (_warningType != WarningType.WrongProcess) return;
        if (!warningDictionary.ContainsKey(_smallPlot))
        {
            warningDictionary[_smallPlot] = new List<WarningCropImage>();
        }

        // Ki?m tra n?u ð? t?n t?i c?nh báo có cùng lo?i WarningType trong danh sách
        bool isWarningTypeExist = warningDictionary[_smallPlot]
            .Any(existingWarning => existingWarning.warningType == _warningType);

        if (!isWarningTypeExist)
        {
            // T?o ð?i tý?ng c?nh báo m?i và gán các thông tin c?n thi?t
            GameObject warningImageObjectUI = Instantiate(warningImageUIPrefab, warningProcessParent);
            WarningCropImage warningCropImg = warningImageObjectUI.GetComponent<WarningCropImage>();
            warningCropImg.SetplotParent(_smallPlot.transform);

            // Ch?n sprite týõng ?ng v?i lo?i c?nh báo
            Sprite spriteToSet = warningProcessSprite;
            warningCropImg.SetWarningImage(spriteToSet, _warningType);

            //      Thêm c?nh báo vào dictionary
            warningDictionary[_smallPlot].Add(warningCropImg);
        }
        else
        {
            Debug.LogWarning($"A warning of type {_warningType} already exists for this plot.");
        }
    }

    public void RemoveWarningByType(SmallPlot _smallPlot, WarningType type)
    {
        if (_smallPlot == null) return;
        wayPoint.RemoveWayPointByType(_smallPlot, type);
        if (warningDictionary.ContainsKey(_smallPlot))
        {
            List<WarningCropImage> warnings = warningDictionary[_smallPlot];
            for (int i = warnings.Count - 1; i >= 0; i--)
            {
                if (warnings[i].warningType == type)
                {
                    warnings[i].DestroyWarningImage();

                    warnings.RemoveAt(i);
                }
            }

            if (warnings.Count == 0)
            {
                warningDictionary.Remove(_smallPlot);
            }
        }
    }

    public void ToggleCircleUI(bool _isEnable)
    {
        if (_isEnable)
        {
            UICircleMenuMgr.instance.Open(ECircleMenu.cropFarm);
        }
        // TurnOffAllCircleButtonSide();
        // turnOffCircleMenuItem.gameObject.SetActive(_isEnable);
        // cirleMenuPanel.gameObject.SetActive(_isEnable);
        // LoadCircleMenuContent(initialDataCircleMenu);
    }
    // public bool CircleMenuIsActive() => cirleMenuPanel.gameObject.activeSelf;
    #endregion

    public void TogglePanel(GameObject panel, bool _isEnable)
    {
        panel.SetActive(_isEnable);
        if (_isEnable)
        {
            Player.LocalPlayer.playerInputEvent.SwitchActionMap(Player.ActionMap.Menu);
        }
        else
        {
            Player.LocalPlayer.playerInputEvent.SwitchActionMap(Player.ActionMap.Player);
        }
    }
    //public void ToggleTaskPanel(bool _isEnable){
    //    TogglePanel(taskPanel, _isEnable);
    //}
    //public void ToggleArchivementPanel(bool _isEnable){
    //    TogglePanel(archivementPanel, _isEnable);
    //}

}

