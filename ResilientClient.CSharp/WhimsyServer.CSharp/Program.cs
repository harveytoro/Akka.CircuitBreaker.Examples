using System;
using System.Configuration;
using Nancy.Hosting.Self;

namespace WhimsyServer.CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var nancyHost = $"{ConfigurationManager.AppSettings["address"]}:{ConfigurationManager.AppSettings["port"]}";
            var nancyConfig = new HostConfiguration();
            nancyConfig.UrlReservations.CreateAutomatically = true;
            using (var host = new NancyHost(nancyConfig, new Uri(nancyHost)))
            {
                host.Start();
                Console.WriteLine($"Running on {nancyHost}. Press Return to quit.");
                Console.ReadLine();
            }
        }
    }
}
