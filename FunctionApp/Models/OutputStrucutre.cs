using System.Collections.Generic;

namespace AzureAutomation.Models
{
    /// <summary>
    /// Provides the OutputStrucutre.
    /// </summary>
    public class OutputStrucutre
    {
        public string OriginFacilityID { get; set; }
        public List<string> ResponseContent { get; set; }
    }
}