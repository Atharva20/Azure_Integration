namespace AzureIntegration.Models
{
    /// <summary>
    /// Iterating over the jsons productDetails data.
    /// </summary>
    public class ProcessedProduct
    {
        public string ProductID { get; set; }
        public string OriginFacilityID { get; set; }
        public int InvoiceCost { get; set; }
        public string WhpkeCost { get; set; }
        public string WhpkQty { get; set; }
    }

    /// <summary>
    /// Iterating over the jsons processedShipment data.
    /// </summary>
    public class ProcessedShipment
    {

        public string DestinationFacilityID { get; set; }
        public string ShipmentCost { get; set; }
        public string ShipmmentID { get; set; }
        public string ShipmentSeqNum { get; set; }

    }

    /// <summary>
    /// Iterating over the jsons SummationPerProductIDs data.
    /// </summary>
    public class SummationPerProductIDs
    {
        public int ProductIdInvcCostSummation { get; set; }
        public string StoreID { get; set; }
    }

    /// <summary>
    /// Iterating over the jsons currentStoreData data.
    /// </summary>
    public class CurrentStoreData
    {
        public string StoreID { get; set; }
        public string ShipmentCost { get; set; }
        public string DestinationFacilityID { get; set; }
    }

}