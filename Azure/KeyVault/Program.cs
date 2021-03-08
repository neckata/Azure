using Amazon.KeyManagementService.Model;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Text;
using System.Threading.Tasks;

namespace KeyVault
{
    class Program
    {
        static string keyvaultURI = "https://azurevault2020.vault.azure.net/secrets/demosecret";


        static void Main(string[] args)
        {
            GetSecret().GetAwaiter().GetResult();
        }

        static public async Task GetSecret()
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();

            var keyVaultClient = new KeyVaultClient(
          new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            var secret = await keyVaultClient.GetSecretAsync(keyvaultURI)
                .ConfigureAwait(false);

            Console.WriteLine(secret.Value);
            Console.ReadKey();
        }

        static void GetSecret(string secretString)
        {
            string keyVaultUrl = "https://demovault2090.vault.azure.net/";
            var client = new SecretClient(vaultUri: new Uri(keyVaultUrl), credential: new DefaultAzureCredential());

            KeyVaultSecret secret = client.GetSecret(secretString);
            Console.WriteLine(secret.Value);
            Console.ReadKey();
        }
    }
}
