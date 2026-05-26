using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UIAnim : MonoBehaviour
{
    public static UIAnim Instance;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void PopUp(GameObject go, float duration, Vector3 scale){
        go.transform.localScale = Vector3.zero;
        go.SetActive(true);
        go.transform.DOScale(scale, duration).SetEase(Ease.OutBack);
    }
    public void ClosePopup(GameObject go, float duration)
    {
        go.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InBack)
            .OnComplete(() => go.SetActive(false));
    }
    public void FadeInText(Text go, float duration){
        go.DOFade(1, duration).SetEase(Ease.InOutQuad);
    }
    public void FadeOutText(Text go, float duration){
        go.DOFade(0, duration).SetEase(Ease.InOutQuad);
    }
    public void TextAppear(TMP_Text go, string text, float duration){
        go.DOFade(0, duration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            go.text = text;
            go.DOFade(01, duration).SetEase(Ease.InOutQuad);
        });
    }
}
