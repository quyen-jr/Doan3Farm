using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SmallPlot : MonoBehaviour
{

    [Header("State Material")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material dryLandMaterial;
    [SerializeField] private Material hasHoeMaterial;
    [SerializeField] private Material rakingMaterial;
    [SerializeField] private MeshRenderer meshRender;

    private bool _isUseFertilizerBeforePlant;
    private Collider _collider;
    private bool isFree;// if has any crop on plot
                        // private Crop currentCrop;
                        // private Crop statusCrops; // 1  cây đại diện cho  tất cả các cây trên mảnh ( vì các cây trên mảnh đồng bộ )
    private bool _istimeToSpawnProblem;
    private List<Crop> _currentCropList = new List<Crop>();

    private bool isHoeing;
    private bool isRaking;
    private int _maxCropSetUp;


    [SerializeField] private bool isProcessIsTrue = true;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        
        FieldPlots fieldPlots= GetComponentInParent<FieldPlots>();
        _maxCropSetUp = (fieldPlots!=null)? fieldPlots.GetMaxCropInSmallPlot():3;
        meshRender.material = normalMaterial;
        isFree = true;
    }
    private void Update()
    {

    }
    public void PlantCrop(BagItemConfig _cropData)
    {
        if (!IsFree()) return;

        if (!isHoeing && !isRaking || isHoeing && !isRaking || isRaking && !isHoeing) isProcessIsTrue = false;
        if (!isProcessIsTrue)
        {
            UIController.Instance.AddWarningByType(this, WarningType.WrongProcess);
        }

        ProcessPlantManyTree(_cropData);

        isFree = false;
    }

    private void ProcessPlantManyTree(BagItemConfig _cropData)
    {
        float totalWidth = GetSmallPlotSize();

        int totalCrops = _maxCropSetUp;
        float spaceBetweenCrop = totalWidth / (totalCrops + 1); // Cộng 1 để tạo khoảng cách hai bên

        for (int i = 0; i < totalCrops; i++)
        {
            GameObject newCropObject = Instantiate(_cropData.itemData.prefabs, transform);

            Crop crop = newCropObject.GetComponent<Crop>();
            crop.SetConfig(_cropData);

            // Cây đầu tiên sẽ bắt đầu từ trái, cây cuối cùng ở phải, và các cây giữa sẽ nằm đều
            float offsetX = spaceBetweenCrop * (i + 1) - totalWidth / 2;

            newCropObject.transform.localPosition = new Vector3(
                offsetX, newCropObject.transform.localPosition.y, newCropObject.transform.localPosition.z);

            // Gán các thuộc tính cho cây
            Crop currentCropPlanted = newCropObject.GetComponent<Crop>();
            currentCropPlanted.SetProcessPlantCropIsTrue(isProcessIsTrue);
            currentCropPlanted.SetUseFertilizerBeforePlant(_isUseFertilizerBeforePlant);
            currentCropPlanted.SetCurrentSmallPlot(this);
            _currentCropList.Add(currentCropPlanted);
        }
    }


    public void HoeingPlant()
    {
        if (!IsFree()) return;
        if (isRaking) isProcessIsTrue = false;
        isHoeing = true;
        setPlotMaterial(hasHoeMaterial);

    }
    public void RakingPlant()
    {
        //Debug.Log(isFree);
        //Debug.Log(isHoeing);
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
        // when raking done  display seedUI to plant
    }
    public void HavertCrop()
    {
        if (IsFree()) return;
        if (_currentCropList == null || _currentCropList.Count <= 0) return;

        int aliveCropCount = 0;
        BagItemConfig cropConfig = null;

        for (int i = 0; i < _currentCropList.Count; i++)
        {
            Crop crop = _currentCropList[i];

            if (crop == null) continue;

            // Lưu config của cây để lát cộng item
            if (cropConfig == null)
            {
                cropConfig = crop._itemConfig;
            }

            // Cây chưa chết thì được tính là thu hoạch được
            if (!crop.IsDead())
            {
                aliveCropCount++;
            }
        }

        // Nếu quy trình trồng đúng và còn cây sống thì mới cộng item
        if (isProcessIsTrue && aliveCropCount > 0 && cropConfig != null)
        {
            BagItemManager.Instance.AddItem(cropConfig, aliveCropCount);
        }

        // Xóa warning UI
        UIController.Instance.RemoveWarningByType(this, WarningType.WrongProcess);
        UIController.Instance.RemoveWarningByType(this, WarningType.Ripe);
        UIController.Instance.RemoveWarningByType(this, WarningType.HasWorm);
        UIController.Instance.RemoveWarningByType(this, WarningType.Fertilizer);
        UIController.Instance.RemoveWarningByType(this, WarningType.Water);

        // Reset đất
        setPlotMaterial(dryLandMaterial);
        RemoveAllCrops();
        ResetPlotVariable();
    }
    private void RemoveAllCrops()
    {
        for (int i = 0; i < _currentCropList.Count; i++)
        {
            Destroy(_currentCropList[i].gameObject);
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
        if (IsFree()) return;
        //        currentCrop.Watering();
        for (int i = 0; i < _currentCropList.Count; i++)
        {
            _currentCropList[i].Watering();
        }
    }
    public void FertilizingCrop()
    {
        if (_currentCropList.Count > 0)
        {
            for (int i = 0; i < _currentCropList.Count; i++)
            {
                _currentCropList[i].UseFertilizer();
            }
        }
        else if (isHoeing && isRaking && isProcessIsTrue) _isUseFertilizerBeforePlant = true;
        Debug.Log(_isUseFertilizerBeforePlant);
    }
    public void UsePesticdes()
    {
        if (IsFree()) return;
        for (int i = 0; i < _currentCropList.Count; i++)
        {
            _currentCropList[i].UsePesticide();
        }
    }
    public void setPlotMaterial(Material _newMaterial)
    {
        meshRender.material = _newMaterial;
    }

    public bool IsProcessIsRight() => isProcessIsTrue;
    public bool CropIsRipe() => GetCurrentCrop().IsRipe();
    public Crop GetCurrentCrop()
    {
        if (_currentCropList.Count <= 0) return null;
        return _currentCropList[_currentCropList.Count - 1];
    }
    public bool IsFree() { return _currentCropList.Count == 0; }
    public bool IsHoeing() { return isHoeing == true; }
    public bool IsRanking() { return isRaking == true; }
    public void CanHaverst()
    {
        
    }
    
    private float GetSmallPlotSize()
    {
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>(); // Lấy MeshRenderer từ GameObject

        if (meshRenderer != null)
        {
            // Lấy kích thước chiều rộng từ bounds
            float width = meshRenderer.bounds.size.x;
            return math.round(width); // Làm tròn giá trị chiều rộng
        }

        return 0; // Trả về 0 nếu không tìm thấy MeshRenderer
    }
}
