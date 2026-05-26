using FairyField.Logic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Register : MonoBehaviour
{

    // [SerializeField] private InputField firstName;
    [SerializeField] private InputField username;
    [SerializeField] private InputField email;
    [SerializeField] private InputField password;
    // [SerializeField] private InputField verifyPassword;
    [SerializeField] private Button registerBtn;
    [SerializeField] private Button NavigateToLoginBtn;
    private bool _showPassword = false;

    private string apiURL = "ádasd";// BaseAPI.API_REGISTER;
    void Start()
    {
        NavigateToLoginBtn.onClick.AddListener(() => { UILoginManager.Instance.CreatePanel(0); });
        registerBtn.onClick.AddListener(() => { RegisterAccount(); });
    }
    public void RegisterAccount()
    {
        registerBtn.interactable = false;
        NavigateToLoginBtn.interactable = false;
        StartCoroutine(RegisterRequest(username.text, email.text, password.text));
    }
    private IEnumerator RegisterRequest(string _username, string _email, string _password)
    {

        RegisterData UserDataReceive = new RegisterData
        {
            username = _username,
            password = _password,
            email = _email,
        };

        // Serialize to JSON
        string jsonData = JsonUtility.ToJson(UserDataReceive, true);
        Debug.Log(jsonData);
        UnityWebRequest request = new UnityWebRequest(apiURL, "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData)),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<LoginResponseData>(request.downloadHandler.text);
            Debug.Log($"Message: {response.message} Access Token: {response.data.user.confirmationToken}");
            DisplayAuthencationPanel();
            UserData.instance.ResendToken = response.data.user.confirmationToken;
        }
        else
        {
            Debug.Log($"Register Failed: {request.error}, Response: {request.downloadHandler.text}");
            registerBtn.interactable = true;
            NavigateToLoginBtn.interactable = true;
        }
        // Debug.Log(11111);
    }

    private void DisplayAuthencationPanel()
    {
        //authencationPanel.gameObject.SetActive(true);
        UILoginManager.Instance.CreateAuthencationPanel(email.text);
    }
    public void TogglePassword()
    {
        _showPassword = !_showPassword;
        if (_showPassword)
        {
            password.contentType = InputField.ContentType.Standard;
        }
        else
        {
            password.contentType = InputField.ContentType.Password;
        }
        password.ActivateInputField();
    }
    [System.Serializable]
    private class RegisterData
    {
        // public string first_name;
        // public string last_name;
        public string username;
        public string password;
        public string email;

    }
    [System.Serializable]
    public class UserDataReceive
    {
        public string username;
        public string password;
        public string email;
        public string first_name;
        public string last_name;
        public string confirmationToken;
        public string confirmationExpires;
        public string _id;
    }

    [System.Serializable]
    public class Data
    {
        public UserDataReceive user;
    }

    [System.Serializable]
    public class LoginResponseData
    {
        public string message;
        public Data data;
    }
}
