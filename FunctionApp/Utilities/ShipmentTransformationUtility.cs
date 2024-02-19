namespace AzureIntegration.Utility
{
    using System.Collections.Generic;
    using AzureIntegration.Models;
    
    /// <summary>
    /// Converts the shipment jsons to csv format.
    /// </summary>
    public class ShipmentTransformationUtility
    {
        /// <summary>
        /// Iterates over the json to transform the productDetails and Shipments.
        /// </summary>
        /// <param name="allOrgOrders">Json to extract the productDetails and Shipments.</param>
        /// <param name="originiFacilityID"></param>
        /// <param name="allprocessedProducts"></param>
        /// <param name="allprocessedShipments"></param>
        public static void ProcessAllOriginalOrders(List<OriginalOrder> allOrgOrders, string originiFacilityID, List<ProcessedProduct> allprocessedProducts, List<ProcessedShipment> allprocessedShipments)
        {
            foreach (var originalOrder in allOrgOrders)
            {
                ProcessAllProductDetails(originalOrder, originiFacilityID, allprocessedProducts);
                ProcessAllShipments(originalOrder, allprocessedShipments);
            }
        }
        /// <summary>
        /// Transforms all the productDetails in the json
        /// </summary>
        /// <param name="originalOrder">Object of the json to extract the productDetails.</param>
        /// <param name="originiFacilityID">originiFacilityID.</param>
        /// <param name="allprocessedProducts">List that holds the transformed productDetails</param>
        public static void ProcessAllProductDetails(OriginalOrder originalOrder, string originiFacilityID, List<ProcessedProduct> allprocessedProducts)
        {

            foreach (var products in originalOrder.ProductDetails)
            {
                ProcessedProduct processedProduct = new()
                {
                    OriginFacilityID = originiFacilityID,
                    ProductID = products.ProductID,
                    WhpkQty = products.ItemDefination.WhpkQty,
                    WhpkeCost = products.ItemDefination.WhpkCost,
                    InvoiceCost = products.ItemDefination.InvoiceCost
                };
                allprocessedProducts.Add(processedProduct);
            }
        }

        /// <summary>
        /// Transforms all the shipments within the json.
        /// </summary>
        /// <param name="originalOrder">Object of the json to extract the shipments.</param>
        /// <param name="allprocessedShipments">List that holds the transformed shipments.</param>
        public static void ProcessAllShipments(OriginalOrder originalOrder, List<ProcessedShipment> allprocessedShipments)
        {
            foreach (var shipment in originalOrder.Shipment.Stop)
            {
                ProcessedShipment processedShipment = new()
                {
                    ShipmmentID = originalOrder.Shipment.ShipmentID,
                    DestinationFacilityID = shipment.DestinationFacilityID,
                    ShipmentCost = shipment.ShipmentCost,
                    ShipmentSeqNum = shipment.ShipmentSeq
                };
                allprocessedShipments.Add(processedShipment);
            }
        }

    }
}