using UnityEngine;
using UnityEngine.UI;

public class RealEstateScreen : UIScreen
{

    [SerializeField] GameObject _UIBuyOption;
    [SerializeField] Button _closeButton;

    void Start()
    {

    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(OnCloseScreen);
    }


    private void OnDisable()
    {
        _closeButton?.onClick.RemoveListener(OnCloseScreen);
    }

    void Update()
    {

    }


    public void OnCloseScreen()
    {
        ScreenGameManager.instance.CloseAll();
    }

    public void OpenUIBuyOption()
    {
        _UIBuyOption.SetActive(true);
    }
    public void CloseUIBuyOption()
    {
        _UIBuyOption.SetActive(false);
    }
}
