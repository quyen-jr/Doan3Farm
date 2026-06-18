using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class SmallPlot : MonoBehaviourPun
{
    [Header("State Material")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material dryLandMaterial;
    [SerializeField] private Material hasHoeMaterial;
    [SerializeField] private Material rakingMaterial;
    [SerializeField] private MeshRenderer meshRender;

    private bool _isUseFertilizerBeforePlant;
    private Collider _collider;
    private bool isFree;
    private bool _istimeToSpawnProblem;
    private readonly List<Crop> _currentCropList = new List<Crop>();

    private bool isHoeing;
    private bool isRaking;
    private int _maxCropSetUp;

    [SerializeField] private bool isProcessIsTrue = true;

    private void Start()
    {
        _collider = GetComponent<Collider>();

        FieldPlots fieldPlots = GetComponentInParent<FieldPlots>();
        _maxCropSetUp = (fieldPlots != null) ? fieldPlots.GetMaxCropInSmallPlot() : 3;
        meshRender.material = normalMaterial;
        isFree = true;
    }

    public void PlantCrop(BagItemConfig cropData)
    {
        if (cropData == null || cropData.itemData == null || cropData.itemData.prefabs == null) return;

        string prefabName = cropData.itemData.prefabs.name;
        string itemDataName = cropData.itemData.name;
        int category = (int)cropData.category;
        int seedType = (int)cropData.seedType;
        if (photonView != null)
        {
            photonView.RPC(nameof(RpcPlantCrop), RpcTarget.AllBuffered, prefabName, itemDataName, category, seedType);
            return;
        }

        PlantCropInternal(cropData);
    }

    private void PlantCropInternal(BagItemConfig cropData)
    {
        if (!IsFree()) return;

        if (!isHoeing && !isRaking || isHoeing && !isRaking || isRaking && !isHoeing) isProcessIsTrue = false;
        if (!isProcessIsTrue)
        {
            UIController.Instance.AddWarningByType(this, WarningType.WrongProcess);
        }

        ProcessPlantManyTree(cropData);
        isFree = false;
    }

    private void ProcessPlantManyTree(BagItemConfig cropData)
    {
        float totalWidth = GetSmallPlotSize();

        int totalCrops = _maxCropSetUp;
        float spaceBetweenCrop = totalWidth / (totalCrops + 1);

        for (int i = 0; i < totalCrops; i++)
        {
            GameObject newCropObject = Instantiate(cropData.itemData.prefabs, transform);

            Crop crop = newCropObject.GetComponent<Crop>();
            crop.SetConfig(cropData);

            float offsetX = spaceBetweenCrop * (i + 1) - totalWidth / 2;

            newCropObject.transform.localPosition = new Vector3(
                offsetX, newCropObject.transform.localPosition.y, newCropObject.transform.localPosition.z);

            Crop currentCropPlanted = newCropObject.GetComponent<Crop>();
            currentCropPlanted.SetProcessPlantCropIsTrue(isProcessIsTrue);
            currentCropPlanted.SetUseFertilizerBeforePlant(_isUseFertilizerBeforePlant);
            currentCropPlanted.SetCurrentSmallPlot(this);
            _currentCropList.Add(currentCropPlanted);
        }
    }

    public void HoeingPlant()
    {
        if (photonView != null)
        {
            photonView.RPC(nameof(RpcHoeingPlant), RpcTarget.AllBuffered);
            return;
        }

        HoeingPlantInternal();
    }

    private void HoeingPlantInternal()
    {
        if (!IsFree()) return;
        if (isRaking) isProcessIsTrue = false;
        isHoeing = true;
        setPlotMaterial(hasHoeMaterial);
    }

    public void RakingPlant()
    {
        if (photonView != null)
        {
            photonView.RPC(nameof(RpcRakingPlant), RpcTarget.AllBuffered);
            return;
        }

        RakingPlantInternal();
    }

    private void RakingPlantInternal()
    {
        if (!IsFree()) return;

        if (!isHoeing)
        {
            isProcessIsTrue = false;
            isRaking = true;
        }
        else
        {
            isProcessIsTrue = true;
            isRaking = true;
        }

        setPlotMaterial(rakingMaterial);
    }

    public void HavertCrop()
    {
        int actorNumber = PhotonNetwork.InRoom ? PhotonNetwork.LocalPlayer.ActorNumber : -1;

        if (photonView != null)
        {
            photonView.RPC(nameof(RpcHavertCrop), RpcTarget.AllBuffered, actorNumber);
            return;
        }

        HavertCropInternal(actorNumber);
    }

    private void HavertCropInternal(int actorNumber)
    {
        if (IsFree()) return;
        if (_currentCropList == null || _currentCropList.Count <= 0) return;

        int ripeCropCount = 0;
        BagItemConfig cropConfig = null;

        for (int i = 0; i < _currentCropList.Count; i++)
        {
            Crop crop = _currentCropList[i];
            if (crop == null) continue;

            if (cropConfig == null)
            {
                cropConfig = crop._itemConfig;
            }

            if (!crop.IsDead() && crop.IsRipe())
            {
                ripeCropCount++;
            }
        }

        bool canRewardLocal = actorNumber == -1 || !PhotonNetwork.InRoom || PhotonNetwork.LocalPlayer.ActorNumber == actorNumber;
        if (isProcessIsTrue && ripeCropCount > 0 && cropConfig != null && canRewardLocal)
        {
            BagItemManager.Instance.AddItem(cropConfig, ripeCropCount);
        }

        UIController.Instance.RemoveWarningByType(this, WarningType.WrongProcess);
        UIController.Instance.RemoveWarningByType(this, WarningType.Ripe);
        UIController.Instance.RemoveWarningByType(this, WarningType.HasWorm);
        UIController.Instance.RemoveWarningByType(this, WarningType.Fertilizer);
        UIController.Instance.RemoveWarningByType(this, WarningType.Water);

        setPlotMaterial(dryLandMaterial);
        RemoveAllCrops();
        ResetPlotVariable();
    }

    private void RemoveAllCrops()
    {
        for (int i = 0; i < _currentCropList.Count; i++)
        {
            if (_currentCropList[i] != null)
            {
                Destroy(_currentCropList[i].gameObject);
            }
        }
    }

    private void ResetPlotVariable()
    {
        _currentCropList.Clear();
        isFree = true;
        isHoeing = false;
        isRaking = false;
        isProcessIsTrue = true;
        _isUseFertilizerBeforePlant = false;
    }

    public void WateringCrop()
    {
        if (photonView != null)
        {
            photonView.RPC(nameof(RpcWateringCrop), RpcTarget.All);
            return;
        }

        WateringCropInternal();
    }

    private void WateringCropInternal()
    {
        if (IsFree()) return;
        for (int i = 0; i < _currentCropList.Count; i++)
        {
            _currentCropList[i].Watering();
        }
    }

    public void FertilizingCrop()
    {
        if (photonView != null)
        {
            photonView.RPC(nameof(RpcFertilizingCrop), RpcTarget.All);
            return;
        }

        FertilizingCropInternal();
    }

    private void FertilizingCropInternal()
    {
        if (_currentCropList.Count > 0)
        {
            for (int i = 0; i < _currentCropList.Count; i++)
            {
                _currentCropList[i].UseFertilizer();
            }
        }
        else if (isHoeing && isRaking && isProcessIsTrue)
        {
            if (_isUseFertilizerBeforePlant) return;
            _isUseFertilizerBeforePlant = true;
        }
    }

    public void UsePesticdes()
    {
        if (photonView != null)
        {
            photonView.RPC(nameof(RpcUsePesticdes), RpcTarget.All);
            return;
        }

        UsePesticdesInternal();
    }

    private void UsePesticdesInternal()
    {
        if (IsFree()) return;
        for (int i = 0; i < _currentCropList.Count; i++)
        {
            _currentCropList[i].UsePesticide();
        }
    }

    [PunRPC]
    private void RpcPlantCrop(string prefabName, string itemDataName, int category, int seedType)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        if (prefab == null)
        {
            Debug.LogWarning("Khong tim thay crop prefab trong Resources: " + prefabName);
            return;
        }

        BagItemConfig runtimeConfig = new BagItemConfig();
        runtimeConfig.category = (EBagItemCategory)category;
        runtimeConfig.seedType = (ESeedsCircleOptionType)seedType;

        ItemData itemDataAsset = Resources.Load<ItemData>(itemDataName);
        if (itemDataAsset != null)
        {
            runtimeConfig.itemData = itemDataAsset;
        }
        else
        {
            Crop cropOnPrefab = prefab.GetComponent<Crop>();
            if (cropOnPrefab != null && cropOnPrefab.GetPlantData() != null)
            {
                runtimeConfig.itemData = cropOnPrefab.GetPlantData();
            }
            else
            {
                runtimeConfig.itemData = ScriptableObject.CreateInstance<ItemData>();
                runtimeConfig.itemData.prefabs = prefab;
            }
        }

        if (runtimeConfig.itemData.prefabs == null)
        {
            runtimeConfig.itemData.prefabs = prefab;
        }

        PlantCropInternal(runtimeConfig);
    }

    [PunRPC]
    private void RpcHoeingPlant()
    {
        HoeingPlantInternal();
    }

    [PunRPC]
    private void RpcRakingPlant()
    {
        RakingPlantInternal();
    }

    [PunRPC]
    private void RpcHavertCrop(int actorNumber)
    {
        HavertCropInternal(actorNumber);
    }

    [PunRPC]
    private void RpcWateringCrop()
    {
        WateringCropInternal();
    }

    [PunRPC]
    private void RpcFertilizingCrop()
    {
        FertilizingCropInternal();
    }

    [PunRPC]
    private void RpcUsePesticdes()
    {
        UsePesticdesInternal();
    }

    public void setPlotMaterial(Material newMaterial)
    {
        meshRender.material = newMaterial;
    }

    public bool IsProcessIsRight() => isProcessIsTrue;
    public bool CropIsRipe() => GetCurrentCrop().IsRipe();

    public Crop GetCurrentCrop()
    {
        if (_currentCropList.Count <= 0) return null;
        return _currentCropList[_currentCropList.Count - 1];
    }

    public bool IsFree() { return _currentCropList.Count == 0; }
    public bool IsHoeing() { return isHoeing; }
    public bool IsRanking() { return isRaking; }
    public bool IsUseFertilizerBeforePlant() { return _isUseFertilizerBeforePlant; }
    public void CanHaverst() { }

    private float GetSmallPlotSize()
    {
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();

        if (meshRenderer != null)
        {
            float width = meshRenderer.bounds.size.x;
            return math.round(width);
        }

        return 0;
    }
}
