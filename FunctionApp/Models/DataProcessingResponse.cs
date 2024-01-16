namespace AzureAutomation.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Extracting the ItemDefination from the json.
    /// </summary>
    public class ItemDefination
    {
        [JsonProperty("Whpk-Qty")]
        public string WhpkQty { get; set; }

        [JsonProperty("Whpk-Cost")]
        public string WhpkCost { get; set; }

        [JsonProperty("Item-Cost")]
        public string ItemCost { get; set; }

        [JsonProperty("Invoice-Cost")]
        public int InvoiceCost { get; set; }
    }

    /// <summary>
    /// Extracting the ProductDetail from the json.
    /// </summary>
    public class ProductDetail
    {
        public string ProductID { get; set; }
        public ItemDefination ItemDefination { get; set; }
    }

    /// <summary>
    /// Extracting the Stop from the json.
    /// </summary>
    public class Stop
    {
        public string ShipmentSeq { get; set; }
        public string DestinationFacilityID { get; set; }
        public string ShipmentCost { get; set; }
    }

    /// <summary>
    /// Extracting the Shipment from the json.
    /// </summary>
    public class Shipment
    {
        public string ShipmentID { get; set; }
        public List<Stop> Stop { get; set; }
    }
    /// <summary>
    /// Extracting the OriginalOrder from the json.
    /// </summary>
    public class OriginalOrder
    {
        public string TotalNumOfShipments { get; set; }
        public string FacilityID { get; set; }
        public string StoreNum { get; set; }
        public List<ProductDetail> ProductDetails { get; set; }
        public Shipment Shipment { get; set; }
    }

    /// <summary>
    /// Extracting the ContextInformation from the json.
    /// </summary>
    public class ContextInformation
    {
        public string Message { get; set; }
        public string OriginFacilityID { get; set; }
    }

    /// <summary>
    /// Deserialises the input json to classes.
    /// </summary>
    public class DataProcessingResponse
    {
        public ContextInformation ContextInformation { get; set; }
        public List<OriginalOrder> OriginalOrder { get; set; }
    }

}