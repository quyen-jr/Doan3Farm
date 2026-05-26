using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFieldPlotsManager : MonoBehaviour
{
    [SerializeField] private List<UIFieldPlot> _listUIFieldPlot = new List<UIFieldPlot>();

    [SerializeField] private UIFieldPlot _currentSelectedUI;
    [SerializeField] Button _buyButton;

    private RealEstateScreen _screen;

    private void Start()
    {
        _listUIFieldPlot.Clear();

        UIFieldPlot[] plots = GetComponentsInChildren<UIFieldPlot>(true);
        _listUIFieldPlot.AddRange(plots);

        Debug.Log("Số ô đất UI tìm được: " + _listUIFieldPlot.Count);
        _screen=GetComponentInParent<RealEstateScreen>();
        InitializedData();
    }

    private void OnEnable()
    {
        if (_buyButton != null)
        {
            _buyButton.onClick.RemoveListener(BuyFieldPlot);
            _buyButton.onClick.AddListener(BuyFieldPlot);
        }
    }

    private void OnDisable()
    {
        if (_buyButton != null)
        {
            _buyButton.onClick.RemoveListener(BuyFieldPlot);
        }
    }
    private void InitializedData()
    {
        if (FieldPlotsManager.Instance == null)
        {
            Debug.LogError("Không tìm thấy FieldPlotsManager.Instance");
            return;
        }

        int count = Mathf.Min(
            _listUIFieldPlot.Count,
            FieldPlotsManager.Instance.landObjectsList.Count
        );

        Debug.Log("Số mảnh đất thật: " + FieldPlotsManager.Instance.landObjectsList.Count);

        for (int i = 0; i < count; i++)
        {
            _listUIFieldPlot[i].SetNumber(i);
            _listUIFieldPlot[i].SetState(FieldPlotsManager.Instance.landObjectsList[i].IsBought);
            _listUIFieldPlot[i].SetSelected(false);
        }
    }

    public void OnSelectUILandPlot(UIFieldPlot plot)
    {
        _currentSelectedUI = plot;

        Debug.Log(_currentSelectedUI);
        for (int i = 0; i < _listUIFieldPlot.Count; i++)
        {
            bool isSelected = _listUIFieldPlot[i] == plot;
            _listUIFieldPlot[i].SetSelected(isSelected);
        }

        if (FieldPlotsManager.Instance.landObjectsList[plot.GetNumberID()].IsBought)
        {
            _screen.CloseUIBuyOption();
        }
        else _screen.OpenUIBuyOption();

        Debug.Log("Đang chọn mảnh đất số: " + plot.FieldPlotNumber);
       // _currentSelectedUI = plot;
    }


    public void BuyFieldPlot()
    {
         Debug.Log(_currentSelectedUI);
        if (_currentSelectedUI != null) 
        FieldPlotsManager.Instance.landObjectsList[_currentSelectedUI.GetNumberID()].SetBuyThisField();
        _screen.CloseUIBuyOption();
        InitializedData();
        
    }
}


