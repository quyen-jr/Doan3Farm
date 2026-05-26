using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace FairyField.Model
{
    public class StoreModel : MonoBehaviour
    {
        
    }

    [System.Serializable]
    public partial class GetItemByTypeModel
    {
        [JsonProperty("data")]
        public StoreItem[] Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    [System.Serializable]
    public partial class StoreItem
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("buy_price")]
        public int BuyPrice { get; set; }

        [JsonProperty("sell_price")]
        public int SellPrice { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("rate")]
        public double Rate { get; set; }

        [JsonProperty("itemType")]
        public ItemType ItemType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public Uri Image { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("__v")]
        public int V { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
    }

    [System.Serializable]
    public partial class ItemType
    {
        [JsonProperty("deletedAt")]
        public object DeletedAt { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }
    }

    [System.Serializable]
    public partial class BuyItemResult
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("itemUsers")]
        public ItemUser[] ItemUsers { get; set; }
    }

    [System.Serializable]
    public partial class ItemUser
    {
        [JsonProperty("userID")]
        public string UserId { get; set; }

        [JsonProperty("itemID")]
        public string ItemId { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("__v")]
        public long V { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
