namespace AzureIntegration.UnitTestHelper
{
    using System.IO;

    public class UnitTestHelper
    {
        public static string GetShipmentData1()
        {
            string localFilePath = @"./Data/SampleShipment1.txt";
            string shipmentData1 = File.ReadAllText(localFilePath);
            return shipmentData1;
        }

        public static string GetShipmentData2()
        {
            string localFilePath = @"./Data/SampleShipment2.txt";
            string shipmentData2 = File.ReadAllText(localFilePath);
            return shipmentData2;
        }
    }



}