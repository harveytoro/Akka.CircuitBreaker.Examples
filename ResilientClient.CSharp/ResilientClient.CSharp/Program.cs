using System;
using System.Configuration;
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
            Console.WriteLine("Press Return to exit:");
            var actorSystem = ActorSystem.Create("AResilientRESTClient");
            var client = actorSystem.ActorOf(Props.Create(() => new WhimsyClientActor(whimsyServerAddress)), "WhimsyClientActor");
            client.Tell(GetData.TakesForever());
            client.Tell(GetData.AlwaysWorks());
            client.Tell(GetData.RandomlyFails());
            client.Tell(GetData.RandomlyFails());
            client.Tell(GetData.RandomlyFails());
            client.Tell(GetData.TakesForever());
            client.Tell(GetData.RandomlyFails());
            client.Tell(GetData.RandomlyFails());
            Console.ReadLine();
        }
    }
}
