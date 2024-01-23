namespace AzureIntegration.Transformation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AzureIntegration.Models;

    /// <summary>
    /// Converts the shipment jsons to csv format.
    /// </summary>
    public class ShipmentDataTransformation
    {
        /// <summary>
        /// Extracts the json to List and Transforms it to a CSV output.
        /// </summary>
        /// <param name="jsonInput">json structure</param>
        /// <returns></returns>
        public OutputStrucutre TransformJsonToCsv(DataProcessingResponse jsonInput)
        {
            List<string> uniqueProducts = new();
            List<string> transformedOutput = new();
            List<SummationPerProductIDs> summationPerProductIDs = new();
            List<CurrentStoreData> latestStoreData = new();
            List<ProcessedProduct> allprocessedProducts = new();
            List<ProcessedShipment> allprocessedShipments = new();
            CurrentStoreData currentStoreData = new();
            OutputStrucutre outputStrucutre = new();
            List<OriginalOrder> allOriginalOrders = jsonInput.OriginalOrder.ToList();
            int sequnceNum = 0;
            string originiFacilityID = jsonInput.ContextInformation.OriginFacilityID;
            outputStrucutre.OriginFacilityID = originiFacilityID;
            ProcessAllOriginalOrders(allOriginalOrders, originiFacilityID, allprocessedProducts, allprocessedShipments);
            var groupOfProducts = allprocessedProducts.GroupBy(b => b.ProductID);
            foreach (var eachproductID in groupOfProducts)
            {
                int sumOfinvoiceCosts = eachproductID.Sum(p => p.InvoiceCost);
                string productID = eachproductID.Key;
                summationPerProductIDs.Add(new SummationPerProductIDs()
                {
                    ProductIdInvcCostSummation = sumOfinvoiceCosts,
                    storeID = productID,
                });
            }

            foreach (var processedProducts in allprocessedProducts)
            {
                foreach (var processedShipments in allprocessedShipments)
                {
                    if (processedProducts.ProductID == processedShipments.ShipmentSeqNum)
                    {
                        currentStoreData.StoreID = processedProducts.ProductID;
                        currentStoreData.ShipmentCost = processedShipments.ShipmentCost;
                        currentStoreData.DestinationFacilityID = processedShipments.DestinationFacilityID;
                        latestStoreData.Add(currentStoreData);
                        break;
                    }
                }
                if (!uniqueProducts.Contains(processedProducts.ProductID))
                {
                    int headerInvoiceCost = summationPerProductIDs.Where(p => p.storeID == processedProducts.ProductID).FirstOrDefault().ProductIdInvcCostSummation;
                    sequnceNum = 1;
                    string headerSeqNum = Convert.ToString(sequnceNum).PadLeft(3, '0');
                    sequnceNum++;
                    string detailSeqNum = Convert.ToString(sequnceNum).PadLeft(3, '0');
                    string headerShipmentCost = latestStoreData.Where(p => p.StoreID == processedProducts.ProductID).FirstOrDefault().ShipmentCost;
                    string headerDestinationStore = latestStoreData.Where(p => p.StoreID == processedProducts.ProductID).FirstOrDefault().DestinationFacilityID;
                    string headerData = string.Join(",", headerSeqNum, processedProducts.ProductID, processedProducts.OriginFacilityID, headerDestinationStore, Convert.ToString(headerInvoiceCost), headerShipmentCost);
                    string detailData = string.Join(",", detailSeqNum, processedProducts.WhpkQty, processedProducts.WhpkeCost);
                    transformedOutput.Add(headerData);
                    transformedOutput.Add(detailData);
                    uniqueProducts.Add(processedProducts.ProductID);
                }
                else
                {
                    sequnceNum++;
                    string detailSeqNum = Convert.ToString(sequnceNum).PadLeft(3, '0');
                    string detailData = string.Join(",", detailSeqNum, processedProducts.WhpkQty, processedProducts.WhpkeCost);
                    transformedOutput.Add(detailData);
                }
            }
            outputStrucutre.ResponseContent = string.Join(Environment.NewLine, transformedOutput.ToArray());
            
            return outputStrucutre;
        }

        /// <summary>
        /// Iterates over the json to transform the productDetails and Shipments.
        /// </summary>
        /// <param name="allOrgOrders">Json to extract the productDetails and Shipments.</param>
        /// <param name="originiFacilityID"></param>
        /// <param name="allprocessedProducts"></param>
        /// <param name="allprocessedShipments"></param>
        public void ProcessAllOriginalOrders(List<OriginalOrder> allOrgOrders, string originiFacilityID, List<ProcessedProduct> allprocessedProducts, List<ProcessedShipment> allprocessedShipments)
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