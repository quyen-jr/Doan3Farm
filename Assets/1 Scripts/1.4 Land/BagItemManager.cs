using System;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using FairyField.Logic;

// Thêm RequireComponent để nhắc nhở/tự động thêm PhotonView
[RequireComponent(typeof(PhotonView))]
public class BagItemManager : MonoBehaviourPunCallbacks
{
    public static BagItemManager Instance;

    [SerializeField] private List<BagItemSlot> bagItemSlots = new List<BagItemSlot>();
    [SerializeField] private List<BagItemConfig> itemCatalog = new List<BagItemConfig>();

    public event Action OnBagItemChanged;

    private List<BagItemSlot> defaultBagItemSlots = new List<BagItemSlot>();
    private string saveFilePath;
    private PhotonView photonView;

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
        saveFilePath = Path.Combine(Application.persistentDataPath, "BagItemRoomPlayerData.json");

        CacheDefaultBagItems();
        RegisterConfigsFromSlots(defaultBagItemSlots);
    }

    private void Start()
    {
        // Chuyển Load sang Start để đảm bảo Photon Network đã sẵn sàng
        LoadBagData();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        LoadBagData(); // Lúc này InRoom đã = True, Client sẽ gọi RPC xin Host data!
    }
    private void OnValidate()
    {
        RegisterConfigsFromSlots(bagItemSlots);
    }

    // ==========================================
    // LOGIC IN-GAME CỦA BẠN (GIỮ NGUYÊN)
    // ==========================================
    public List<BagItemSlot> GetAllSlots() => bagItemSlots;

    public List<BagItemSlot> GetSlotsByCategory(EBagItemCategory category)
    {
        return bagItemSlots.FindAll(slot =>
            slot != null && slot.itemConfig != null &&
            slot.itemConfig.category == category && slot.amount > 0);
    }

    public void AddItem(BagItemConfig itemConfig, int amount)
    {
        if (itemConfig == null || itemConfig.itemData == null || amount <= 0) return;

        BagItemSlot slot = bagItemSlots.Find(x => x.itemConfig != null && x.itemConfig.itemData == itemConfig.itemData);

        if (slot != null) slot.amount += amount;
        else bagItemSlots.Add(new BagItemSlot { itemConfig = itemConfig, amount = amount });

        OnBagItemChanged?.Invoke();
        SaveBagData(); // Gọi Save sau khi thêm
    }

    public void RemoveItem(BagItemConfig itemConfig, int amount)
    {
        if (itemConfig == null || itemConfig.itemData == null || amount <= 0) return;

        BagItemSlot slot = bagItemSlots.Find(x => x.itemConfig != null && x.itemConfig.itemData == itemConfig.itemData);
        if (slot == null) return;

        slot.amount -= amount;
        if (slot.amount <= 0) bagItemSlots.Remove(slot);

        OnBagItemChanged?.Invoke();
        SaveBagData(); // Gọi Save sau khi xóa
    }

    public bool TryUseItem(EBagItemCategory category, int amount = 1, ESeedsCircleOptionType seedType = ESeedsCircleOptionType.none)
    {
        if (amount <= 0) return false;
        return FindItemSlot(category, amount, seedType) != null;
    }

    public bool DecreaseItemAmount(EBagItemCategory category, int amount = 1, ESeedsCircleOptionType seedType = ESeedsCircleOptionType.none)
    {
        if (amount <= 0) return false;

        BagItemSlot slot = FindItemSlot(category, amount, seedType);
        if (slot == null) return false;

        slot.amount -= amount;
        if (slot.amount <= 0) bagItemSlots.Remove(slot);

        OnBagItemChanged?.Invoke();
        SaveBagData();

        return true;
    }

    private BagItemSlot FindItemSlot(EBagItemCategory category, int amount, ESeedsCircleOptionType seedType)
    {
        return bagItemSlots.Find(x =>
            x != null && x.itemConfig != null && x.itemConfig.category == category &&
            x.amount >= amount && IsCorrectItemType(x.itemConfig, category, seedType));
    }

    private bool IsCorrectItemType(BagItemConfig itemConfig, EBagItemCategory category, ESeedsCircleOptionType seedType)
    {
        if (category == EBagItemCategory.seed) return itemConfig.seedType == seedType;
        return true;
    }

    private void CacheDefaultBagItems()
    {
        defaultBagItemSlots = CloneSlots(bagItemSlots);
    }

    // ==========================================
    // LOGIC LOAD DATA (ĐÃ SỬA CƠ CHẾ HOST-CLIENT)
    // ==========================================
    private void LoadBagData()
    {
        // Nếu chưa vào phòng thì chỉ dùng đồ mặc định, không đọc ổ cứng
        if (!PhotonNetwork.InRoom)
        {
            bagItemSlots = CloneSlots(defaultBagItemSlots);
            return;
        }
        // Nếu Offline hoặc là Máy Chủ (Master Client) -> Đọc file ổ cứng
        if (!PhotonNetwork.InRoom || PhotonNetwork.IsMasterClient)
        {
            LoadDataFromLocalDisk(GetCurrentPlayerName());
        }
        else
        {
            // Nếu là Client -> Xóa túi hiện tại, gửi RPC nhờ Host đưa data
            bagItemSlots.Clear();
            photonView.RPC(nameof(RpcRequestBagDataFromHost), RpcTarget.MasterClient, GetCurrentPlayerName());
        }
    }

    private void LoadDataFromLocalDisk(string targetPlayerName)
    {
        BagItemSaveFile saveFile = ReadSaveFile();
        PlayerBagData playerData = GetPlayerData(saveFile, targetPlayerName, false);

        if (playerData == null || playerData.bagItemSlots == null || playerData.bagItemSlots.Count == 0)
        {
            bagItemSlots = CloneSlots(defaultBagItemSlots);
            SaveBagData(); // Lưu lại data mặc định mới
            return;
        }

        bagItemSlots = CreateSlotsFromSavedData(playerData.bagItemSlots);
        OnBagItemChanged?.Invoke();
    }

    [PunRPC]
    private void RpcRequestBagDataFromHost(string requesterName, PhotonMessageInfo info)
    {
        // Chỉ Host mới xử lý yêu cầu này
        if (!PhotonNetwork.IsMasterClient) return;

        BagItemSaveFile saveFile = ReadSaveFile();
        PlayerBagData playerData = GetPlayerData(saveFile, requesterName, true);

        // Chuyển Data của người chơi đó thành chuỗi JSON
        string jsonData = JsonUtility.ToJson(playerData);

        // Gửi trả lại chuỗi JSON đó cho đúng cái máy Client vừa hỏi (info.Sender)
        photonView.RPC(nameof(RpcReceiveBagDataFromHost), info.Sender, jsonData);
    }

    [PunRPC]
    private void RpcReceiveBagDataFromHost(string jsonData)
    {
        // Client nhận được Data từ Host -> Giải nén ra và nạp vào túi
        PlayerBagData playerData = JsonUtility.FromJson<PlayerBagData>(jsonData);

        if (playerData == null || playerData.bagItemSlots == null || playerData.bagItemSlots.Count == 0)
        {
            bagItemSlots = CloneSlots(defaultBagItemSlots);
        }
        else
        {
            bagItemSlots = CreateSlotsFromSavedData(playerData.bagItemSlots);
        }

        OnBagItemChanged?.Invoke();
    }


    // ==========================================
    // LOGIC SAVE DATA (ĐÃ SỬA CƠ CHẾ HOST-CLIENT)
    // ==========================================
    private void SaveBagData()
    {
        if (!PhotonNetwork.InRoom) return;
        RegisterConfigsFromSlots(bagItemSlots);

        // Nếu Offline hoặc là Host -> Ghi thẳng vào ổ cứng
        if (!PhotonNetwork.InRoom || PhotonNetwork.IsMasterClient)
        {
            SaveDataToLocalDisk(GetCurrentPlayerName(), bagItemSlots);
        }
        else
        {
            // Nếu là Client -> Đóng gói túi đồ thành JSON, gửi nhờ Host lưu hộ
            PlayerBagData myData = new PlayerBagData
            {
                playerName = GetCurrentPlayerName(),
                bagItemSlots = CreateSaveSlots(bagItemSlots)
            };

            string jsonData = JsonUtility.ToJson(myData);
            photonView.RPC(nameof(RpcSaveBagDataToHost), RpcTarget.MasterClient, GetCurrentPlayerName(), jsonData);
        }
    }

    private void SaveDataToLocalDisk(string targetPlayerName, List<BagItemSlot> slotsToSave)
    {
        BagItemSaveFile saveFile = ReadSaveFile();
        PlayerBagData playerData = GetPlayerData(saveFile, targetPlayerName, true);

        playerData.bagItemSlots = CreateSaveSlots(slotsToSave);

        string json = JsonUtility.ToJson(saveFile, true);
        File.WriteAllText(saveFilePath, json);
    }

    [PunRPC]
    private void RpcSaveBagDataToHost(string playerName, string jsonData)
    {
        // Chỉ Host mới được quyền ghi file
        if (!PhotonNetwork.IsMasterClient) return;

        PlayerBagData newDataFromClient = JsonUtility.FromJson<PlayerBagData>(jsonData);
        if (newDataFromClient == null) return;

        BagItemSaveFile saveFile = ReadSaveFile();

        // Tìm phòng và cập nhật dữ liệu người chơi tương ứng
        string roomName = GetCurrentRoomName();
        RoomBagData roomData = saveFile.rooms.Find(x => x.roomName == roomName);
        if (roomData == null)
        {
            roomData = new RoomBagData { roomName = roomName };
            saveFile.rooms.Add(roomData);
        }

        // Xóa data cũ của player đó, đè data mới mà Client vừa gửi
        roomData.players.RemoveAll(x => x.playerName == playerName);
        roomData.players.Add(newDataFromClient);

        // Ghi xuống file ở máy Host
        string finalJson = JsonUtility.ToJson(saveFile, true);
        File.WriteAllText(saveFilePath, finalJson);
    }


    // ==========================================
    // CÁC HÀM TIỆN ÍCH LÀM VIỆC VỚI DỮ LIỆU
    // ==========================================
    private BagItemSaveFile ReadSaveFile()
    {
        if (!File.Exists(saveFilePath)) return new BagItemSaveFile();

        string json = File.ReadAllText(saveFilePath);
        if (string.IsNullOrWhiteSpace(json)) return new BagItemSaveFile();

        BagItemSaveFile saveFile = JsonUtility.FromJson<BagItemSaveFile>(json);
        return saveFile ?? new BagItemSaveFile();
    }

    // LƯU Ý: Đã sửa hàm này để nhận tham số playerName thay vì tự lấy tên của máy hiện tại
    private PlayerBagData GetPlayerData(BagItemSaveFile saveFile, string targetPlayerName, bool createIfMissing)
    {
        string roomName = GetCurrentRoomName();

        RoomBagData roomData = saveFile.rooms.Find(x => x.roomName == roomName);
        if (roomData == null && createIfMissing)
        {
            roomData = new RoomBagData { roomName = roomName };
            saveFile.rooms.Add(roomData);
        }

        if (roomData == null) return null;

        PlayerBagData playerData = roomData.players.Find(x => x.playerName == targetPlayerName);
        if (playerData == null && createIfMissing)
        {
            playerData = new PlayerBagData { playerName = targetPlayerName };
            roomData.players.Add(playerData);
        }

        return playerData;
    }

    private List<BagItemSlotSaveData> CreateSaveSlots(List<BagItemSlot> sourceSlots)
    {
        List<BagItemSlotSaveData> saveSlots = new List<BagItemSlotSaveData>();
        foreach (BagItemSlot slot in sourceSlots)
        {
            if (slot == null || slot.itemConfig == null || slot.itemConfig.itemData == null || slot.amount <= 0) continue;

            saveSlots.Add(new BagItemSlotSaveData
            {
                itemId = GetItemId(slot.itemConfig),
                category = (int)slot.itemConfig.category,
                seedType = (int)slot.itemConfig.seedType,
                amount = slot.amount
            });
        }
        return saveSlots;
    }

    private List<BagItemSlot> CreateSlotsFromSavedData(List<BagItemSlotSaveData> savedSlots)
    {
        List<BagItemSlot> loadedSlots = new List<BagItemSlot>();
        foreach (BagItemSlotSaveData savedSlot in savedSlots)
        {
            BagItemConfig config = FindConfigBySavedData(savedSlot);
            if (config == null || savedSlot.amount <= 0) continue;

            loadedSlots.Add(new BagItemSlot
            {
                itemConfig = config,
                amount = savedSlot.amount
            });
        }
        return loadedSlots;
    }

    private BagItemConfig FindConfigBySavedData(BagItemSlotSaveData savedSlot)
    {
        if (savedSlot == null) return null;

        BagItemConfig config = itemCatalog.Find(x => x != null && x.itemData != null && GetItemId(x) == savedSlot.itemId);
        if (config != null) return config;

        return itemCatalog.Find(x => x != null && x.itemData != null &&
            (int)x.category == savedSlot.category && (int)x.seedType == savedSlot.seedType);
    }

    private void RegisterConfigsFromSlots(List<BagItemSlot> slots)
    {
        if (slots == null) return;
        foreach (BagItemSlot slot in slots) RegisterConfig(slot != null ? slot.itemConfig : null);
    }

    private void RegisterConfig(BagItemConfig config)
    {
        if (config == null || config.itemData == null) return;
        if (itemCatalog.Exists(x => x != null && x.itemData == config.itemData)) return;
        itemCatalog.Add(config);
    }

    private List<BagItemSlot> CloneSlots(List<BagItemSlot> sourceSlots)
    {
        List<BagItemSlot> clonedSlots = new List<BagItemSlot>();
        foreach (BagItemSlot slot in sourceSlots)
        {
            if (slot == null || slot.itemConfig == null || slot.itemConfig.itemData == null) continue;
            clonedSlots.Add(new BagItemSlot { itemConfig = slot.itemConfig, amount = slot.amount });
        }
        return clonedSlots;
    }

    private string GetCurrentRoomName()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom != null && !string.IsNullOrWhiteSpace(PhotonNetwork.CurrentRoom.Name))
        {
            return PhotonNetwork.CurrentRoom.Name;
        }
        return "OfflineRoom";
    }

    private string GetCurrentPlayerName()
    {
        if (PhotonNetwork.LocalPlayer != null && !string.IsNullOrWhiteSpace(PhotonNetwork.LocalPlayer.NickName))
        {
            return PhotonNetwork.LocalPlayer.NickName;
        }
        if (!string.IsNullOrWhiteSpace(PhotonNetwork.NickName)) return PhotonNetwork.NickName;
        if (UserData.instance != null && !string.IsNullOrWhiteSpace(UserData.instance.GetUsername())) return UserData.instance.GetUsername();
        return "Guest";
    }

    private string GetItemId(BagItemConfig config)
    {
        if (config == null || config.itemData == null) return string.Empty;
        return config.itemData.name;
    }
}

[Serializable]
public class BagItemSaveFile
{
    public List<RoomBagData> rooms = new List<RoomBagData>();
}

[Serializable]
public class RoomBagData
{
    public string roomName;
    public List<PlayerBagData> players = new List<PlayerBagData>();
}

[Serializable]
public class PlayerBagData
{
    public string playerName;
    public List<BagItemSlotSaveData> bagItemSlots = new List<BagItemSlotSaveData>();
}

[Serializable]
public class BagItemSlotSaveData
{
    public string itemId;
    public int category;
    public int seedType;
    public int amount;
}