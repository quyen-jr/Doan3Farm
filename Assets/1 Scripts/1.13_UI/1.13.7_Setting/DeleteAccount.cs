using FairyField.Logic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeleteAccount : MonoBehaviour
{
    //[SerializeField] private Button _deleteButton;// click to open delete panel
    //[SerializeField] private Transform deletePanel;
    //[SerializeField] private Button _cancelButton;
    //[SerializeField] private Button _confirmButton;
    //private bool isIndeleteProcess;
    //void Start()
    //{

    //}
    //private void OnEnable()
    //{
    //    _deleteButton.onClick.AddListener(OnDeleteButtonClicked);
    //    _cancelButton.onClick.AddListener(OnCancelButtonClicked);
    //    _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
    //}

    //private void OnDisable()
    //{
    //    _deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
    //    _cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
    //    _confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
    //}

    //private void OnDeleteButtonClicked()
    //{
    //    ToggleDeletePanel(true);
    //}

    //private void OnCancelButtonClicked()
    //{
    //    if (isIndeleteProcess) return;
    //    ToggleDeletePanel(false);
    //}
    //private void OnConfirmButtonClicked()
    //{
    //    if (isIndeleteProcess) return;
    //    GetProfileIdAndDelete();
    //}
    //private void ToggleDeletePanel(bool isActive)
    //{
    //    if (isIndeleteProcess) return;
    //    deletePanel.gameObject.SetActive(isActive);
    //}
    //public void GetProfileIdAndDelete()
    //{
    //    // Disable any buttons or UI elements related to account deletion if needed
    //    isIndeleteProcess = true;
    //    StartCoroutine(GetProfileRequest()); // get id ==> delete 
    //}

    //private IEnumerator DeleteAccountRequest(string idPlayer)
    //{
    //    string deleteURl = BaseAPI.API_DELETE + "/" + idPlayer;
    //    UnityWebRequest request = new UnityWebRequest(deleteURl, "DELETE")
    //    {
    //        downloadHandler = new DownloadHandlerBuffer()
    //    };
    //    // Đặt token vào header Authorization
    //    request.SetRequestHeader("Authorization", $"Bearer {UserData.instance.UserAccessToken}");
    //    request.SetRequestHeader("Content-Type", "application/json");

    //    yield return request.SendWebRequest();

    //    if (request.result == UnityWebRequest.Result.Success)
    //    {
    //        //   Debug.Log("Xóa tài khoản thành công.");
    //        SceneManager.LoadScene("LoginMenu", LoadSceneMode.Single);
    //        isIndeleteProcess = false;
    //    }
    //    else
    //    {
    //        //   Debug.Log($"Xóa tài khoản thất bại: {request.error}, Phản hồi: {request.downloadHandler.text}");
    //        isIndeleteProcess = false;
    //    }
    //}

    //private IEnumerator GetProfileRequest()
    //{
    //    UnityWebRequest request = new UnityWebRequest(BaseAPI.API_GETPROFILE, "GET")
    //    {
    //        downloadHandler = new DownloadHandlerBuffer()
    //    };

    //    // Đặt token vào header Authorization
    //    request.SetRequestHeader("Authorization", $"Bearer {UserData.instance.UserAccessToken}");
    //    request.SetRequestHeader("Content-Type", "application/json");

    //    yield return request.SendWebRequest();

    //    if (request.result == UnityWebRequest.Result.Success)
    //    {

    //        string jsonResponse = request.downloadHandler.text;
    //        //   Debug.Log("Raw Response: " + jsonResponse);

    //        UserProfileResponse userProfile = JsonUtility.FromJson<UserProfileResponse>(jsonResponse);

    //        // Check if the userProfile is valid and data is not null
    //        if (userProfile != null && userProfile.data != null && !string.IsNullOrEmpty(userProfile.data._id))
    //        {
    //            string userId = userProfile.data._id;
    //            StartCoroutine(DeleteAccountRequest(userId));
    //        }
    //        else
    //        {
    //            isIndeleteProcess = false;
    //            //  Debug.LogError("User ID is empty or could not be parsed correctly.");
    //        }
    //    }
    //    else
    //    {
    //        //  Debug.LogError($"Failed to get profile: {request.error}, Response: {request.downloadHandler.text}");
    //    }
    //}


    //[System.Serializable]
    //public class UserDataProfile
    //{
    //    public string _id;  // The user ID
    //    public string username;
    //    public string email;
    //    public bool is_active;
    //    public string access_token;
    //    public string refresh_token;
    //    // Add other fields as needed...
    //}

    //[System.Serializable]
    //public class UserProfileResponse
    //{
    //    public string message; // The message field
    //    public UserDataProfile data;  // This corresponds to the "data" object in the response
    //}
}
