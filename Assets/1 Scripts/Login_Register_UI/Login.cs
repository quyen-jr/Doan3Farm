using UnityEngine;


public class Login : MonoBehaviour
{
    //[SerializeField] private InputField username;
    //[SerializeField] private InputField password;
    //[SerializeField] private Button submitBtn;
    //[SerializeField] private Button registerBtn;
    //[SerializeField] private PlayerRefScriptableObject playerRef;
    //private bool _showPassword = false;

    //void Start()
    //{
    //    registerBtn.onClick.AddListener(() => { UILoginManager.Instance.CreatePanel(1); });
    //    submitBtn.onClick.AddListener(() => { LoginAccount(); });
    //    Debug.Log(playerRef);
    //}
    //public void LoginAccount()
    //{
    //    registerBtn.interactable = false;
    //    submitBtn.interactable = false;
    //    StartCoroutine(LoginRequest(username.text, password.text));
    //}
    //private IEnumerator LoginRequest(string _email, string _password)
    //{


    //    LoginData userData = new LoginData
    //    {
    //        username = _email,
    //        password = _password
    //    };

    //    // Serialize to JSON
    //    string jsonData = JsonUtility.ToJson(userData, true);
    //    Debug.Log(jsonData);
    //    UnityWebRequest request = new UnityWebRequest(BaseAPI.API_LOGIN, "POST")
    //    {
    //        uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData)),
    //        downloadHandler = new DownloadHandlerBuffer()
    //    };
    //    request.SetRequestHeader("Content-Type", "application/json");

    //    yield return request.SendWebRequest();

    //    if (request.result == UnityWebRequest.Result.Success)
    //    {
    //        var response = JsonUtility.FromJson<LoginResponseData>(request.downloadHandler.text);
    //        Debug.Log($"Message: {response.message}, Access Token: {response.data.accessToken}");
    //        UserData.instance.UserAccessToken = response.data.accessToken;
    //        Debug.Log(response.data.is_active);
    //        // when user not active account
    //        if (!response.data.is_active)
    //        {
    //         //   Debug.Log("send request resend");
    //            //UILoginManager.Instance.CreateAuthencationPanel();
    //            StartCoroutine(ResendCode(response.data.accessToken));


    //        }
    //        else
    //        {
    //            playerRef.playerName = username.text;
    //            SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    //        }
    //        UserData.instance.SetUsername(_email);
    //        // LoadSceneGame();
    //    }
    //    else
    //    {
    //        Debug.Log($"Login Failed: {request.result}, Response: {request.downloadHandler.text}");
    //        UILoginManager.Instance.CreateNotification("Account or password is incorrect");
    //        registerBtn.interactable = true;
    //        submitBtn.interactable = true;
    //    }
    //}
    //public IEnumerator ResendCode(string accessToken)
    //{
    //    UnityWebRequest request = new UnityWebRequest(BaseAPI.API_RESENDCODE, "POST")
    //    {
    //        uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes("")),
    //        downloadHandler = new DownloadHandlerBuffer()
    //    };

    //    // Add the authorization token as a Bearer token
    //    request.SetRequestHeader("Authorization", $"Bearer {UserData.instance.UserAccessToken}");
    //    request.SetRequestHeader("Content-Type", "text/plain");

    //    yield return request.SendWebRequest();

    //    //Debug.Log()
    //    if (request.result == UnityWebRequest.Result.Success)
    //    {
    //       // var response = JsonUtility.FromJson<LoginResponseData>(request.downloadHandler.text);
    //        UILoginManager.Instance.CreateAuthencationPanel("email của bạn");
    //        // Debug.Log(response.message);
    //        // UILoginManager.Instance.CreateAuthencationPanel();
    //        // SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    //        // UserData.instance.SetUsername(username.text);
    //    }
    //    else
    //    {
    //        Debug.Log($"Register Failed: {request.error}, Response: {request.downloadHandler.text}");
    //    }
    //}


    //public void LoadSceneGame()
    //{
    //    SceneManager.LoadScene(1);
    //}
    //public void TogglePassword()
    //{
    //    _showPassword = !_showPassword;
    //    if (_showPassword)
    //    {
    //        password.contentType = InputField.ContentType.Standard;
    //    }
    //    else
    //    {
    //        password.contentType = InputField.ContentType.Password;
    //    }
    //    password.ActivateInputField();
    //}
    //[System.Serializable]
    //private class LoginResponseData
    //{
    //    public string message;
    //    public TokenData data;
    //}
    //[System.Serializable]
    //private class LoginData
    //{
    //    public string username;
    //    public string password;

    //}

    //[System.Serializable]
    //private class TokenData
    //{
    //    public string accessToken, refreshToken;
    //    public bool is_active;
    //}

}
