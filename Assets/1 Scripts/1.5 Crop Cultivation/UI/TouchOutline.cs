using UnityEngine;
using UnityEngine.EventSystems;

public class TouchOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform outline;

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
    private void OnMouseDown()
    {
        outline.gameObject.SetActive(true);
        // enable circle UI if this is a crop
        EnableCircleUI();
    }
    private void OnMouseExit()
    {
        outline.gameObject.SetActive(false);
    }
    private void EnableCircleUI()
    {
        //if (GetComponentInParent<Crop>() == null) return;
        //if (Player.LocalPlayer.playerMovement.IsbusyDoingAction()) return;
        //LandPlot currentPlot= GetComponentInParent<Crop>().GetCurrentPlot();       
        //UIController.Instance.SetCurrentSelectedPlot(currentPlot);
        //UIController.Instance.ToggleCircleUI(true);
    }
}

