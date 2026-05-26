using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : UIScreen
{
    [SerializeField] Button _closeButton;

    [SerializeField] Button _shopSaleButton;
    [SerializeField] Button _shopSellButton;

    [SerializeField] Transform _shopSale;
    [SerializeField] Transform _shopSell;

    [SerializeField] UINotification _notificationUI;
    void Start()
    {
        OpenShopSale();
    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(OnCloseScreen);

        _shopSaleButton.onClick.AddListener(OpenShopSale);
        _shopSellButton.onClick.AddListener(OpenShopSell);

        OpenShopSale();

    }


    private void OnDisable()
    {
        _closeButton?.onClick.RemoveListener(OnCloseScreen);

        _shopSaleButton.onClick.RemoveListener(OpenShopSale);
        _shopSellButton.onClick.RemoveListener(OpenShopSell);
    }

    void Update()
    {

    }


    public void OnCloseScreen()
    {
        ScreenGameManager.instance.CloseAll();
    }

    public void OpenShopSale()
    {
        _shopSale.gameObject.SetActive(true);
        _shopSell.gameObject.SetActive(false);
    }

    public void OpenShopSell()
    {
        _shopSale.gameObject.SetActive(false);
        _shopSell.gameObject.SetActive(true);
    }


    public void DisPlayNotification(string text, bool isSucces)
    {
        _notificationUI.gameObject.SetActive(true);
        _notificationUI.SetNotification(text, isSucces);
    }
}
