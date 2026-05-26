using UnityEngine;
using UnityEngine.UI;

public class NavBarMenu : MonoBehaviour
{

    private RectTransform navBarRectTransform;
    [SerializeField]private RectTransform navBarContentRectTransform;
    private float initalNavBarHeight;
    private float targetNavBarHeight = 160f;

    private bool inProcessToggleNavbar = true;
    private bool isProcessInDone = true;

    [SerializeField] private Button toggleNavBarBTN;
    private RectTransform toggleBtnRectTransform;
    private float initalBtnHeight;
    private float targetBtnHeight = -20f;

    [SerializeField] private Image toggleButtonImage;
    private RectTransform imageRectTransform;
    private float initalImageRotation = 0f;
    private float targetRotation = 180f;

    private bool canClickElement;
    private float duration = 0.25f;
    private float elapsedTime = 0f;

    private Vector2 lastScreenSize;

    private void Start()
    {
        SetUpNavBar();
        navBarRectTransform = GetComponent<RectTransform>();
        initalNavBarHeight =navBarContentRectTransform.anchoredPosition.y;
        targetNavBarHeight = navBarContentRectTransform.rect.height;



        toggleBtnRectTransform = toggleNavBarBTN.GetComponent<RectTransform>();
        initalBtnHeight = toggleBtnRectTransform.anchoredPosition.y;
        imageRectTransform = toggleButtonImage.GetComponent<RectTransform>();
        toggleNavBarBTN.onClick.AddListener(() => { ToggleNavbar(); });
        lastScreenSize= new Vector2(Screen.width, Screen.height);
    }

    private void SetUpNavBar()
    {

    }

    private void Update()
    {
       if(targetNavBarHeight!= navBarContentRectTransform.rect.height + 11f)
            targetNavBarHeight = navBarContentRectTransform.rect.height + 11f;
  
        if (isProcessInDone) return;
        
        // close  menu 
        CheckCloseNavBarAction();
        // open  menu 
        CheckOpenNavBarAction();
    }

    private void CheckOpenNavBarAction()
    {
        if (inProcessToggleNavbar && !isProcessInDone)
        {
            canClickElement = false;

            if (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newHeightNavBar = Mathf.Lerp(targetNavBarHeight, initalNavBarHeight, elapsedTime / duration);
                navBarRectTransform.anchoredPosition = new Vector2(navBarRectTransform.anchoredPosition.x, newHeightNavBar);

                //float newHeightToggleBtn = Mathf.Lerp(targetBtnHeight, initalBtnHeight, elapsedTime / duration);
                //toggleBtnRectTransform.anchoredPosition = new Vector2(toggleBtnRectTransform.anchoredPosition.x, newHeightToggleBtn);

                float newRotation = Mathf.Lerp(targetRotation, initalImageRotation, elapsedTime / duration);
                imageRectTransform.rotation = Quaternion.Euler(0, 0, newRotation);

            }
            else
            {
              //  Debug.Log("canclick");
                elapsedTime = 0f;
                canClickElement = true;
                isProcessInDone = true;
            }

        }
    }

    private void CheckCloseNavBarAction()
    {
        if (!inProcessToggleNavbar && !isProcessInDone)
        {
            Debug.Log("closeeeee");
            canClickElement = false;

            if (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newHeightNavBar = Mathf.Lerp(initalNavBarHeight, targetNavBarHeight, elapsedTime / duration);
                navBarRectTransform.anchoredPosition = new Vector2(navBarRectTransform.anchoredPosition.x, newHeightNavBar);

                // toglle btn
                //float newHeightToggleBtn = Mathf.Lerp(initalBtnHeight, targetBtnHeight, elapsedTime / duration);
                //toggleBtnRectTransform.anchoredPosition = new Vector2(toggleBtnRectTransform.anchoredPosition.x, newHeightToggleBtn);
                // image
                float newRotation = Mathf.Lerp(initalImageRotation, targetRotation, elapsedTime / duration);
                imageRectTransform.rotation = Quaternion.Euler(0, 0, newRotation);
            }
            else
            {
            //    Debug.Log("canclick");
                canClickElement = true;
                isProcessInDone = true;
                elapsedTime = 0f;
            }

        }
    }

    public void ToggleNavbar()
    {
        inProcessToggleNavbar = !inProcessToggleNavbar;
        isProcessInDone = false;
    }
}
