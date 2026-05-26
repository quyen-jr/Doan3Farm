using System.Collections.Generic;
using UnityEngine;

public class UICircleMenuListOptionsBase : MonoBehaviour
{
    [SerializeField] private UICircleMenuOptionBase _uiOptionPrefab;
    [SerializeField] protected UICircleMenuBase _uICircleMenuBase;
    [SerializeField] private float _margin = 0.25f; // % khoảng cách thụt vào tính từ đỉnh

    public List<UICircleMenuOptionBase> _uICircleMenuOptions = new List<UICircleMenuOptionBase>();

    protected virtual void Awake()
    {

    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    public void GenerateOption(float count, ECircleMenuIndexSelectedType eCircleMenuIndexSelectedType)
    {
        float width = ((RectTransform)transform).rect.width / 2;
        float eulerOneOption = 360 / count;
        float offsetEuler = eulerOneOption / 2;
        for (int i = 0; i < count; i++)
        {
            float currentEuler = 0;
            switch (eCircleMenuIndexSelectedType)
            {
                case ECircleMenuIndexSelectedType.clockwise:
                    currentEuler = -(offsetEuler + eulerOneOption * i);
                    break;
                case ECircleMenuIndexSelectedType.counterclockwise:
                    currentEuler = offsetEuler + eulerOneOption * i;
                    break;

            }
            var option = Instantiate(_uiOptionPrefab, transform);
            option.transform.localPosition = FindVectorEuler(currentEuler, width);
            _uICircleMenuOptions.Add(option);
        }

        FillData();
    }

    public Vector3 FindVectorEuler(float euler, float width)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, euler);
        Vector3 A = rotation * Vector3.right * (width - width * _margin);
        return A;
    }

    protected virtual void FillData() { }

    public UICircleMenuOptionBase GetOptionByIndex(int index) => _uICircleMenuOptions[index];
}
