using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Crop : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private List<Transform> stagesList;
    // [SerializeField] private Transform ripeStageLackOfWater;
    [SerializeField] private List<int> timeBetweenStateList;
    [SerializeField] private float HoursToRipe;
    [SerializeField] private ItemData PlantData;

    [SerializeField] public BagItemConfig _itemConfig;
    [Header("Time Reduce When Fertilizer Before Plant")]
    [SerializeField] private int _timeReduceFertilizingBeforePlant;

    private SmallPlot currentSmallPlot;

    private float elapsedTime;
    private int currentStage = 0;
    private bool isRipe;


    private bool isLackWater;
    private bool isFallingLackWaterState;

    private bool isHasWorm;
    private bool isFallingWorn;
    private bool isLackOfFertilizer;
    private bool isUseFertilizerBeforePlant;

    private bool isDead; //  

    private bool isProcessTrue = true;

    void Start()
    {
        elapsedTime = 0f;
        isRipe = false;
    }

    void Update()
    {
        CheckPlantGrowth();
    }

    public void SetConfig(BagItemConfig itemConfig)
    {
        _itemConfig = itemConfig;
        Debug.Log("crop config" + _itemConfig.seedType);
    }

    public void SetProcessPlantCropIsTrue(bool _isProcessTrue)
    {
        isProcessTrue = _isProcessTrue;
    }
    public void SetUseFertilizerBeforePlant(bool _isUseFertilizer)
    {
        isUseFertilizerBeforePlant = _isUseFertilizer;
        if (isUseFertilizerBeforePlant) ReduceGrowthTime();
    }
    private void ReduceGrowthTime() // giam thoi gian : chi ap dung voi bon phan truoc khi trong cay
    {
        HoursToRipe -= _timeReduceFertilizingBeforePlant;

        for (int i = 1; i < timeBetweenStateList.Count; i++)
        {
            timeBetweenStateList[i] -= _timeReduceFertilizingBeforePlant;
            Debug.Log(timeBetweenStateList[i]);
        }
    }
    public void SetCurrentSmallPlot(SmallPlot _currentPlot)
    {
        currentSmallPlot = _currentPlot;
    }
    public SmallPlot GetCurrentSmallPlot() => currentSmallPlot;
    private void CheckPlantGrowth()
    {
        if (isRipe) return;

        if (isProcessTrue)
        {
            elapsedTime += Time.deltaTime;
            if (currentStage < stagesList.Count - 1 && elapsedTime > timeBetweenStateList[currentStage + 1])
            {
                stagesList[currentStage].gameObject.SetActive(false);
                currentStage++;
                int newState = currentStage;
                // check if crop is ripe or not
                if (newState == stagesList.Count - 1)
                {
                    isRipe = true;
                    SetStateWhenRipe(newState);
                    //ReloadCircleMenuIfDisplayed();// reload tool
                }
                else
                {
                    stagesList[newState].gameObject.SetActive(true);

                    // check if this state can have problem ==> spawm it 
                    bool canHappenProblemInThisState = stagesList[newState].GetComponent<CropStateHappenProblem>() ? true : false;
                    if (canHappenProblemInThisState)
                    {
                        Transform currenCropState = stagesList[newState];
                        //stageCanHaveProblemList.Remove(newState);
                        if (IsLackWater())
                        {
                            SetLackWaterCropState(currenCropState);
                        }
                        if (IsLackOfFertilizer())
                        {
                            SetLackOfFertilizerCropState(currenCropState);
                        }
                        if (IsHasWorn())
                        {
                            SetHasWornCropState(currenCropState);
                        }
                        StartCoroutine(SpawmProblem(newState, 2f));
                    }

                }
            }
        }
        else
        {
            isRipe = true;
        }
    }

    //private void ReloadCircleMenuIfDisplayed()
    //{
    //    if (!UIController.Instance.CircleMenuIsActive()) return; 
    //    // reload tool
    //    if (UIController.Instance.GetCurrentSelectedSmallPlot() == GetCurrentSmallPlot())
    //    {

    //        UIController.Instance.ToggleCircleUI(true);
    //    }
    //}

    private IEnumerator SpawmProblem(int _currentState, float _time)
    {
        yield return new WaitForSeconds(_time);
        Transform currenCropState = stagesList[_currentState];
        if (!isLackWater)
        {
            isLackWater = true;
            SetLackWaterCropState(currenCropState);

        }
        if (!isLackOfFertilizer)
        {
            isLackOfFertilizer = true;
            SetLackOfFertilizerCropState(currenCropState);
        }
        if (!isHasWorm && Random.value > 0.5f)
        {
            isHasWorm = true;
            SetHasWornCropState(currenCropState);
        }
        // ReloadCircleMenuIfDisplayed();
    }
    private void SetStateWhenRipe(int _cropStage)
    {
        Transform currentRipeCrop = stagesList[_cropStage];

        bool canHappenProblemInThisState = stagesList[_cropStage].GetComponent<CropStateHappenProblem>() ? true : false;
        if (canHappenProblemInThisState)
        {
            if (isLackWater)
            {
                SetLackWaterCropState(currentRipeCrop);
            }
            if (isLackOfFertilizer)
            {
                SetLackOfFertilizerCropState(currentRipeCrop);
            }
        }
        //  GetCurrentSmallPlot().GetComponentInParent<FieldPlots>().RemoveWayPointIMGToFIeld(WarningType.Fertilizer);
        // priority Withering stage =>> to set small model if it lack of water

        currentRipeCrop.gameObject.SetActive(true);
        UIController.Instance.AddWarningByType(GetCurrentSmallPlot(), WarningType.Ripe);
        UIController.Instance.RemoveWarningByType(GetCurrentSmallPlot(), WarningType.HasWorm);
        UIController.Instance.RemoveWarningByType(GetCurrentSmallPlot(), WarningType.Fertilizer);
        UIController.Instance.RemoveWarningByType(GetCurrentSmallPlot(), WarningType.Water);
    }
    #region treat Crop


    public void UseFertilizerBeforePlantCrop()
    {
        if (isRipe) return;
        if (isDead) return;
        if (isUseFertilizerBeforePlant) return;

        Debug.Log("giam thoi gian phat trien ");
    }
    public void UseFertilizer()
    {
        if (isRipe) return;
        if (isDead) return;
        UIController.Instance.RemoveWarningByType(GetCurrentSmallPlot(), WarningType.Fertilizer);
        float recoveryTime = 0.02f * (timeBetweenStateList[currentStage + 1] - timeBetweenStateList[currentStage]);
        StartCoroutine(UseFertilizerCoroutine(recoveryTime));
    }
    private IEnumerator UseFertilizerCoroutine(float _timeToBackNormalCropState)
    {
        isLackOfFertilizer = false;
        yield return new WaitForSeconds(_timeToBackNormalCropState);

        SetNormalScaleCropState(stagesList[currentStage]);
    }
    public void Watering()
    {
        if (isRipe) return;
        if (isDead) return;
        UIController.Instance.RemoveWarningByType(GetCurrentSmallPlot(), WarningType.Water);
        isFallingLackWaterState = false;
        float recoveryTime = 0.02f * (timeBetweenStateList[currentStage + 1] - timeBetweenStateList[currentStage]);
        StartCoroutine(WateringCoroutine(recoveryTime));

    }
    private IEnumerator WateringCoroutine(float _timeToBackNormalCropState)
    {
        isLackWater = false;
        yield return new WaitForSeconds(_timeToBackNormalCropState);
        SetNormalCropState(stagesList[currentStage]);
    }
    public void UsePesticide()
    {
        if (isRipe) return;
        if (isDead) return;
        UIController.Instance.RemoveWarningByType(GetCurrentSmallPlot(), WarningType.HasWorm);
        isFallingWorn = false;
        Debug.Log("Use pesticide");
        isHasWorm = false;
    }
    #endregion
    #region set crop Problem
    private void SetHasWornCropState(Transform _cropState)
    {
        UIController.Instance.AddWarningByType(GetCurrentSmallPlot(), WarningType.HasWorm);
        if (!isFallingWorn)
        {
            isFallingWorn = true;
            StartCoroutine(SetHasWormLongTimeCropState());
        }
    }
    private IEnumerator SetHasWormLongTimeCropState()
    {
        float amountTimeToSetWithering = 0.7f * ((timeBetweenStateList[timeBetweenStateList.Count - 1] - timeBetweenStateList[currentStage]));
        //  Debug.Log(timeBetweenStateList[timeBetweenStateList.Count - 1] + " " + timeBetweenStateList[currentStage]);
        yield return new WaitForSeconds(amountTimeToSetWithering);

        if (isFallingWorn)
        {
            Transform currenCropState = stagesList[currentStage];
            CropStateHappenProblem currentCropHappenState = currenCropState.GetComponent<CropStateHappenProblem>();
            if (currentCropHappenState != null)
            {
                currentCropHappenState.SetWitheringStateMaterials();
                SetDeadState();
                //ReloadCircleMenuIfDisplayed();
            }
        }


    }

    // lack of water
    private void SetLackWaterCropState(Transform _cropState)
    {
        UIController.Instance.AddWarningByType(GetCurrentSmallPlot(), WarningType.Water);
        CropStateHappenProblem currentCropState = _cropState.GetComponent<CropStateHappenProblem>();
        currentCropState.SetLackOfWaterStateMaterials();
        if (!isFallingLackWaterState)
        {
            isFallingLackWaterState = true;
            StartCoroutine(SetWitheringCropState());
        }
    }
    private IEnumerator SetWitheringCropState()
    {
        float amountTimeToSetWithering = 0.7f * ((timeBetweenStateList[timeBetweenStateList.Count - 1] - timeBetweenStateList[currentStage]));
        Debug.Log(timeBetweenStateList[timeBetweenStateList.Count - 1] + " " + timeBetweenStateList[currentStage]);
        yield return new WaitForSeconds(amountTimeToSetWithering);

        if (isFallingLackWaterState)
        {
            Transform currenCropState = stagesList[currentStage];
            CropStateHappenProblem currentCropHappenState = currenCropState.GetComponent<CropStateHappenProblem>();
            if (currentCropHappenState != null)
            {
                currentCropHappenState.SetWitheringStateMaterials();
                SetDeadState();
                //ReloadCircleMenuIfDisplayed();
            }
        }


    }
    // lack of fertilizer
    private void SetLackOfFertilizerCropState(Transform _cropState)
    {
        UIController.Instance.AddWarningByType(GetCurrentSmallPlot(), WarningType.Fertilizer);
        CropStateHappenProblem currentCropState = _cropState.GetComponent<CropStateHappenProblem>();
        currentCropState.SetLackFertilizerScaleState();
    }

    private void SetNormalCropState(Transform _cropState)
    {
        CropStateHappenProblem currentCropState = _cropState.GetComponent<CropStateHappenProblem>();
        if (currentCropState != null)
            currentCropState.SetNormalStateMaterials();
    }
    private void SetNormalScaleCropState(Transform _cropState)
    {
        CropStateHappenProblem currentCropState = _cropState.GetComponent<CropStateHappenProblem>();
        if (currentCropState != null)
            currentCropState.SetNormalScaleState();
    }
    #endregion
    public void SetDeadState()
    {
        isDead = true;
        isRipe = true;
        UIController.Instance.RemoveWarningByType(GetCurrentSmallPlot(), WarningType.HasWorm);
        UIController.Instance.RemoveWarningByType(GetCurrentSmallPlot(), WarningType.Fertilizer);
        UIController.Instance.RemoveWarningByType(GetCurrentSmallPlot(), WarningType.Water);
    }

    public bool IsLackWater() => isLackWater;
    public bool IsLackOfFertilizer() => isLackOfFertilizer;
    public bool IsHasWorn() => isHasWorm;
    public float GetHoursToRipe() => HoursToRipe;
    public float GetCurrentGrowthTimeElapsed() => elapsedTime;
    public bool IsRipe() => isRipe;
    public ItemData GetPlantData() => PlantData;

    public void OnPointerDown(PointerEventData eventData)
    {
        Player.LocalPlayer.playerMovement.SetMoveToTarget(GetCurrentSmallPlot().transform);
    }
    public bool IsDead() => isDead;
}

