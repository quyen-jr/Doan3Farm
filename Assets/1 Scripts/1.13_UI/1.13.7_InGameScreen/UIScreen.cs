using UnityEngine;

public class UIScreen : MonoBehaviour
{
    public bool IsOpen;

    protected virtual void Awake()
    {

    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
