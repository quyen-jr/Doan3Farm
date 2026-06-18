using System;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using FairyField.Logic;

public class BagItemManager : MonoBehaviour
{
    public static BagItemManager Instance;

    [SerializeField] private List<BagItemSlot> bagItemSlots = new List<BagItemSlot>();
    [SerializeField] private List<BagItemConfig> itemCatalog = new List<BagItemConfig>();

    public event Action OnBagItemChanged;

    private List<BagItemSlot> defaultBagItemSlots = new List<BagItemSlot>();
    private string saveFilePath;

    private void Awake()
    {
        Instance = this;
        saveFilePath = Path.Combine(Application.persistentDataPath, "BagItemRoomPlayerData.json");

        CacheDefaultBagItems();
        RegisterConfigsFromSlots(defaultBagItemSlots);
        LoadBagData();
    }

    private void OnValidate()
    {
        RegisterConfigsFromSlots(bagItemSlots);
    }

    public List<BagItemSlot> GetAllSlots()
    {
        return bagItemSlots;
    }

    public List<BagItemSlot> GetSlotsByCategory(EBagItemCategory category)
    {
        return bagItemSlots.FindAll(slot =>
            slot != null &&
            slot.itemConfig != null &&
            slot.itemConfig.category == category &&
            slot.amount > 0
        );
    }

    public void AddItem(BagItemConfig itemConfig, int amount)
    {
        if (itemConfig == null || itemConfig.itemData == null || amount <= 0)
            return;

        BagItemSlot slot = bagItemSlots.Find(x =>
            x.itemConfig != null &&
            x.itemConfig.itemData == itemConfig.itemData
        );

        if (slot != null)
        {
            slot.amount += amount;
        }
        else
        {
            bagItemSlots.Add(new BagItemSlot
            {
                itemConfig = itemConfig,
                amount = amount
            });
        }

        OnBagItemChanged?.Invoke();
        SaveBagData();
    }

    public void RemoveItem(BagItemConfig itemConfig, int amount)
    {
        if (itemConfig == null || itemConfig.itemData == null || amount <= 0)
            return;

        BagItemSlot slot = bagItemSlots.Find(x =>
            x.itemConfig != null &&
            x.itemConfig.itemData == itemConfig.itemData
        );

        if (slot == null) return;

        slot.amount -= amount;

        if (slot.amount <= 0)
        {
            bagItemSlots.Remove(slot);
        }

        OnBagItemChanged?.Invoke();
        SaveBagData();
    }

    public bool TryUseItem(
        EBagItemCategory category,
        int amount = 1,
        ESeedsCircleOptionType seedType = ESeedsCircleOptionType.none
    )
    {
        if (amount <= 0) return false;

        BagItemSlot slot = FindItemSlot(category, amount, seedType);

        return slot != null;
    }

    public bool DecreaseItemAmount(
        EBagItemCategory category,
        int amount = 1,
        ESeedsCircleOptionType seedType = ESeedsCircleOptionType.none
    )
    {
        if (amount <= 0) return false;

        BagItemSlot slot = FindItemSlot(category, amount, seedType);

        if (slot == null)
        {
            return false;
        }

        slot.amount -= amount;

        if (slot.amount <= 0)
        {
            bagItemSlots.Remove(slot);
        }

        OnBagItemChanged?.Invoke();
        SaveBagData();

        return true;
    }

    private BagItemSlot FindItemSlot(
        EBagItemCategory category,
        int amount,
        ESeedsCircleOptionType seedType
    )
    {
        return bagItemSlots.Find(x =>
            x != null &&
            x.itemConfig != null &&
            x.itemConfig.category == category &&
            x.amount >= amount &&
            IsCorrectItemType(x.itemConfig, category, seedType)
        );
    }

    private bool IsCorrectItemType(
        BagItemConfig itemConfig,
        EBagItemCategory category,
        ESeedsCircleOptionType seedType
    )
    {
        if (category == EBagItemCategory.seed)
        {
            return itemConfig.seedType == seedType;
        }

        return true;
    }

    private void CacheDefaultBagItems()
    {
        defaultBagItemSlots = CloneSlots(bagItemSlots);
    }

    private void LoadBagData()
    {
        BagItemSaveFile saveFile = ReadSaveFile();
        PlayerBagData playerData = GetPlayerData(saveFile, false);

        if (playerData == null || playerData.bagItemSlots == null || playerData.bagItemSlots.Count == 0)
        {
            bagItemSlots = CloneSlots(defaultBagItemSlots);
            SaveBagData();
            return;
        }

        bagItemSlots = CreateSlotsFromSavedData(playerData.bagItemSlots);
    }

    private void SaveBagData()
    {
        RegisterConfigsFromSlots(bagItemSlots);

        BagItemSaveFile saveFile = ReadSaveFile();
        PlayerBagData playerData = GetPlayerData(saveFile, true);
        playerData.bagItemSlots = CreateSaveSlots(bagItemSlots);

        string json = JsonUtility.ToJson(saveFile, true);
        File.WriteAllText(saveFilePath, json);
    }

    private BagItemSaveFile ReadSaveFile()
    {
        if (!File.Exists(saveFilePath))
        {
            return new BagItemSaveFile();
        }

        string json = File.ReadAllText(saveFilePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new BagItemSaveFile();
        }

        BagItemSaveFile saveFile = JsonUtility.FromJson<BagItemSaveFile>(json);
        return saveFile ?? new BagItemSaveFile();
    }

    private PlayerBagData GetPlayerData(BagItemSaveFile saveFile, bool createIfMissing)
    {
        string roomName = GetCurrentRoomName();
        string playerName = GetCurrentPlayerName();

        RoomBagData roomData = saveFile.rooms.Find(x => x.roomName == roomName);
        if (roomData == null && createIfMissing)
        {
            roomData = new RoomBagData { roomName = roomName };
            saveFile.rooms.Add(roomData);
        }

        if (roomData == null)
        {
            return null;
        }

        PlayerBagData playerData = roomData.players.Find(x => x.playerName == playerName);
        if (playerData == null && createIfMissing)
        {
            playerData = new PlayerBagData { playerName = playerName };
            roomData.players.Add(playerData);
        }

        return playerData;
    }

    private List<BagItemSlotSaveData> CreateSaveSlots(List<BagItemSlot> sourceSlots)
    {
        List<BagItemSlotSaveData> saveSlots = new List<BagItemSlotSaveData>();

        foreach (BagItemSlot slot in sourceSlots)
        {
            if (slot == null || slot.itemConfig == null || slot.itemConfig.itemData == null || slot.amount <= 0)
            {
                continue;
            }

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
            if (config == null || savedSlot.amount <= 0)
            {
                continue;
            }

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
        if (savedSlot == null)
        {
            return null;
        }

        BagItemConfig config = itemCatalog.Find(x =>
            x != null &&
            x.itemData != null &&
            GetItemId(x) == savedSlot.itemId
        );

        if (config != null)
        {
            return config;
        }

        return itemCatalog.Find(x =>
            x != null &&
            x.itemData != null &&
            (int)x.category == savedSlot.category &&
            (int)x.seedType == savedSlot.seedType
        );
    }

    private void RegisterConfigsFromSlots(List<BagItemSlot> slots)
    {
        if (slots == null)
        {
            return;
        }

        foreach (BagItemSlot slot in slots)
        {
            RegisterConfig(slot != null ? slot.itemConfig : null);
        }
    }

    private void RegisterConfig(BagItemConfig config)
    {
        if (config == null || config.itemData == null)
        {
            return;
        }

        if (itemCatalog.Exists(x => x != null && x.itemData == config.itemData))
        {
            return;
        }

        itemCatalog.Add(config);
    }

    private List<BagItemSlot> CloneSlots(List<BagItemSlot> sourceSlots)
    {
        List<BagItemSlot> clonedSlots = new List<BagItemSlot>();

        foreach (BagItemSlot slot in sourceSlots)
        {
            if (slot == null || slot.itemConfig == null || slot.itemConfig.itemData == null)
            {
                continue;
            }

            clonedSlots.Add(new BagItemSlot
            {
                itemConfig = slot.itemConfig,
                amount = slot.amount
            });
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

        if (!string.IsNullOrWhiteSpace(PhotonNetwork.NickName))
        {
            return PhotonNetwork.NickName;
        }

        if (UserData.instance != null && !string.IsNullOrWhiteSpace(UserData.instance.GetUsername()))
        {
            return UserData.instance.GetUsername();
        }

        return "Guest";
    }

    private string GetItemId(BagItemConfig config)
    {
        if (config == null || config.itemData == null)
        {
            return string.Empty;
        }

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
