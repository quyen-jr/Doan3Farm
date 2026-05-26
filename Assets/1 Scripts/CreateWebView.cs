using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CreateWebView : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    private GameObject webview;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitWebView()
    {
        webview=GameObject.Instantiate(prefab);
       //Initialization(prefab,)
    }
    public void DestroyWebView()
    {
        if (webview != null)
        {
            GameObject.Destroy(webview);
        }
    }
}
