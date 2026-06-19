using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FairyField.Logic;
using System.Collections.Generic;
using System;
using System.IO; // QUAN TRỌNG: Thêm thư viện này để đọc/ghi file từ ổ cứng

public class Launcher : MonoBehaviourPunCallbacks
{
    [Header("UI Input")]
    [SerializeField] private InputField createRoomInput;
    [SerializeField] private InputField joinRoomInput;

    [Header("UI Button & Panels")]
    [SerializeField] private GameObject CreateHostPanel;
    [SerializeField] private Button createHostButton;
    [SerializeField] private Button joinHostButton;

    [SerializeField] private GameObject existHostPanel;
    [SerializeField] private Button JoinExistHostButton;

    [Header("UI Text")]
    [SerializeField] private TMP_Text statusText;

    [Header("Room Settings")]
    [SerializeField] private byte maxPlayers = 4;

    // Tên file JSON của bạn
    private string saveFileName = "BagItemRoomPlayerData.json"; 
    private string existingRoomName = "";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        ConfigurePhotonIdentity();
        
        // Đọc file từ ổ cứng ngay khi khởi động
        CheckExistingPlayerData();

        SetStatus("Đang kết nối tới Photon...");

        createHostButton.interactable = false;
        joinHostButton.interactable = false;
        if (JoinExistHostButton != null) JoinExistHostButton.interactable = false;

        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnEnable()
    {
        base.OnEnable();
        createHostButton.onClick.AddListener(CreateHost);
        joinHostButton.onClick.AddListener(JoinHost);
        if (JoinExistHostButton != null) JoinExistHostButton.onClick.AddListener(JoinExistingHost);
    }

    private void OnDisable()
    {
        createHostButton.onClick.RemoveListener(CreateHost);
        joinHostButton.onClick.RemoveListener(JoinHost);
        if (JoinExistHostButton != null) JoinExistHostButton.onClick.RemoveListener(JoinExistingHost);
        base.OnDisable();
    }

    // --- LOGIC ĐỌC TRỰC TIẾP TỪ THƯ MỤC LƯU TRỮ ---
    private void CheckExistingPlayerData()
    {
        // Application.persistentDataPath tự động trỏ tới: C:\Users\TênUser\AppData\LocalLow\TênCôngTy\TênGame
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
        
        Debug.Log($"Đang tìm file save tại: {filePath}");

        // Kiểm tra xem file có tồn tại không
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Không tìm thấy file save. Bật Panel tạo phòng mới.");
            ShowCreatePanel();
            return;
        }

        try
        {
            // Đọc toàn bộ chữ trong file JSON
            string jsonContent = File.ReadAllText(filePath);
            
            // Ép kiểu về cấu trúc class của bạn
            BagItemSaveFile data = JsonUtility.FromJson<BagItemSaveFile>(jsonContent);
            
            if (data != null && data.rooms != null)
            {
                string currentName = PhotonNetwork.NickName;
                Debug.Log($"Đang kiểm tra tên: {currentName}");

                foreach (var room in data.rooms)
                {
                    foreach (var player in room.players)
                    {
                        if (player.playerName.Trim() == currentName.Trim())
                        {
                            existingRoomName = room.roomName;
                            ShowExistPanel();
                            Debug.Log($"=> TÌM THẤY! Bạn đã ở phòng: {existingRoomName}");
                            return; 
                        }
                    }
                }
            }

            // Nếu đọc xong không thấy tên trùng khớp
            Debug.Log("Không có tên người chơi này trong file save.");
            ShowCreatePanel();
        }
        catch (Exception e)
        {
            Debug.LogError("Lỗi khi đọc file JSON: " + e.Message);
            ShowCreatePanel();
        }
    }

    private void ShowCreatePanel()
    {
        if (CreateHostPanel != null) CreateHostPanel.SetActive(true);
        if (existHostPanel != null) existHostPanel.SetActive(false);
    }

    private void ShowExistPanel()
    {
        if (CreateHostPanel != null) CreateHostPanel.SetActive(false);
        if (existHostPanel != null) existHostPanel.SetActive(true);
    }
    // ------------------------------------------

    public void JoinExistingHost()
    {
        if (!string.IsNullOrEmpty(existingRoomName))
        {
            SetStatus("Đang join lại phòng cũ: " + existingRoomName);
            PhotonNetwork.JoinRoom(existingRoomName); 
        }
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
        if (JoinExistHostButton != null) JoinExistHostButton.interactable = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SetStatus("Đã vào Lobby.");
    }

    public void CreateHost()
    {
        string roomId = createRoomInput.text.Trim();
        if (string.IsNullOrEmpty(roomId)) return;

        RoomOptions roomOptions = new RoomOptions { MaxPlayers = maxPlayers, IsVisible = true, IsOpen = true };
        SetStatus("Đang tạo phòng: " + roomId);
        PhotonNetwork.CreateRoom(roomId, roomOptions);
    }

    public void JoinHost()
    {
        string roomId = joinRoomInput.text.Trim();
        if (string.IsNullOrEmpty(roomId)) return;

        SetStatus("Đang join phòng: " + roomId);
        PhotonNetwork.JoinRoom(roomId);
    }

    public override void OnCreatedRoom()
    {
        SetStatus("Tạo phòng thành công: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        SetStatus("Đã vào phòng: " + PhotonNetwork.CurrentRoom.Name);
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel("SampleScene");
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => SetStatus("Tạo phòng thất bại: " + message);
    public override void OnJoinRoomFailed(short returnCode, string message) => SetStatus("Join phòng thất bại: " + message);
    public override void OnDisconnected(DisconnectCause cause) => SetStatus("Mất kết nối: " + cause);

    private void SetStatus(string message)
    {
        Debug.Log(message);
        if (statusText != null) statusText.text = message;
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