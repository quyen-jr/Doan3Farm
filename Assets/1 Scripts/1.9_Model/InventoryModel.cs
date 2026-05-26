using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace FairyField.Model
{
    public class InventoryModel
    {

    }

    public partial class GetItemUserByLocationResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("items")]
        public ItemInventoryResponse[] Items { get; set; }
    }

    public partial class ItemInventoryResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public partial class ChangeLocationResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public DataChangeLocation Data { get; set; }
    }

    public partial class DataChangeLocation
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("userID")]
        public string UserId { get; set; }

      //  [JsonProperty("items")]
      //  public Item[] ItemChangeLocaitionResponse { get; set; }
//
        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("__v")]
        public long V { get; set; }
    }

    public partial class ItemChangeLocaitionResponse
    {
        [JsonProperty("itemID")]
        public string ItemId { get; set; }

        [JsonProperty("quantityItem")]
        public long QuantityItem { get; set; }

        [JsonProperty("location")]
        public long Location { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }

    public partial class GetAllLocation
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public DataLocation Data { get; set; }
    }

    public partial class DataLocation
    {
        [JsonProperty("locations")]
        public Location[] Locations { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
