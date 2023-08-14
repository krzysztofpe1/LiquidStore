using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using StoreServer.DatabaseModels;

namespace StoreServer
{
    internal class Program
    {
        public static IServiceCollection Services { get; private set; }
        public static IServiceProvider Provider { get; private set; }
        static void Main(string[] args)
        {
            Services = new ServiceCollection();
            Services.AddSingleton<StoreDbService, StoreDbService>();
            Provider = Services.BuildServiceProvider();

            var dbService = Provider.GetRequiredService<StoreDbService>();
        }
    }
}