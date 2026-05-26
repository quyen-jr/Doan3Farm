using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    public GameObject menu;
    protected bool _isOpen = false;

    public virtual void Close() {
        _isOpen = false;
        menu.SetActive(false);
    }
    public virtual void Open(){
        _isOpen = true;
        menu.SetActive(true);
    }
    public bool IsOpen() => _isOpen;
}
