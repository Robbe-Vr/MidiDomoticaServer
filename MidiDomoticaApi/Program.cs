using MidiDomoticaApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SYWCentralLogging;
using MidiDomotica.Core;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace MidiDomoticaApi
{
    public class Program
    {
        public static IHost WebHost { get; private set; }

        public static MidiManager MidiManager { get; set; }

        public static void Main(string[] args)
        {
            WebHost = CreateHostBuilder(args).Build();

            try
            {
                string executionFolderAppSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                if (!File.Exists(executionFolderAppSettingsPath))
                {
                    Logger.Log("Copy of appsettings.json for Web Api missing! Copying from installation folder.");

                    string appsettingsFile = Path.Combine("D:", "My_Software_Apps", "MidiDomotica", "Api", "appsettings.json");
                    File.Copy(appsettingsFile, executionFolderAppSettingsPath);
                }

                var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json").Build();

                var section = config.GetSection("Authorization");

                string appPassword = section["AppPassword"];
                SetApiPassword(appPassword);

                string appSecret = section["AppSecret"];
                Startup.SetApiSecret(appSecret);

                Logger.Log("MidiDomotica Web Api initialized!");
            }
            catch (Exception e)
            {
                Logger.Log("Failed to setup authorization for MidiDomotica Web Api!\nError: " + e.Message);
                return;
            }

            WebHost.Run();
        }

        public static void SetApiPassword(string pw)
        {
            Startup.SetUnhashedApiPassword(pw);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((services) =>
                    {
                        services.AddSingleton(x => MidiManager);
                    });

                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://192.168.2.101:55555");
                });
    }
}
