namespace AzureIntegration.Transformation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AzureIntegration.Models;
    using AzureIntegration.Utility;
    using Newtonsoft.Json;

    /// <summary>
    /// Converts the shipment jsons to csv format.
    /// </summary>
    public class ShipmentDataTransformation : IDisposable
    {
        /// <summary>
        /// Extracts the json to List and Transforms it to a CSV output.
        /// </summary>
        /// <param name="jsonInput">json structure</param>
        /// <returns></returns>
        public static OutputStrucutre TransformJsonToCsv(string rawShipmentJsonData)
        {
            DataProcessingResponse shipmentJsonData = JsonConvert.DeserializeObject<DataProcessingResponse>(rawShipmentJsonData);
            List<string> uniqueProducts = new();
            List<string> transformedOutput = new();
            List<SummationPerProductIDs> summationPerProductIDs = new();
            List<CurrentStoreData> latestStoreData = new();
            List<ProcessedProduct> allprocessedProducts = new();
            List<ProcessedShipment> allprocessedShipments = new();
            ShipmentTransformationUtility shipmentTransformationUtility = new();
            CurrentStoreData currentStoreData = new();
            OutputStrucutre outputStrucutre = new();
            List<OriginalOrder> allOriginalOrders = shipmentJsonData.OriginalOrder.ToList();
            int sequnceNum = 0;
            string originiFacilityID = shipmentJsonData.ContextInformation.OriginFacilityID;
            outputStrucutre.OriginFacilityID = originiFacilityID;
            ShipmentTransformationUtility.ProcessAllOriginalOrders(allOriginalOrders, originiFacilityID, allprocessedProducts, allprocessedShipments);
            var groupOfProducts = allprocessedProducts.GroupBy(b => b.ProductID);
            foreach (var eachproductID in groupOfProducts)
            {
                int sumOfinvoiceCosts = eachproductID.Sum(p => p.InvoiceCost);
                string productID = eachproductID.Key;
                summationPerProductIDs.Add(new SummationPerProductIDs()
                {
                    ProductIdInvcCostSummation = sumOfinvoiceCosts,
                    StoreID = productID,
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
                    int headerInvoiceCost = summationPerProductIDs.Where(p => p.StoreID == processedProducts.ProductID).FirstOrDefault().ProductIdInvcCostSummation;
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

        public void Dispose()
        {
        }
    }
}