using Azure;
using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Azure.Management.Storage;
using Microsoft.Azure.Management.Storage.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

class Program
{
    static async Task Main(string[] args)
    {
        var credentials = new DefaultAzureCredential();

        var storageManagementClient = new StorageManagementClient(credentials)
        {
            SubscriptionId = "88f14899-8abd-4cea-96bd-05bd66fd9dae"
        };

        var resourceGroupName = "macbeee-sdk-rg";
        var storageAccountName = "blobstoragebymacbeee";

        if (await storageManagementClient.ResourceGroups.CheckExistenceAsync(resourceGroupName) == false)
        {
            await storageManagementClient.ResourceGroups.CreateOrUpdateAsync(resourceGroupName, new Microsoft.Azure.Management.ResourceManager.Models.ResourceGroup());
        }

        var storageAccountParameters = new StorageAccountCreateParameters
        {
            Kind = Kind.StorageV2,
            Location = "westus",
            Sku = new Sku
            {
                Name = SkuName.StandardLRS
            }
        };

        var storageAccount = await storageManagementClient.StorageAccounts.CreateAsync(resourceGroupName, storageAccountName, storageAccountParameters);

        var keys = await storageManagementClient.StorageAccounts.ListKeysAsync(resourceGroupName, storageAccountName);
        var connectionString = $"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={keys.Keys.First().Value};EndpointSuffix=core.windows.net";
        Console.WriteLine($"Storage Account Connection String: {connectionString}");
    }
}
