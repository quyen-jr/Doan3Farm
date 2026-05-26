using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NavBarBtn : MonoBehaviour,IPointerUpHandler, IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    [SerializeField] protected Image panelImage;
    [SerializeField] protected Color initialColor;
    [SerializeField] protected Color clickColor;
    [SerializeField] protected GameObject panel;

    [SerializeField] private Sprite initialSprite;
    [SerializeField] protected Sprite clickSprite;
    private void Start()
    {
        panelImage.color = initialColor;
        panelImage.sprite = initialSprite;
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        panelImage.sprite = clickSprite;
        panelImage.color = clickColor;
        UIController.Instance.TogglePanel(panel, true);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      //  panelImage.color = clickColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       // panelImage.color= initialColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        panelImage.color = initialColor;
        panelImage.sprite=initialSprite;
    }

}
