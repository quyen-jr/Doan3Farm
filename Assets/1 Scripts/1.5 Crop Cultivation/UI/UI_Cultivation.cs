using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Cultivation : MonoBehaviour
{
    [NonSerialized] public static UI_Cultivation Instance;
    [SerializeField] Button cancleButton;
    [Header("Content")]
    [SerializeField] private Transform contentPanel;
    [SerializeField] private Transform seedUI;
    [SerializeField] private Transform toolUI;
    [SerializeField] private Transform additionUI; // water,, fertilizer, pesticide 
    private LandPlot currentPlotSelected;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    private void Start()
    {
        cancleButton.onClick.AddListener(() => { DisablePanelUI(); });
    }
    public void ToggleUI(bool _isEnable)
    {
        contentPanel.gameObject.SetActive(_isEnable);
    }

    public void ToggleSeedUI(bool _isEnable)
    {
        DisableActionUI();
        seedUI.gameObject.SetActive(_isEnable);
    }
    public void ToggleHaverstUI(bool _isEnable)
    {
        DisableActionUI();
        additionUI.gameObject.SetActive(_isEnable);
    }
    public void ToggleToolUI(bool _isEnable)
    {
        DisableActionUI();
        toolUI.gameObject.SetActive(_isEnable);
    }
    private void DisableActionUI()
    {
        seedUI.gameObject.SetActive(false);
        toolUI.gameObject.SetActive(false);
        additionUI.gameObject.SetActive(false);
    }
    public void DisablePanelUI()
    {
        DisableActionUI();
        contentPanel.gameObject.SetActive(false);
        SetCurrentSelectedPlot(null);
    }

    public LandPlot GetCurrentSelectedPlot()
    {
        return currentPlotSelected;
    }
    public void SetCurrentSelectedPlot(LandPlot _selectedPlot)
    {
        currentPlotSelected = _selectedPlot;
    }
}
