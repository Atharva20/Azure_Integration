using AzureIntegration.Interfaces;
using AzureIntegration.Interfaces.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzureIntegration.Functions.Startup))]

namespace AzureIntegration.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IBlobStorageService, BlobStorageService>();
            builder.Services.AddTransient<IServiceBusService, ServicebusService>();
        }
    }
}