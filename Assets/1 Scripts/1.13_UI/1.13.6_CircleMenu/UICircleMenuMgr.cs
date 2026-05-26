using UnityEngine;

public enum ECircleMenu
{
    chickenfarm,
    chickenFeed,
    cropFarm,
    seeds,
}

public class UICircleMenuMgr : Singleton<UICircleMenuMgr>
{
    [SerializeField] private GameObject _blurBackground;
    [SerializeField] private UICircleMenuBase[] _listCircleMenu;

    protected override void Awake()
    {
        base.Awake();
    }

    public UICircleMenuBase Open(ECircleMenu eCircleMenu)
    {
        if (Player.LocalPlayer != null) Player.LocalPlayer.playerInputEvent.SwitchActionMapMenu(); //khóa camera
        _blurBackground.SetActive(true);
        foreach (var ui in _listCircleMenu)
        {
            if (ui.eCircleMenu == eCircleMenu)
            {
                ui.Visible(true);
                return ui;
            }
            else
            {
                ui.Visible(false);
            }
        }

        return null;
    }

    public void CloseAll()
    {
        if (Player.LocalPlayer != null) Player.LocalPlayer.playerInputEvent.SwitchActionMapPlayer(); //mở khóa camera
        _blurBackground.SetActive(false);
        foreach (var ui in _listCircleMenu)
        {
            ui.Visible(false);
        }
    }
}


