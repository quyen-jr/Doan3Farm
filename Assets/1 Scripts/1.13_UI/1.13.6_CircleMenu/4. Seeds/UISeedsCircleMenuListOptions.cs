using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISeedsCircleMenuListOptions : UICircleMenuListOptionsBase
{
    //[SerializeField] private List<SeedsCircleOptionConfig> circleOptionsConfig;

    [SerializeField] private Button _buttonBack;
    [SerializeField] private Button _buttonNext;
    [SerializeField] private Button _buttonPrevious;
    [SerializeField] private TextMeshProUGUI _textCurrentPage;

    private int _currentPage;
    private int _maxPage;

    protected override void Awake()
    {
        base.Awake();


    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _buttonBack.onClick.AddListener(BackToCropCircleMenu);
        _buttonNext.onClick.AddListener(GoToNextPage);
        _buttonPrevious.onClick.AddListener(GoToPreviousPage);

        int itemCount = BagItemManager.Instance.GetSlotsByCategory(EBagItemCategory.seed).Count;
        int maxPiece = (int)_uICircleMenuBase.MaxPiece;

        _maxPage = Mathf.CeilToInt((float)itemCount / maxPiece);
        _maxPage = Mathf.Max(1, _maxPage);

        _currentPage = 1;
        GoToPageNumber(_currentPage);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _buttonBack.onClick.RemoveListener(BackToCropCircleMenu);
        _buttonNext.onClick.RemoveListener(GoToNextPage);
        _buttonPrevious.onClick.RemoveListener(GoToPreviousPage);
    }

    protected override void FillData()
    {
        base.FillData();
    }

    private void BackToCropCircleMenu()
    {

    }

    private void GoToNextPage()
    {
        _currentPage = (_currentPage >= _maxPage) ? _currentPage : _currentPage + 1;

        GoToPageNumber(_currentPage);
    }

    private void GoToPreviousPage()
    {
        _currentPage = (_currentPage <= 1) ? _currentPage : _currentPage - 1;

        GoToPageNumber(_currentPage);
    }

    private void GoToPageNumber(int page)
    {
        List<BagItemSlot> slots = BagItemManager.Instance.GetSlotsByCategory(EBagItemCategory.seed);

        int optionCount = _uICircleMenuOptions.Count;
        int maxPiece = (int)_uICircleMenuBase.MaxPiece;

        int minIndex = (page - 1) * maxPiece;

        for (int i = 0; i < optionCount; i++)
        {
            int dataIndex = minIndex + i;

            UISeedsCircleMenuOption option = (UISeedsCircleMenuOption)_uICircleMenuOptions[i];

            if (dataIndex >= slots.Count)
            {
                option.InitData(null);
            }
            else
            {
                option.InitData(slots[dataIndex].itemConfig);
            }
        }

        UpdateTextCurrentPage();
    }

    private void UpdateTextCurrentPage()
    {
        _textCurrentPage.text = $"{_currentPage}/{_maxPage}";
    }
}

public enum ESeedsCircleOptionType
{
    none,
    bingoi,
    cantay,
    dualeo,
    hanh,
    ngo,
    peanut,
    salad,
    strawberry

}

//[System.Serializable]
//public class SeedsCircleOptionConfig
//{
//    public ESeedsCircleOptionType eSeedsCircleOptionType;
//    public ItemData itemData; //sử dụng tạm thời
//}