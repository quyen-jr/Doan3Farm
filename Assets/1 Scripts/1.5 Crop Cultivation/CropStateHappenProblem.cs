using System.Collections.Generic;
using UnityEngine;

public class CropStateHappenProblem : MonoBehaviour
{
    [SerializeField] private Transform cropModel;

    [Header("Withering State")]
    [SerializeField] private List<Material> lackOfWaterMaterial;
    [SerializeField] private List<Material> witheringMaterial;
    private Material[] initalMaterial;

    [Header("Lack Of Fertilizer State")]
    [SerializeField] private Vector3 lackFertilizerScale;
    private Vector3 initialScale;


    private void Start()
    {

        initalMaterial = cropModel.GetComponent<MeshRenderer>().materials;
        initialScale = transform.localScale;
        // if set   state for fruits
    }
    public void SetNormalStateMaterials()
    {
        if (lackOfWaterMaterial.Count > 0)
        {
            cropModel.GetComponent<MeshRenderer>().materials = initalMaterial;
        }
        else
        {
            Debug.Log("dont have normal material");
        }
    }
    public void SetLackOfWaterStateMaterials()
    {
        if (lackOfWaterMaterial.Count > 0)
        {
            cropModel.GetComponent<MeshRenderer>().materials = lackOfWaterMaterial.ToArray();
        }
        else
        {
            Debug.Log("dont have wearthering material");
        }
    }
    public void SetWitheringStateMaterials()
    {
        if (lackOfWaterMaterial.Count > 0)
        {
            cropModel.GetComponent<MeshRenderer>().materials = witheringMaterial.ToArray();
            SetLackFertilizerScaleState();
        }
        else
        {
            Debug.Log("dont have wearthering material");
        }
    }
    public void SetLackFertilizerScaleState()
    {
        if (lackFertilizerScale == Vector3.zero)
        {
            Debug.Log("Error: withering scale is vecto zero");
            return;
        }
        transform.localScale = lackFertilizerScale;
    }
    public void SetNormalScaleState()
    {
        transform.localScale = initialScale;
    }
}
