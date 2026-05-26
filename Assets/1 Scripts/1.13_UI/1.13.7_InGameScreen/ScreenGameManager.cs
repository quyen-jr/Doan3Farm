using System.Collections.Generic;
using UnityEngine;

public class ScreenGameManager : Singleton<ScreenGameManager>
{
    private List<UIScreen> _listUIScreen;

    [SerializeField] private GameObject _imageBackground;

    protected override void Awake()
    {
        base.Awake();
        AttachUIScreen();
    }

    private void AttachUIScreen()
    {
        _listUIScreen = new List<UIScreen>();

        UIScreen[] screens = GetComponentsInChildren<UIScreen>(true);

        for (int i = 0; i < screens.Length; i++)
        {
            _listUIScreen.Add(screens[i]);
        }

        Debug.Log("Số UI Screen tìm được: " + _listUIScreen.Count);
    }

    public T Open<T>() where T : UIScreen
    {
        if (_listUIScreen == null) return null;

        for (int i = 0; i < _listUIScreen.Count; i++)
        {
            T view = _listUIScreen[i] as T;

            if (view != null)
            {
                if (_imageBackground != null)
                    _imageBackground.SetActive(true);

                OpenScreen(view);
                return view;
            }
        }

        Debug.LogError("Không tìm thấy screen: " + typeof(T).Name);
        return null;
    }

    public void CloseAll()
    {
        if (_listUIScreen == null) return;

        for (int i = 0; i < _listUIScreen.Count; i++)
        {
            if (_listUIScreen[i] != null)
            {
                _listUIScreen[i].Close();
            }
        }

        if (_imageBackground != null)
            _imageBackground.SetActive(false);
    }

    private void OpenScreen(UIScreen screen)
    {
        if (screen == null) return;

        screen.Open();
    }

    public void CloseScreen(UIScreen screen)
    {
        if (screen == null) return;

        screen.Close();
    }
}