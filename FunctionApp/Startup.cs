using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzureIntegration.Functions.Startup))]
namespace AzureIntegration.Functions
{
    using System.Diagnostics.CodeAnalysis;
    using AzureIntegration.Interfaces;
    using AzureIntegration.Interfaces.Services;
    using Microsoft.Extensions.DependencyInjection;

    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IBlobStorageService, BlobStorageService>();
            builder.Services.AddTransient<IServiceBusService, ServicebusService>();
        }
    }
}