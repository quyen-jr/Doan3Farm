using System.Collections.Generic;
using UnityEngine;

public class FieldPlots : MonoBehaviour
{

    public bool IsBought;
    [SerializeField] private GameObject fence;

    [SerializeField] private int _maxCropInSmallPlot;
    List<WayPointImage> wayPointImagesList = new List<WayPointImage>();
    private bool isPlayerInField = false;
    public void AddWayPointIMGToField(WayPointImage _wayPointImage)
    {
        if (_wayPointImage != null)
        {
            wayPointImagesList.Add(_wayPointImage);
        }
        else Debug.Log(" waypoint null");
    }
    public void RemoveWayPointIMGToFIeld(WayPointImage _wayPointImage)
    {
        if (wayPointImagesList.Contains(_wayPointImage))
        {
            wayPointImagesList.Remove(_wayPointImage);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        isPlayerInField = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerInField = false;
    }
    public bool CheckPlayerInField() => isPlayerInField;
    public int GetMaxCropInSmallPlot() => _maxCropInSmallPlot;


    public void SetBuyThisField()
    {
        IsBought = true;
        fence.SetActive(false);
    }
}
