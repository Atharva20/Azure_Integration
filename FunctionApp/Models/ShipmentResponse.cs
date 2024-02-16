using System.Diagnostics.CodeAnalysis;

namespace AzureIntegration.Models
{
    /// <summary>
    /// Provides the output strucutre for the shipment response of GetShipmentsFromClient.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ShipmentResponse
    {
        public int TotalShipmentsReceieved {get;set;}
        public int TotalShipmentsTransformed { get; set; }
        public string OutputTransformedLoc { get; set; }
    }
}
