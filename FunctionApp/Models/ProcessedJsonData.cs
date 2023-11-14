namespace AzureAutomation.Models
{
    using System;

    public class processedProduct
    {
        public string productID{get;set;}
        public string originFacilityID{get;set;}
        public int invoiceCost{get;set;}
        public string whpkeCost{get;set;}
        public string whpkQty{get;set;}
        public string storeNum {get;set;}

    }

    public class processedShipment
    {
        
        public string  destinationFacilityID{get;set;}
        public string shipmentCost{get;set;}
        public string shipmmentID{get;set;}
        public string shipmentSeqNum{get;set;}

    }

     public class SummationPerProductIDs
    {
        public int productIdInvcCostSummation {get;set;}
        public string storeID{get;set;}
    }

    public class currentStoreData
    {
        public string storeID{get;set;}
        public string shipmentCost{get;set;}
        public string destinationFacilityID{get;set;}
    }


}