using UnityEngine;
using UnityEngine.UI;

public class BagUIScreen : UIScreen
{
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
}
