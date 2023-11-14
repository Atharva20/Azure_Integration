namespace AzureAutomation.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

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

    public class ProductDetail
    {
        public string ProductID { get; set; }
        public ItemDefination ItemDefination { get; set; }
    }

    public class Stop
    {
        public string ShipmentSeq { get; set; }
        public string DestinationFacilityID { get; set; }
        public string ShipmentCost { get; set; }
    }

    public class Shipment
    {
        public string ShipmentID { get; set; }
        public List<Stop> Stop { get; set; }
    }

    public class OriginalOrder
    {
        public string TotalNumOfShipments { get; set; }
        public string FacilityID { get; set; }
        public string StoreNum { get; set; }
        public List<ProductDetail> ProductDetails { get; set; }
        public Shipment Shipment { get; set; }
    }

    public class ContextInformation
    {
        public string Message { get; set; }
        public string OriginFacilityID { get; set; }
    }

    public class DataProcessingResponse
    {
        public ContextInformation ContextInformation { get; set; }
        public List<OriginalOrder> OriginalOrder { get; set; }
    }

}