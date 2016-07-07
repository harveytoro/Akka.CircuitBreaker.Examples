using System;
using System.Configuration;
using System.Threading;
using Akka.Actor;
using ResilientClient.CSharp.Actors;
using ResilientClient.CSharp.Messages;

namespace ResilientClient.CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var whimsyServerAddress = $"{ConfigurationManager.AppSettings["address"]}:{ConfigurationManager.AppSettings["port"]}";
            
            var actorSystem = ActorSystem.Create("AResilientRESTClient");
            var client = actorSystem.ActorOf(Props.Create(() => new WhimsyClientActor(new Uri(whimsyServerAddress), TimeSpan.FromSeconds(2))), "WhimsyClientActor");
            Console.WriteLine("Starting in 3 seconds.");
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Console.WriteLine("Press Return to exit:");
            Communicate(client);
            Communicate(client);
            Communicate(client);
            Communicate(client);

            Console.ReadLine();
        }

        static void Communicate(IActorRef client)
        {
            client.Tell(GetData.TakesForever());
            client.Tell(GetData.AlwaysWorks());
            client.Tell(GetData.RandomlyFails());
        }
    }
}
