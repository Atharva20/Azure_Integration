namespace AzureAutomation.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AzureAutomation.Models;
    using Newtonsoft.Json;
    using Microsoft.Extensions.Logging;
    using AzureAutomation.Interfaces;
    using Microsoft.Extensions.DependencyModel;

    public class ShipmentDataTransformation
    {
        private readonly ILogger logger;
        private readonly IBlobStorageService blobStorageService;

        OutputStrucutre outputStrucutre = new();

        public ShipmentDataTransformation(ILogger logger, IBlobStorageService blobStorageService)
        {
            this.logger = logger;
            this.blobStorageService = blobStorageService;
        }

        public List<string> RouteJsonData(List<DataProcessingResponse> allShipments)
        {
            List<string> transformedCSVData = new();
            foreach (var shipment in allShipments)
            {
                transformedCSVData.AddRange(TransformJsonToCsv(shipment));
            }
            return new List<string>
            {
                string.Join(Environment.NewLine,transformedCSVData.ToArray())
            };
        }

        public List<string> TransformJsonToCsv(DataProcessingResponse jsonInput)
        {
            List<string> uniqueProducts = new();
            List<string> transformedOutput = new();
            List<SummationPerProductIDs> summationPerProductIDs = new();
            List<currentStoreData> latestStoreData = new();
            List<processedProduct> allprocessedProducts = new();
            List<processedShipment> allprocessedShipments = new();
            currentStoreData currentStoreData = new();
            List<OriginalOrder> allOriginalOrders = jsonInput.OriginalOrder.ToList();
            int sequnceNum = 0;
            string originiFacilityID = jsonInput.ContextInformation.OriginFacilityID;
            outputStrucutre.OriginFacilityID = originiFacilityID;
            ProcessAllOriginalOrders(allOriginalOrders, originiFacilityID, allprocessedProducts, allprocessedShipments);
            var groupOfProducts = allprocessedProducts.GroupBy(b => b.productID);
            foreach (var eachproductID in groupOfProducts)
            {
                int sumOfinvoiceCosts = eachproductID.Sum(p => p.invoiceCost);
                string productID = eachproductID.Key;
                summationPerProductIDs.Add(new SummationPerProductIDs()
                {
                    productIdInvcCostSummation = sumOfinvoiceCosts,
                    storeID = productID,
                });
            }

            foreach (var processedProducts in allprocessedProducts)
            {
                foreach (var processedShipments in allprocessedShipments)
                {
                    if (processedProducts.productID == processedShipments.shipmentSeqNum)
                    {
                        currentStoreData.storeID = processedProducts.productID;
                        currentStoreData.shipmentCost = processedShipments.shipmentCost;
                        currentStoreData.destinationFacilityID = processedShipments.destinationFacilityID;
                        latestStoreData.Add(currentStoreData);
                        break;
                    }
                }
                if (!uniqueProducts.Contains(processedProducts.productID))
                {
                    int headerInvoiceCost = summationPerProductIDs.Where(p => p.storeID == processedProducts.productID).FirstOrDefault().productIdInvcCostSummation;
                    sequnceNum = 1;
                    string headerSeqNum = Convert.ToString(sequnceNum).PadLeft(3, '0');
                    sequnceNum++;
                    string detailSeqNum = Convert.ToString(sequnceNum).PadLeft(3, '0');
                    string headerShipmentCost = latestStoreData.Where(p => p.storeID == processedProducts.productID).FirstOrDefault().shipmentCost;
                    string headerDestinationStore = latestStoreData.Where(p => p.storeID == processedProducts.productID).FirstOrDefault().destinationFacilityID;
                    string headerData = string.Join(",", headerSeqNum, processedProducts.productID, processedProducts.originFacilityID, headerDestinationStore, Convert.ToString(headerInvoiceCost), headerShipmentCost);
                    string detailData = string.Join(",", detailSeqNum, processedProducts.whpkQty, processedProducts.whpkeCost);
                    transformedOutput.Add(headerData);
                    transformedOutput.Add(detailData);
                    uniqueProducts.Add(processedProducts.productID);
                }
                else
                {
                    sequnceNum++;
                    string detailSeqNum = Convert.ToString(sequnceNum).PadLeft(3, '0');
                    string detailData = string.Join(",", detailSeqNum, processedProducts.whpkQty, processedProducts.whpkeCost);
                    transformedOutput.Add(detailData);
                }
            }
            return transformedOutput;
        }

        public void ProcessAllOriginalOrders(List<OriginalOrder> allOrgOrders, string originiFacilityID, List<processedProduct> allprocessedProducts, List<processedShipment> allprocessedShipments)
        {
            foreach (var originalOrder in allOrgOrders)
            {
                ProcessAllProductDetails(originalOrder, originiFacilityID, allprocessedProducts);
                ProcessAllShipments(originalOrder, allprocessedShipments);
            }
        }

        public static void ProcessAllProductDetails(OriginalOrder originalOrder, string originiFacilityID, List<processedProduct> allprocessedProducts)
        {

            foreach (var products in originalOrder.ProductDetails)
            {
                processedProduct processedProduct = new()
                {
                    originFacilityID = originiFacilityID,
                    productID = products.ProductID,
                    whpkQty = products.ItemDefination.WhpkQty,
                    whpkeCost = products.ItemDefination.WhpkCost,
                    invoiceCost = products.ItemDefination.InvoiceCost
                };
                allprocessedProducts.Add(processedProduct);
            }
        }

        public static void ProcessAllShipments(OriginalOrder originalOrder, List<processedShipment> allprocessedShipments)
        {
            foreach (var shipment in originalOrder.Shipment.Stop)
            {
                processedShipment processedShipment = new()
                {
                    shipmmentID = originalOrder.Shipment.ShipmentID,
                    destinationFacilityID = shipment.DestinationFacilityID,
                    shipmentCost = shipment.ShipmentCost,
                    shipmentSeqNum = shipment.ShipmentSeq
                };
                allprocessedShipments.Add(processedShipment);
            }
        }
    }
}