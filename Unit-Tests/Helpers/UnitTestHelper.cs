namespace AzureIntegration.UnitTestHelper
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Helps unit tests in generating results also in retireving test data.
    /// </summary>
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

         public static IEnumerable<object[]> GetShipmentJsons()
        {
            yield return new object[] { GetShipmentData1() };
            yield return new object[] { GetShipmentData2() };
        }
    }



}