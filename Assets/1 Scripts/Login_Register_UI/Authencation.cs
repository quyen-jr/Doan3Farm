using FairyField.Logic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Register;

public class Authencation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private InputField inputCode;
    [SerializeField] private Button submmitBtn;
    [SerializeField] private TMP_InputField[] inputCodes;
    [SerializeField] private Button reSendButton;
    [SerializeField] private Text timeCountDownText;
    [SerializeField] private Button cancleButton;
    private float countdown = 5f;
    private float timeElapsed;
    private bool canResend = false;
    private string emailText;
    private int _currentInputCode = 0;
    private string apiURL = "ádasdsd";// BaseAPI.API_CONFIRM;
    void Start()
    {
        submmitBtn.onClick.AddListener(() => { AuthencationAccount(); });
        reSendButton.onClick.AddListener(() =>
        {
            if (!timeCountDownText.gameObject.activeSelf && canResend)
            {
                canResend = false;
                timeCountDownText.gameObject.SetActive(true);
                StartCoroutine(ResendCode(UserData.instance.ResendToken));

            }
        });
    }
    public void SetEmailTitle(string text)
    {
        emailText = text;
        titleText.text = "Code đã được gửi tới " + text + ". Vui lòng nhập code";
    }
    private void Update()
    {
        if (countdown > 0 && timeCountDownText.gameObject.activeSelf)
        {
            timeElapsed += Time.deltaTime;
            countdown = 5f - timeElapsed;
            timeCountDownText.text = Mathf.Ceil(countdown).ToString() + "s";
            if (countdown <= 0)
            {
                countdown = 5f;
                timeCountDownText.text = "0s";
                timeCountDownText.gameObject.SetActive(false);
                canResend = true;
                timeElapsed = 0;
            }
        }
    }
    private void AuthencationAccount()
    {
        string validateCode = "";
        foreach (TMP_InputField code in inputCodes)
        {
            validateCode += code.text;
        }
        Debug.Log(validateCode);
        StartCoroutine(AuthencationRequest(validateCode));
    }
    private IEnumerator AuthencationRequest(string _inputCode)
    {


        apiURL += _inputCode;
        UnityWebRequest request = new UnityWebRequest(apiURL, "POST")
        {
            //uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData)),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<AuthencationResponseData>(request.downloadHandler.text);
            Debug.Log($"Message: {response.message}");
            LoadSceneGame();
        }
        else
        {
            // Debug.Log($"Login Failed: {request.error}, Response: {request.downloadHandler.text}");
            UILoginManager.Instance.CreateNotification("Verification code is incorrect");
        }
    }
    public IEnumerator ResendCode(string accessToken)
    {
        UnityWebRequest request = new UnityWebRequest("ádsad", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes("")),
            downloadHandler = new DownloadHandlerBuffer()
        };

        // Add the authorization token as a Bearer token
        request.SetRequestHeader("Authorization", $"Bearer {accessToken}");
        request.SetRequestHeader("Content-Type", "text/plain");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<LoginResponseData>(request.downloadHandler.text);
            UILoginManager.Instance.CreateAuthencationPanel(emailText);
            // Debug.Log(response.message);
            // UILoginManager.Instance.CreateAuthencationPanel();
            // SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            // UserData.instance.SetUsername(username.text);
        }
        else
        {
            Debug.Log($"Register Failed: {request.error}, Response: {request.downloadHandler.text}");
        }
    }
    private class AuthencationResponseData
    {
        public string message;
    }
    public void LoadSceneGame()
    {
        SceneManager.LoadScene(1);
    }
    public void JumpToNextInput()
    {
        if (_currentInputCode < inputCodes.Length - 1 && inputCodes[_currentInputCode].text.Length != 0)
        {
            _currentInputCode++;
            inputCodes[_currentInputCode].ActivateInputField();
        }
    }
    public void SetCurrentIndexCode(int index) => _currentInputCode = index;
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
