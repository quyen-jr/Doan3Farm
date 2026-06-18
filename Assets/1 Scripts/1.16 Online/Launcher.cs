using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FairyField.Logic;

public class Launcher : MonoBehaviourPunCallbacks
{
    [Header("UI Input")]
    [SerializeField] private InputField createRoomInput;
    [SerializeField] private InputField joinRoomInput;

    [Header("UI Button")]
    [SerializeField] private Button createHostButton;
    [SerializeField] private Button joinHostButton;

    [Header("UI Text")]
    [SerializeField] private TMP_Text statusText;

    [Header("Room Settings")]
    [SerializeField] private byte maxPlayers = 4;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        ConfigurePhotonIdentity();
        SetStatus("Đang kết nối tới Photon...");

        createHostButton.interactable = false;
        joinHostButton.interactable = false;

        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnEnable()
    {
        base.OnEnable();

        createHostButton.onClick.AddListener(CreateHost);
        joinHostButton.onClick.AddListener(JoinHost);
    }

    private void OnDisable()
    {
        createHostButton.onClick.RemoveListener(CreateHost);
        joinHostButton.onClick.RemoveListener(JoinHost);

        base.OnDisable();
    }

    public override void OnConnected()
    {
        SetStatus("Đã kết nối tới Photon Name Server...");
    }

    public override void OnConnectedToMaster()
    {
        SetStatus("Đã kết nối Photon Master Server.");

        createHostButton.interactable = true;
        joinHostButton.interactable = true;

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SetStatus("Đã vào Lobby. Có thể tạo host hoặc join host.");
    }

    public void CreateHost()
    {
        string roomId = createRoomInput.text.Trim();

        if (string.IsNullOrEmpty(roomId))
        {
            SetStatus("Vui lòng nhập Room ID để tạo host.");
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        SetStatus("Đang tạo phòng với Room ID: " + roomId);

        PhotonNetwork.CreateRoom(roomId, roomOptions);
    }

    public void JoinHost()
    {
        string roomId = joinRoomInput.text.Trim();

        if (string.IsNullOrEmpty(roomId))
        {
            SetStatus("Vui lòng nhập Room ID để join.");
            return;
        }

        SetStatus("Đang join phòng với Room ID: " + roomId);

        PhotonNetwork.JoinRoom(roomId);
    }

    public override void OnCreatedRoom()
    {
        SetStatus("Tạo phòng thành công. Room ID: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        SetStatus("Đã vào phòng: " + PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("SampleScene");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetStatus("Tạo phòng thất bại: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetStatus("Join phòng thất bại: " + message);
    }

    //public override void OnJoinLobbyFailed(short returnCode, string message)
    //{
    //    SetStatus("Vào Lobby thất bại: " + message);
    //}

    public override void OnDisconnected(DisconnectCause cause)
    {
        SetStatus("Mất kết nối Photon: " + cause);
    }

    private void SetStatus(string message)
    {
        Debug.Log(message);

        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    private void ConfigurePhotonIdentity()
    {
        string username = "Guest";

        if (UserData.instance != null && !string.IsNullOrWhiteSpace(UserData.instance.GetUsername()))
        {
            username = UserData.instance.GetUsername().Trim();
        }

        PhotonNetwork.NickName = username;
        PhotonNetwork.AuthValues = new AuthenticationValues(username) { UserId = username };
    }
}
