using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WarningWayPoint : MonoBehaviour
{
    Dictionary<SmallPlot, List<WayPointImage>> wayPointDictionary = new Dictionary<SmallPlot, List<WayPointImage>>();
    [SerializeField] private GameObject wayPointImageUIPrefab;
    [SerializeField] private Sprite warningWaterSprite;
    [SerializeField] private Sprite warningFertilizerSprite;
    [SerializeField] private Sprite warningHasWormSprite;
    [SerializeField] private Sprite warningRipeSprite;
    // Indicator icon
    //public Image img;
    //// The target (location, enemy, etc..)
    //public Transform target;
    //// UI Text to display the distance
    //public Text meter;
    //// To adjust the position of the icon
    //public Vector3 offset;




    public void AddWayPointByType(SmallPlot _smallPlot, WarningType _wayPointType)
    {
        // Nếu _smallPlot chưa có trong dictionary, tạo danh sách mới
        if (_smallPlot == null) return;
        if (!wayPointDictionary.ContainsKey(_smallPlot))
        {
            wayPointDictionary[_smallPlot] = new List<WayPointImage>();
        }

        // Kiểm tra nếu đã tồn tại cảnh báo có cùng loại WarningType trong danh sách
        bool isWayPointTypeExist = wayPointDictionary[_smallPlot]
            .Any(existingWarning => existingWarning.GetWayPointType() == _wayPointType);

        if (!isWayPointTypeExist)
        {
            // Tạo đối tượng cảnh báo mới và gán các thông tin cần thiết
            GameObject warningImageObjectUI = Instantiate(wayPointImageUIPrefab, transform);
            WayPointImage wayPointCropImg = warningImageObjectUI.GetComponent<WayPointImage>();
            //  wayPointCropImg.set(_smallPlot.transform);

            // Chọn sprite tương ứng với loại cảnh báo
            Sprite spriteToSet = null;
            switch (_wayPointType)
            {
                case WarningType.Water:
                    spriteToSet = warningWaterSprite;
                    break;

                case WarningType.HasWorm:
                    spriteToSet = warningHasWormSprite;
                    break;
                case WarningType.Fertilizer:
                    spriteToSet = warningFertilizerSprite;
                    break;
                case WarningType.Ripe:
                    spriteToSet = warningRipeSprite;
                    break;

                default:
                    Debug.LogWarning("Unknown Warning Type");
                    break;
            }

            // Đặt sprite và loại cảnh báo cho đối tượng
            wayPointCropImg.SetSprite(spriteToSet);
            wayPointCropImg.SetWayPointType(_wayPointType);
            wayPointCropImg.setTargetOBJ(_smallPlot.transform);

            _smallPlot.GetComponentInParent<FieldPlots>().AddWayPointIMGToField(wayPointCropImg);
            // Thêm cảnh báo vào dictionary

            wayPointDictionary[_smallPlot].Add(wayPointCropImg);
        }
        else
        {
            Debug.LogWarning($"A warning of type {_wayPointType} already exists for this plot.");
        }
    }

    public void RemoveWayPointByType(SmallPlot _smallPlot, WarningType type)
    {
        if (_smallPlot == null) return;
        if (wayPointDictionary.ContainsKey(_smallPlot))
        {
            List<WayPointImage> warnings = wayPointDictionary[_smallPlot];
            for (int i = warnings.Count - 1; i >= 0; i--)
            {
                if (warnings[i].GetWayPointType() == type)
                {
                    _smallPlot.GetComponentInParent<FieldPlots>().RemoveWayPointIMGToFIeld(warnings[i]);
                    warnings[i].DestroyWayPoint();
                    warnings.RemoveAt(i);
                }
            }

            if (warnings.Count == 0)
            {
                wayPointDictionary.Remove(_smallPlot);
            }
        }
    }

    private void Update()
    {

    }
}
