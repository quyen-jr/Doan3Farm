using UnityEngine;
using UnityEngine.UI;

public class UICircleMenuOptionBase : MonoBehaviour
{
    [SerializeField] protected Image _imageOption;

    public void Visible(bool isVisible)
    {
        if (gameObject.activeSelf != isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}
