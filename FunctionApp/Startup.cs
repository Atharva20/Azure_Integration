using AzureAutomation.Interfaces;
using AzureAutomation.Interfaces.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzureAutomation.Functions.Startup))]

namespace AzureAutomation.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IBlobStorageService,BlobStorageService>();
        }
    }
}