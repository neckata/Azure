using Microsoft.Azure.Management.Compute.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using System;

namespace VM
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }


        private static void CreateVM()
        {
            var azure = Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(null)
                .WithDefaultSubscription();
     
            var vmName = "test-wp1-eus-vm";

            azure.VirtualMachines.Define(vmName)
                .WithRegion(Region.USEast)
                .WithExistingResourceGroup("TestResourceGroup")
                .WithExistingPrimaryNetworkInterface(null)
                .WithLatestWindowsImage("MicrosoftWindowsServer", "WindowsServer", "2012-R2-Datacenter")
                .WithAdminUsername("jonc")
                .WithAdminPassword("aReallyGoodPasswordHere")
                .WithComputerName(vmName)
                .WithSize(VirtualMachineSizeTypes.StandardDS1)
                .Create();
        }
    }
}
