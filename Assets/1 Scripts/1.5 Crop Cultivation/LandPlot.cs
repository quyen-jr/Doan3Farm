using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandPlot : MonoBehaviour
{
    public enum ActionType { Hoeing, Ranking, PlantCrop, Watering, Haverst, Pesticedes, Fertilizer, None }

    private ActionType currentActionType = ActionType.None;
    [SerializeField] private List<Transform> smallPlotList;
    //  [SerializeField] private bool testraking;
    private Dictionary<SmallPlot, bool> smallPlotHasWorkDic = new Dictionary<SmallPlot, bool>();

    // action 
    private bool isImplementActionInBigLandPlot;
    private bool inAction;
    //private bool isCancleAction;

    private Vector3 distancePlayerMustDoAction = Vector3.zero;

    private BagItemConfig currentItemDataCrop;
    private SmallPlot currentSmallPlot;

    private void Start()
    {
        foreach (Transform smallPlotTransform in smallPlotList)
        {
            SmallPlot smallPlot = smallPlotTransform.GetComponent<SmallPlot>();
            smallPlotHasWorkDic.Add(smallPlot, false);
        }
    }
    private void Update()
    {
        HandleProcess();
    }

    private void HandleProcess()
    {
        if (currentActionType == ActionType.None) return;
        if (isImplementActionInBigLandPlot)
        {
            ImplementAction(currentActionType);
        }
    }
    private void ImplementAction(ActionType _actionType)
    {
        float totalDistance = GetPlotSize();

        // check curren pos of player on LandPlot ( right or left)
        if (distancePlayerMustDoAction == Vector3.zero)
        {
            if (IsPlayerInRightSideOfPlot())
            {
                distancePlayerMustDoAction = new Vector3(transform.position.x - totalDistance / 2, transform.position.y, transform.position.z);
            }
            else
            {
                distancePlayerMustDoAction = new Vector3(transform.position.x + totalDistance / 2, transform.position.y, transform.position.z);
            }
        }



        float distanceToTarget = Vector3.Distance(Player.LocalPlayer.playerMovement.transform.position, distancePlayerMustDoAction);

        if (distanceToTarget >= 1f && !inAction)
        {
            if (currentSmallPlot == null)
            {
                if (ActionType.Ranking == currentActionType || ActionType.Hoeing == currentActionType || ActionType.PlantCrop == currentActionType)
                    currentSmallPlot = FindNearestSmallPlot();
                else if (!IgnoreThisSmallPlot(UIController.Instance.GetCurrentSelectedSmallPlot()))
                {
                    currentSmallPlot = UIController.Instance.GetCurrentSelectedSmallPlot();
                    Debug.Log(currentSmallPlot);
                }

            }

            if (currentSmallPlot == null)
            {
                Debug.Log("complete");
                //UIController.Instance.ToggleActionPanelUI(false);
                ResetAllAndEnablePlayerMovement();
                return;
            }
            //            Debug.Log(currentActionType+" " +currentSmallPlot.name);
            Player.LocalPlayer.playerMovement.CancleActionMoveToPlot();
            if (Player.LocalPlayer.playerMovement.IsMoveTo(currentSmallPlot.transform.position))
            {
                inAction = true;
                DoAnimationAfterReachSmallPlot(currentSmallPlot);
                currentSmallPlot = null;
            }
        }


        //update Progess UI when click player 
        float distanceTravelled = totalDistance - distanceToTarget;
        // UIController.Instance.UpdateActionTimeSlider(distanceTravelled, GetPlotSize() - 5);


    }
    private SmallPlot FindNearestSmallPlot()
    {

        PlayerMovement playerMovement = Player.LocalPlayer.playerMovement;
        SmallPlot closestPlot = null;
        float closestDistance = float.MaxValue;

        foreach (Transform _smallPlot in smallPlotList)
        {
            bool isHasWork = false;
            smallPlotHasWorkDic.TryGetValue(_smallPlot.GetComponent<SmallPlot>(), out isHasWork);
            if (isHasWork)
            {
                continue;
            }

            float distance = Vector3.Distance(playerMovement.transform.position, _smallPlot.position);
            if (distance < closestDistance && !IgnoreThisSmallPlot(_smallPlot.GetComponent<SmallPlot>()))
            {
                closestDistance = distance;
                closestPlot = _smallPlot.GetComponent<SmallPlot>();
            }
        }

        if (closestPlot != null)
        {
            smallPlotHasWorkDic[closestPlot] = true;
        }
        return closestPlot;
    }
    private bool IgnoreThisSmallPlot(SmallPlot _smallPlot)
    {
        bool isIgnoreCurrentSmallPlot = false;

        if (currentActionType == ActionType.Hoeing)
        {
            if (_smallPlot.GetCurrentCrop() == null && _smallPlot.IsHoeing())
            {
                isIgnoreCurrentSmallPlot = true;
            }

        }

        if (currentActionType == ActionType.Hoeing || currentActionType == ActionType.Ranking)
        {
            if (_smallPlot.GetCurrentCrop() != null)
            {
                isIgnoreCurrentSmallPlot = true;
            }

        }
        if (currentActionType == ActionType.Watering ||
            currentActionType == ActionType.Pesticedes)
        {
            if (_smallPlot.GetCurrentCrop() == null || (_smallPlot.GetCurrentCrop() != null && _smallPlot.CropIsRipe()))
            {
                isIgnoreCurrentSmallPlot = true;

            }

        }
        if (currentActionType == ActionType.Fertilizer)
        {
            if (_smallPlot.GetCurrentCrop() != null && _smallPlot.CropIsRipe())
            {
                isIgnoreCurrentSmallPlot = true;

            }
            else if (_smallPlot.GetCurrentCrop() == null)
            {
                bool canPreFertilize = _smallPlot.IsHoeing() && _smallPlot.IsRanking() && _smallPlot.IsProcessIsRight() && !_smallPlot.IsUseFertilizerBeforePlant();
                if (!canPreFertilize)
                {
                    isIgnoreCurrentSmallPlot = true;
                }
            }

        }


        if (currentActionType == ActionType.PlantCrop)
        {
            if (_smallPlot.GetCurrentCrop() != null)
            {
                isIgnoreCurrentSmallPlot = true;

            }

        }
        if (currentActionType == ActionType.Haverst)
        {

            if (_smallPlot.GetCurrentCrop() == null)
            {
                isIgnoreCurrentSmallPlot = true;
            }
            //else if (!_smallPlot.GetCurrentCrop().IsRipe())
            //{
            //    isIgnoreCurrentSmallPlot = true;
            //}
        }

        if (isIgnoreCurrentSmallPlot)
        {
            return true;
        }
        return false;

    }

    private void DoAnimationAfterReachSmallPlot(SmallPlot _currentSmallPlot)
    {
        if (!inAction) return;
        //  if (!isCanActionInPlanPlot) return;
        // reset  after do a action
        //  isCanActionInPlanPlot = false;


        SmallPlot currentSmallPlotToWork = _currentSmallPlot;
        UIController.Instance.SetCurrentSelectedSmallPlot(currentSmallPlotToWork);

        if (currentActionType == ActionType.PlantCrop)
        {
            if (currentSmallPlotToWork.IsFree())
            {
                Player.LocalPlayer.playerMovement.SetMoving(false);
                UIController.Instance.landInteraction.Plant();


                // StartCoroutine(PlantCrop(currentItemDataCrop, currentSmallPlotToWork));
            }
            else ResetActionAfterWorkInSmallPlot();
        }
        if (currentActionType == ActionType.Hoeing)
        {
            if (currentSmallPlotToWork.IsFree())
            {
                Player.LocalPlayer.playerMovement.SetMoving(false);
                UIController.Instance.landInteraction.Hoe();
                // StartCoroutine(HoeingPlant(currentSmallPlotToWork));
                // HoeingPlant();
            }
            else ResetActionAfterWorkInSmallPlot();
        }
        if (currentActionType == ActionType.Ranking)
        {
            if (currentSmallPlotToWork.IsFree())
            {

                // StartCoroutine(PoachingCourountine());
                UIController.Instance.landInteraction.Poach();
                // StartCoroutine(RankingPlant(currentSmallPlotToWork));
            }
            else ResetActionAfterWorkInSmallPlot();

        }
        if (currentActionType == ActionType.Watering)
        {
            if (!currentSmallPlotToWork.IsFree() && !currentSmallPlotToWork.CropIsRipe())
            {
                Player.LocalPlayer.playerMovement.SetMoving(false);
                UIController.Instance.landInteraction.Water();
                //  S//tartCoroutine(WateringPlant(currentSmallPlotToWork));
            }
            else ResetActionAfterWorkInSmallPlot();
        }

        //}
        if (currentActionType == ActionType.Fertilizer)
        {
            bool hasUnripeCrop = currentSmallPlotToWork.GetCurrentCrop() != null && !currentSmallPlotToWork.CropIsRipe();
            bool canPreFertilize = currentSmallPlotToWork.GetCurrentCrop() == null
                                   && currentSmallPlotToWork.IsHoeing()
                                   && currentSmallPlotToWork.IsRanking()
                                   && currentSmallPlotToWork.IsProcessIsRight()
                                   && !currentSmallPlotToWork.IsUseFertilizerBeforePlant();

            if (hasUnripeCrop || canPreFertilize)
            {
                Player.LocalPlayer.playerMovement.SetMoving(false);
                UIController.Instance.landInteraction.Fertilize();

                BagItemManager.Instance.DecreaseItemAmount(
                 EBagItemCategory.fertilizer,
                    1
                );
            }
            else ResetActionAfterWorkInSmallPlot();


        }
        if (currentActionType == ActionType.Pesticedes)
        {
            if (!currentSmallPlotToWork.IsFree() && !currentSmallPlotToWork.CropIsRipe())
            {
                Player.LocalPlayer.playerMovement.SetMoving(false);
                UIController.Instance.landInteraction.Spray();

                BagItemManager.Instance.DecreaseItemAmount(
                 EBagItemCategory.pesticide,
                 1
                );


                //    StartCoroutine(UsePesticide(currentSmallPlotToWork));
            }
            else ResetActionAfterWorkInSmallPlot();




        }

        if (currentActionType == ActionType.Haverst)
        {

            if (!currentSmallPlotToWork.IsFree())//&& currentSmallPlotToWork.CropIsRipe()
            {
                Player.LocalPlayer.playerMovement.SetMoving(false);
                UIController.Instance.landInteraction.Harvest();
                // StartCoroutine(HaverstPlant(currentSmallPlotToWork));
            }
            else ResetActionAfterWorkInSmallPlot();

        }

    }
    private IEnumerator PoachingCourountine()
    {
        Player.LocalPlayer.playerMovement.SetMoving(false);
        yield return new WaitForSeconds(0.1f);
        UIController.Instance.landInteraction.Poach();//

    }
    public void ResetAllAndEnablePlayerMovement()
    {
        // ... (Giữ nguyên đoạn code setup logic phía trên của bạn)
        foreach (Transform _smallPlotTransform in smallPlotList)
        {
            SmallPlot _smallPlot = _smallPlotTransform.GetComponent<SmallPlot>();
            smallPlotHasWorkDic[_smallPlot] = false;
        }

        currentSmallPlot = null;
        currentActionType = ActionType.None;
        distancePlayerMustDoAction = Vector3.zero;
        inAction = false;
        isImplementActionInBigLandPlot = false;

        UIController.Instance.SetCurrentSelectedLandPlot(null);
        UIController.Instance.SetCurrentSelectedSmallPlot(null);

        Player.LocalPlayer.playerMovement.SetMoving(false);
        Player.LocalPlayer.playerMovement.SetBusyDoingAction(false);
        Player.LocalPlayer.moveSpeed = 3;
        Player.LocalPlayer.playerAnimation.ClearAllTrigger();

        // ================= SỬA ĐOẠN NÀY =================
        // Thêm check null cho chắc chắn và gọi hàm có đồng bộ RPC mạng
        if (UIController.Instance.landInteraction != null)
        {
            UIController.Instance.landInteraction.SetActiveToolAndSync(string.Empty);
        }
        // ================================================
    }
    private bool IsPlayerInRightSideOfPlot()
    {
        float leftBoundary = transform.position.x - GetPlotSize() / 2;
        float rightBoundary = transform.position.x + GetPlotSize() / 2;

        float playerPositionX = Player.LocalPlayer.playerMovement.transform.position.x;
        if (Mathf.Abs(playerPositionX - leftBoundary) > Mathf.Abs(playerPositionX - rightBoundary))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool isFullCropInPlotLand()
    {
        foreach (Transform smallPlotTransform in smallPlotList)
        {
            SmallPlot smallPlot = smallPlotTransform.GetComponent<SmallPlot>();
            if (smallPlot != null && smallPlot.GetCurrentCrop() == null)
            {
                return false;
            }
        }
        return true;
    }

    public void DoAction(ActionType actionType)
    {
        if (isImplementActionInBigLandPlot && currentActionType == actionType)
        {
            return;
        }

        if (actionType == ActionType.Ranking || actionType == ActionType.Hoeing)
        {
            if (isFullCropInPlotLand()) return;
        }


        currentActionType = actionType;
        Player.LocalPlayer.playerMovement.SetBusyDoingAction(true);
        isImplementActionInBigLandPlot = true;


    }
    public void DoPlant(BagItemConfig bagItem)
    {
        // check if full crop ==>
        if (isFullCropInPlotLand()) return;
        isImplementActionInBigLandPlot = true;
        currentItemDataCrop = bagItem;
        currentActionType = ActionType.PlantCrop;
        Player.LocalPlayer.playerMovement.SetBusyDoingAction(true);
        Player.LocalPlayer.playerMovement.SetMoving(true);
        Debug.Log("do plant");
    }
    #region Interact With Plot of LandPLot

    public void PlantCrop(BagItemConfig _cropData, SmallPlot _currentSmallPlot)
    {

        Debug.Log("Plant");
        _currentSmallPlot.PlantCrop(_cropData);

    }

    public void HoeingPlant(SmallPlot _currentSmallPlot)
    {
        //   ResetActionAfterWorkInSmallPlot();
        _currentSmallPlot.HoeingPlant();
        //  Player.LocalPlayer.moveSpeed = 0;  

        // if (isCancleAction) ResetAllAndEnablePlayerMovement();
    }

    public void UsePesticide(SmallPlot _currentSmallPlot)
    {

        Debug.Log("Per");
        _currentSmallPlot.UsePesticdes();

    }



    public void UseFertilizer(SmallPlot _currentSmallPlot)
    {
        if (_currentSmallPlot == null) return;
        Debug.Log("Fer");
        _currentSmallPlot.FertilizingCrop();

    }

    public void HaverstPlant(SmallPlot _currentSmallPlot)
    {

        Debug.Log("Harvest");
        _currentSmallPlot.HavertCrop();

    }

    public void WateringPlant(SmallPlot _currentSmallPlot)
    {

        Debug.Log("Water");
        _currentSmallPlot.WateringCrop();

    }

    public void RankingPlant(SmallPlot _currentSmallPlot)
    {

        _currentSmallPlot.RakingPlant();

    }

    #endregion

    public void ResetActionAfterWorkInSmallPlot()
    {
        inAction = false;
    }

    public float GetPlotSize()
    {
        return GetComponentInChildren<SmallPlot>().GetComponent<Collider>().bounds.size.x * smallPlotList.Count;
    }
    public SmallPlot GetCurrentSmallPlot() => currentSmallPlot;
    public ActionType GetCurrentActionType() => currentActionType;
    public void SetCurrentActionType(ActionType type) => currentActionType = type;
    public BagItemConfig GetCurrentItemDataCrop() => currentItemDataCrop;
    // public bool IsCancelAction() => isCancleAction;

}

