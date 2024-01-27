namespace AzureIntegration.UnitTests
{
    using Xunit;
    using AzureIntegration.UnitTestHelper;
    using AzureIntegration.Models;
    using AzureIntegration.Transformation;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Performs unit testing for the ShipmentDataTransformation Function
    /// </summary>
    public class ShipmentDataTransformationTest : UnitTestHelper
    {

        [Fact]
        public void ShipmentDataTransformationTest_ClientDataDoesNotExist_ReturnsException()
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                var csvShipmentOut = ShipmentDataTransformation.TransformJsonToCsv(null);
            });

        }
        
        [Fact]
        public void ShipmentDataTransformationTest_ClientDataExist_ReturnsOriginFacilityIDLength() //methodname_scenario_expectation
        {
            //Arrange
            int lengthOfFacilityId = 4;

            //Act
            var csvShipmentOut = ShipmentDataTransformation.TransformJsonToCsv(GetShipmentData1());

            //Assert
            Assert.Equal(lengthOfFacilityId, csvShipmentOut.OriginFacilityID.Length);
        }

        [Theory]
        [MemberData(nameof(GetShipmentJsons))]
        public void ShipmentDataTransformationTest_ClientDataExist_ReturnsResponseContentHeaderData(string inputData)
        {
            //Arrange
            int numOfFieldsInHeaderLine = 6;
            int lengthOfTheGroupIDField = 3;

            //Act
            var csvShipmentOut = ShipmentDataTransformation.TransformJsonToCsv(inputData);

            //Assert
            var result = csvShipmentOut.ResponseContent.Split("\r\n");
            var headerData = result[0].Split(",");
            var groupIDLength = headerData[0].Length;

            Assert.Equal(numOfFieldsInHeaderLine, headerData.Length);
            Assert.Equal(lengthOfTheGroupIDField, groupIDLength);
        }

        [Theory]
        [MemberData(nameof(GetShipmentJsons))]
        public void ShipmentDataTransformationTest_ClientDataExist_ReturnsResponseContentDetailData(string inputData)
        {
            //Arrange
            int numOfFieldsInDetailLine = 3;
            int lengthOfTheGroupIDField = 3;

            //Act
            var csvShipmentOut = ShipmentDataTransformation.TransformJsonToCsv(inputData);

            //Assert
            var result = csvShipmentOut.ResponseContent.Split("\r\n");
            var detailData = result[1].Split(",");
            var groupIDLength = detailData[0].Length;

            Assert.Equal(numOfFieldsInDetailLine, detailData.Length);
            Assert.Equal(lengthOfTheGroupIDField, groupIDLength);
        }
    }
}
